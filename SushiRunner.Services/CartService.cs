using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services.Dto;
using SushiRunner.Services.Interfaces;

namespace SushiRunner.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart, long> _cartRepository;
        private readonly IRepository<CartItem, long> _cardItemRepository;
        private readonly IRepository<Meal, long> _mealRepository;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public CartService(IRepository<Cart, long> cartRepository, IRepository<CartItem, long> cardItemRepository,
            IRepository<Meal, long> mealRepository, IAccountService accountService, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _cardItemRepository = cardItemRepository;
            _mealRepository = mealRepository;
            _accountService = accountService;
            _mapper = mapper;
        }

        private Cart _GetByUserOrCreateNew(User user)
        {
            var cart = _cartRepository.Search(c => c.User.Id == user.Id).FirstOrDefault();
            if (cart == null)
            {
                cart = new Cart
                {
                    User = user
                };
                _cartRepository.Create(cart);
            }

            return cart;
        }

        public CartDTO GetByUserOrCreateNew(User user)
        {
            var cart = _GetByUserOrCreateNew(user);
            return _mapper.Map<Cart, CartDTO>(cart);
        }

        public void AddItem(User user, long mealId)
        {
            var meal = _mealRepository.Get(mealId);
            if (meal == null)
            {
                throw new Exception($"Meal with Id={mealId} not found");
            }

            var cart = _GetByUserOrCreateNew(user);
            var cartItem = cart.Items.FirstOrDefault(item => item.Meal.Id == mealId);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    MealId = meal.Id,
                    CartId = cart.Id,
                    Amount = 1
                };
                _cardItemRepository.Create(cartItem);
                cart.Items.Add(cartItem);
            }
            else
            {
                cartItem.Amount++;
            }

            _cartRepository.Update(cart);
        }

        public void RemoveItem(User user, long mealId)
        {
            var cart = _GetByUserOrCreateNew(user);
            cart.Items = cart.Items.Where(it => it.MealId != mealId).ToList();
            _cartRepository.Update(cart);
        }

        public void ChangeItemAmount(User user, long mealId, int newAmount)
        {
            var cart = _GetByUserOrCreateNew(user);
            var cartItem = cart.Items.FirstOrDefault(item => item.MealId == mealId);
            if (cartItem == null)
            {
                return;
            }

            if (newAmount < 1)
            {
                newAmount = 1;
            }
            else if (newAmount > 99)
            {
                newAmount = 99;
            }

            cartItem.Amount = newAmount;
            _cartRepository.Update(cart);
        }

        public void Clear(User user)
        {
            var card = _GetByUserOrCreateNew(user);
            card.Items.Clear();
            _cartRepository.Update(card);
        }

        public async Task<(int, int)> CountAndTotalPrice(ClaimsPrincipal principal)
        {
            var user = await _accountService.GetLoggedUser(principal);
            if (user == null)
            {
                return (0, 0);
            }

            var cart = _GetByUserOrCreateNew(user);
            var totalPrice = cart.Items
                .Select(item => item.Amount * item.Meal.Price)
                .Sum();
            return (cart.Items.Count, totalPrice);
        }
    }
}
