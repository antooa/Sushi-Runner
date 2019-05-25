using System.Collections.Generic;
using SushiRunner.Services.Dto;

namespace SushiRunner.Services.Interfaces
{
    public interface IMealService : ICrudService<MealDTO, long>
    {
        IEnumerable<MealDTO> GetByGroupId(long id);
    }
}
