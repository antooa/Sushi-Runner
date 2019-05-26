using System.Security.Claims;
using System.Threading.Tasks;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.Services.Interfaces
{
    public interface ICartService
    {
        CartDTO GetByUserOrCreateNew(User user);

        void AddItem(User user, long mealId);
        
        void RemoveItem(User user, long mealId);

        void ChangeItemAmount(User user, long mealId, int newAmount);

        void Clear(User user);
        
        Task<(int, int)> CountAndTotalPrice(ClaimsPrincipal principal);
    }
}
