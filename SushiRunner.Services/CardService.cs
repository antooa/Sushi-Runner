using System.Linq;
using SushiRunner.Data.Entities;
using SushiRunner.Data.Repositories;
using SushiRunner.Services.Interfaces;

namespace SushiRunner.Services
{
    public class CardService : ICardService
    {
        private readonly CardRepository _cardRepository;
        private readonly CardItemRepository _cardItemRepository;

        public CardService(CardRepository cardRepository, CardItemRepository cardItemRepository)
        {
            _cardRepository = cardRepository;
            _cardItemRepository = cardItemRepository;
        }

        public Card GetByUserOrCreateNew(User user)
        {
            var card = _cardRepository.Search(c => c.User.Equals(user)).FirstOrDefault();
            if (card == null)
            {
                card = new Card
                {
                    User = user
                };
                _cardRepository.Create(card);
            }

            return card;
        }

        public void AddItem(User user, Meal meal, int amount)
        {
            var card = GetByUserOrCreateNew(user);
            card.Items.Add(new CardItem
            {
                Meal = meal,
                Amount = amount
            });
            _cardRepository.Update(card);
        }

        public void Clear(User user)
        {
            var card = GetByUserOrCreateNew(user);
            card.Items.Clear();
            _cardRepository.Update(card);
        }
    }
}