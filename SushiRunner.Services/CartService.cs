using System.Linq;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services.Interfaces;

namespace SushiRunner.Services
{
    public class CartService : ICartService
    {
        private readonly CartRepository _cartRepository;
        private readonly CardItemRepository _cardItemRepository;

        public CartService(CartRepository cartRepository, CardItemRepository cardItemRepository)
        {
            _cartRepository = cartRepository;
            _cardItemRepository = cardItemRepository;
        }

        public Cart GetByUserOrCreateNew(User user)
        {
            var card = _cartRepository.Search(c => c.User.Equals(user)).FirstOrDefault();
            if (card == null)
            {
                card = new Cart
                {
                    User = user
                };
                _cartRepository.Create(card);
            }

            return card;
        }

        public void AddItem(User user, Meal meal, int amount)
        {
            var card = GetByUserOrCreateNew(user);
            card.Items.Add(new CartItem
            {
                Meal = meal,
                Amount = amount
            });
            _cartRepository.Update(card);
        }

        public void Clear(User user)
        {
            var card = GetByUserOrCreateNew(user);
            card.Items.Clear();
            _cartRepository.Update(card);
        }
    }
}