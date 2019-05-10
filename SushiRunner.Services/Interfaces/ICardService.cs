using SushiRunner.Data.Entities;

namespace SushiRunner.Services.Interfaces
{
    public interface ICardService
    {
        Card GetByUserOrCreateNew(User user);

        void AddItem(User user, Meal meal, int amount);

        void Clear(User user);
    }
}
