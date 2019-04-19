using System.ComponentModel.DataAnnotations.Schema;

namespace SushiRunner.Data.Entities
{
    [Table("Meal")]
    public class Meal
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public string ImagePath { get; set; }
        public int Price { get; set; }
        public MealGroup MealGroup { get; set; }
    }
}