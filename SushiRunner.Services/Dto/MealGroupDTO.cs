using System.Collections.Generic;
using SushiRunner.Data.Entities;

namespace SushiRunner.Services.Dto
{
    public class MealGroupDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Meal> Meals { get; set; }
    }
}
