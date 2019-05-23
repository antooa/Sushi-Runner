using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services.Interfaces;

namespace SushiRunner.Services
{
    public class CartService : ICartService
    {
        private readonly IRepository<Cart, long> _cartRepository;
        private readonly IRepository<CartItem, long> _cardItemRepository;
        private readonly IRepository<Meal, long> _mealRepository;
        private readonly IAccountService _accountService;

        public CartService(IRepository<Cart, long> cartRepository, IRepository<CartItem, long> cardItemRepository,
            IRepository<Meal, long> mealRepository, IAccountService accountService)
        {
            _cartRepository = cartRepository;
            _cardItemRepository = cardItemRepository;
            _mealRepository = mealRepository;
            _accountService = accountService;
        }

        public Cart GetByUserOrCreateNew(User user)
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

        public void AddItem(User user, long mealId)
        {
            var meal = _mealRepository.Get(mealId);
            if (meal == null)
            {
                throw new Exception($"Meal with Id={mealId} not found");
            }

            var cart = GetByUserOrCreateNew(user);
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
            var cart = GetByUserOrCreateNew(user);
            cart.Items.RemoveAll(item => item.MealId == mealId);
            _cartRepository.Update(cart);
        }

        public void Clear(User user)
        {
            var card = GetByUserOrCreateNew(user);
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

            var cart = GetByUserOrCreateNew(user);
            var totalPrice = cart.Items
                .Select(item => item.Amount * item.Meal.Price)
                .Sum();
            return (cart.Items.Count, totalPrice);
        }
    }
}