using Microsoft.AspNetCore.Http;
using SushiRunner.Data.Entities;
using SushiRunner.Services.Dto;

namespace SushiRunner.Services.Interfaces
{
    public interface IMealGroupService : ICrudService<MealGroupDTO, long>
    {
        void Create(MealGroupDTO groupDto, IFormFile file);

        void Update(MealGroupDTO groupDto, IFormFile file);
    }
}