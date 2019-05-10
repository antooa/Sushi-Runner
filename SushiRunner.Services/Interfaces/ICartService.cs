using SushiRunner.Data.Entities;

namespace SushiRunner.Services.Interfaces
{
    public interface ICartService
    {
        Cart GetByUserOrCreateNew(User user);

        void AddItem(User user, Meal meal, int amount);

        void Clear(User user);
    }
}
