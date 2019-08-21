using System;
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
                //.Where(meal => meal.isShown)
                .ToList();

            return View(
                new HomeModel
                {
                    Meals = mealModels
                });
        }

        public async Task<IActionResult> ShowAll(string redirectPath)
        {
            var user = await GetLoggedUserOrCreateAnonymous();
            var mealIds = _mealService.GetMealsWithCartCheckbox(user)
                .Select(meal => meal.Id)
                .ToList();
            foreach (var meal in mealIds)
            {
                _mealService.Show(meal);
            }
            return HandleRedirect(redirectPath);
        }
        
        public async Task<IActionResult> HideAll(string redirectPath)
        {
            var user = await GetLoggedUserOrCreateAnonymous();
            var mealIds = _mealService.GetMealsWithCartCheckbox(user)
                .Select(meal => meal.Id)
                .ToList();

            foreach (var meal in mealIds)
            {
                _mealService.Hide(meal);
            }
            return HandleRedirect(redirectPath);
        }

        public async Task<IActionResult> Hide(long mealId, string redirectPath)
        {
            var user = await GetLoggedUserOrCreateAnonymous();
            _mealService.Hide(mealId);
            var mealModels = _mealService.GetMealsWithCartCheckbox(user)
            .Select(meal => _mapper.Map<MealDTO, MealModel>(meal))
            .ToList();
            
            return HandleRedirect(redirectPath);
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
;
            _orderService.Create( new OrderDTO
                {
                    Address = orderModel.Address,
                    User = user,
                    CustomerName = orderModel.CustomerName,
                    PhoneNumber = orderModel.PhoneNumber,
                    PaymentType = orderModel.PaymentType,
                    TotalPrice = orderModel.TotalPrice,
                    Items = cart.Items.Select(i => new OrderItemDTO
                    {
                        Meal = i.Meal,
                        Amount = i.Amount
                    }).ToList()
                });
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
            var meal = _mealService.Get(mealId);
            //meal.Comments.Add
            return View("MealDescription", 
                new MealDescriptionModel{
                    Id = meal.Id,
                    Name = meal.Name,
                    Description = meal.Description,
                    ImagePath = meal.ImagePath,
                    Weight = meal.Weight,
                    Price = meal.Price,
                    Comments = meal.Comments.Select(comment => new CommentModel
                    {
                        Id = comment.Id,
                        Message = comment.Message,
                        CreationDate = comment.CreationDate,
                        Rating = comment.Rating
                    })
                    
                    
                }
                );
        }
        
        [HttpPost]
        public async Task<IActionResult> AddComment(long mealId, string message, int rating, string redirectPath)
        {
            
            _mealService.AddComment(mealId, message, rating);
            return HandleRedirect(redirectPath);
        }
    }
}