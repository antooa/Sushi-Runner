using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReturnTrue.AspNetCore.Identity.Anonymous;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;

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

            var mealss = from meal in meals
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
                    Meals = mealss,
                    CartAmount = cart.Items.Count
                });
        }

        public IActionResult Error()
        {
            return View(
                new ErrorModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
        }

        public async Task<IActionResult> AddToCart(long id)
        {
            var user = await _accountService.GetLoggedUserOrCreateAnonymous(
                HttpContext.User,
                HttpContext.Features.Get<IAnonymousIdFeature>()?.AnonymousId);
            _cartService.AddItem(user, id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromCart(long id)
        {
            var user = await _accountService.GetLoggedUserOrCreateAnonymous(
                HttpContext.User,
                HttpContext.Features.Get<IAnonymousIdFeature>()?.AnonymousId);
            _cartService.RemoveItem(user, id);
            return RedirectToAction("Index");
        }
    }
}
