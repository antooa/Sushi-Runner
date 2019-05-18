using SushiRunner.Data.Entities;

namespace SushiRunner.Services.Dto
{
    public class MealDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public string ImagePath { get; set; }
        public int Price { get; set; }
        public long MealGroupId { get; set; }
        public MealGroup MealGroup { get; set; }
    }
}