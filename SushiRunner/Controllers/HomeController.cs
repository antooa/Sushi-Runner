using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ReturnTrue.AspNetCore.Identity.Anonymous;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;
using SushiRunner.ViewModels.Home;

namespace SushiRunner.Controllers
{
    public class HomeController : Controller
    {
        private IMealService _mealService;
        private IAccountService _accountService;
        private ICartService _cartService;

        public HomeController(IMealService mealService, IAccountService accountService, ICartService cartService)
        {
            _mealService = mealService;
            _accountService = accountService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var meals = _mealService.GetList();
            var user = await _accountService.GetLoggedUserOrCreateAnonymous(
                HttpContext.User,
                HttpContext.Features.Get<IAnonymousIdFeature>()?.AnonymousId);
            var cart = _cartService.GetByUserOrCreateNew(user);

            var mealsWithCartCheckbox = from meal in meals
                join cartItem in cart.Items on meal.Id equals cartItem.MealId into mealModels
                from m in mealModels.DefaultIfEmpty()
                select new MealModel
                {
                    Id = meal.Id,
                    Name = meal.Name,
                    Description = meal.Description,
                    Price = meal.Price,
                    ImagePath = meal.ImagePath,
                    Weight = meal.Weight,
                    IsInCart = m != null
                };

            return View(
                new HomeModel
                {
                    Meals = mealsWithCartCheckbox,
                });
        }

        public async Task<IActionResult> Cart()
        {
            var user = await _accountService.GetLoggedUserOrCreateAnonymous(
                HttpContext.User,
                HttpContext.Features.Get<IAnonymousIdFeature>()?.AnonymousId);
            var cart = _cartService.GetByUserOrCreateNew(user);
            return View(
                new CartModel
                {
                    Items = cart.Items
                        .Select(item =>
                            new CartItemModel
                            {
                                Meal = new MealModel
                                {
                                    Id = item.Meal.Id,
                                    Name = item.Meal.Name,
                                    Description = item.Meal.Description,
                                    Weight = item.Meal.Weight,
                                    ImagePath = item.Meal.ImagePath,
                                    Price = item.Meal.Price
                                },
                                Amount = item.Amount
                            }
                        )
                }
            );
        }

        public async Task<IActionResult> AddToCart(long id)
        {
            var user = await _accountService.GetLoggedUserOrCreateAnonymous(
                HttpContext.User,
                HttpContext.Features.Get<IAnonymousIdFeature>()?.AnonymousId);
            _cartService.AddItem(user, id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromCart(long id, string redirectView)
        {
            var user = await _accountService.GetLoggedUserOrCreateAnonymous(
                HttpContext.User,
                HttpContext.Features.Get<IAnonymousIdFeature>()?.AnonymousId);
            _cartService.RemoveItem(user, id);
            if (redirectView == null)
            {
                redirectView = "Index";
            }

            return RedirectToAction(redirectView);
        }

        public IActionResult Error()
        {
            return View(
                new ErrorModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
        }
    }
}
