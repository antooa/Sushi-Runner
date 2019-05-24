using System.Collections.Generic;
using SushiRunner.Data.Entities;

namespace SushiRunner.Services.Interfaces
{
    public interface IMealService : ICrudService<Meal, long>
    {
        IEnumerable<Meal> GetByGroupId(long mealGroupId);
    }
}
