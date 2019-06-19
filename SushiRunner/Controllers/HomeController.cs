using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SushiRunner.Services;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;
using SushiRunner.ViewModels;
using SushiRunner.ViewModels.Home;

namespace SushiRunner.Controllers
{
    public class HomeController : SushiRunnerBaseController
    {
        private readonly IMealService _mealService;
        private readonly IMealGroupService _mealGroupService;
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public HomeController(IMealService mealService, IMealGroupService mealGroupService,
            IAccountService accountService, ICartService cartService, IOrderService orderService,
            IMapper mapper) : base(accountService)
        {
            _mealService = mealService;
            _mealGroupService = mealGroupService;
            _cartService = cartService;
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var user = await GetLoggedUserOrCreateAnonymous();
            var mealModels = _mealService.GetMealsWithCartCheckbox(user)
                .Select(meal => _mapper.Map<MealDTO, MealModel>(meal))
                .ToList();

            return View(
                new HomeModel
                {
                    Meals = mealModels,
                });
        }

        [Route("/Home/MealGroups/{mealGroupId}")]
        public async Task<IActionResult> MealGroups(long mealGroupId)
        {
            var user = await GetLoggedUserOrCreateAnonymous();
            var mealModels = _mealService.GetMealsWithCartCheckbox(user, mealGroupId)
                .Select(meal => _mapper.Map<MealDTO, MealModel>(meal))
                .ToList();

            return View("Index",
                new HomeModel
                {
                    Meals = mealModels,
                    Title = _mealGroupService.Get(mealGroupId).Name
                });
        }

        public async Task<IActionResult> Cart()
        {
            var user = await GetLoggedUserOrCreateAnonymous();
            var cart = _cartService.GetByUserOrCreateNew(user);
            var cartModel = _mapper.Map<CartDTO, CartModel>(cart);
            cartModel.OrderModel = new MakeOrderFormModel
            {
                TotalPrice = (await _cartService.CountAndTotalPrice(User)).Item2
            };
            return View(cartModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(long id, string redirectPath)
        {
            var user = await GetLoggedUserOrCreateAnonymous();
            _cartService.AddItem(user, id);
            return HandleRedirect(redirectPath);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(long id, string redirectPath)
        {
            var user = await GetLoggedUserOrCreateAnonymous();
            _cartService.RemoveItem(user, id);
            return HandleRedirect(redirectPath);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeCartItemAmount(long id, int cartItemAmount, string redirectPath)
        {
            var user = await GetLoggedUserOrCreateAnonymous();
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

        [HttpPost]
        public async Task<IActionResult> MakeOrder(MakeOrderFormModel orderModel)
        {
            var user = await GetLoggedUserOrCreateAnonymous();
            var cart = _cartService.GetByUserOrCreateNew(user);
            _orderService.Create(
                user,
                orderModel.CustomerName,
                orderModel.PhoneNumber,
                orderModel.PaymentType,
                orderModel.Address,
                cart);
            _cartService.Clear(user);

            return View("ThankYou");
        }

        private IActionResult HandleRedirect(string redirectPath)
        {
            if (redirectPath == null)
            {
                redirectPath = "/";
            }

            return Redirect(redirectPath);
        }
        
        [Route("/Home/Meals/{mealId}")]
        public async Task<IActionResult> ViewMealDescription(long mealId)
        {
            var mealDescription = _mealService.Get(mealId);
            //var comment = _mealService.GetComment(mealId);
            return View("MealDescription", 
                new MealDescriptionModel{
                    Id = mealDescription.Id,
                    Name = mealDescription.Name,
                    Description = mealDescription.Description,
                    ImagePath = mealDescription.ImagePath,
                    Weight = mealDescription.Weight,
                    Price = mealDescription.Price,
                    GroupId = mealDescription.GroupId,
                    /*Comments = new CommentModel
                    {
                        Message = 
                        
                    }*/
                }
                );
        }
    }
}