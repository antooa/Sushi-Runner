using System;
using System.Linq;
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

        public CartService(IRepository<Cart, long> cartRepository, IRepository<CartItem, long> cardItemRepository,
            IRepository<Meal, long> mealRepository)
        {
            _cartRepository = cartRepository;
            _cardItemRepository = cardItemRepository;
            _mealRepository = mealRepository;
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

        public void Clear(User user)
        {
            var card = GetByUserOrCreateNew(user);
            card.Items.Clear();
            _cartRepository.Update(card);
        }
    }
}