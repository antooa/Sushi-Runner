using System.Collections.Generic;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.Services.Interfaces
{
    public interface IMealService : ICrudService<MealDTO, long>
    {
        IEnumerable<MealDTO> GetByGroup(MealDTO mealDto);
    }
}