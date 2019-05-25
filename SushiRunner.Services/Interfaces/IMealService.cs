using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SushiRunner.Services.Dto;

namespace SushiRunner.Services.Interfaces
{
    public interface IMealService : ICrudService<MealDTO, long>
    {

        IEnumerable<MealDTO> GetByGroup(MealDTO mealDto);
        void Create(MealDTO mealDto, IFormFile file);
    }
}
