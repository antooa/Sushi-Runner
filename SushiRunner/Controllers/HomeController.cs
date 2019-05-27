using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReturnTrue.AspNetCore.Identity.Anonymous;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;
using SushiRunner.ViewModels.Home;

namespace SushiRunner.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMealService _mealService;
        private readonly IMealGroupService _mealGroupService;
        private readonly IAccountService _accountService;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public HomeController(IMealService mealService, IMealGroupService mealGroupService,
            IAccountService accountService, ICartService cartService, IMapper mapper)
        {
            _mealService = mealService;
            _mealGroupService = mealGroupService;
            _accountService = accountService;
            _cartService = cartService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var meals = _mealService.GetList();
            var user = await _accountService.GetLoggedUserOrCreateAnonymous(
                HttpContext.User,
                HttpContext.Features.Get<IAnonymousIdFeature>()?.AnonymousId);
            var mealsWithCartCheckbox = _mealService.GetMealsWithCartCheckbox(user, meals);
            var mealModels = mealsWithCartCheckbox.Select(meal => _mapper.Map<MealDTO, MealModel>(meal)).ToList();

            return View(
                new HomeModel
                {
                    Meals = mealModels,
                });
        }

        [Route("/Home/MealGroups/{mealGroupId}")]
        public async Task<IActionResult> MealGroups(long mealGroupId)
        {
            var user = await _accountService.GetLoggedUserOrCreateAnonymous(
                HttpContext.User,
                HttpContext.Features.Get<IAnonymousIdFeature>()?.AnonymousId);
            var meals = _mealService.GetByGroupId(mealGroupId);
            var mealsWithCartCheckbox = _mealService.GetMealsWithCartCheckbox(user, meals);
            var mealModels = mealsWithCartCheckbox.Select(meal => _mapper.Map<MealDTO, MealModel>(meal)).ToList();

            return View("Index",
                new HomeModel
                {
                    Meals = mealModels,
                    Title = _mealGroupService.Get(mealGroupId).Name
                });
        }

        public async Task<IActionResult> Cart()
        {
            var user = await GetUserAsync();
            var cart = _cartService.GetByUserOrCreateNew(user);
            var cartModel = _mapper.Map<CartDTO, CartModel>(cart);
            var (_, totalPrice) = await _cartService.CountAndTotalPrice(User);
            cartModel.OrderModel = new OrderModel
            {
                TotalPrice = totalPrice
            };
            return View(cartModel);
        }

        public async Task<IActionResult> AddToCart(long id, string redirectPath)
        {
            var user = await GetUserAsync();
            _cartService.AddItem(user, id);
            return HandleRedirect(redirectPath);
        }

        public async Task<IActionResult> RemoveFromCart(long id, string redirectPath)
        {
            var user = await GetUserAsync();
            _cartService.RemoveItem(user, id);
            return HandleRedirect(redirectPath);
        }

        public async Task<IActionResult> ChangeCartItemAmount(long id, int cartItemAmount, string redirectPath)
        {
            var user = await GetUserAsync();
            _cartService.ChangeItemAmount(user, id, cartItemAmount);
            return HandleRedirect(redirectPath);
        }

        public IActionResult Error()
        {
            return View(
                new ErrorModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
        }

        public async Task<IAccountService> MakeOrder(OrderModel orderModel)
        {
            return null;
        }

        private async Task<User> GetUserAsync()
        {
            return await _accountService.GetLoggedUserOrCreateAnonymous(
                HttpContext.User,
                HttpContext.Features.Get<IAnonymousIdFeature>()?.AnonymousId);
        }

        private IActionResult HandleRedirect(string redirectPath)
        {
            if (redirectPath == null)
            {
                redirectPath = "/";
            }

            return Redirect(redirectPath);
        }
    }
}
