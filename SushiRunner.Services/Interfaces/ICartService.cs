using SushiRunner.Data.Entities;

namespace SushiRunner.Services.Interfaces
{
    public interface ICartService
    {
        Cart GetByUserOrCreateNew(User user);

        void AddItem(User user, long mealId);
        
        void RemoveItem(User user, long mealId);

        void Clear(User user);
    }
}
