using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.Services.Interfaces
{
    public interface IMealService : ICrudService<MealDTO, long>
    {

        IEnumerable<MealDTO> GetByGroupId(long id);

        void Create(MealDTO mealDto, IFormFile file);

        void Update(MealDTO mealDto, IFormFile file);

        IEnumerable<MealDTO> GetMealsWithCartCheckbox(User user);

        IEnumerable<MealDTO> GetMealsWithCartCheckbox(User user, long mealGroupId);

        void AddComment(long mealId, string message);
    }
}