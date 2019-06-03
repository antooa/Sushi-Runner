using System.Collections.Generic;

namespace SushiRunner.ViewModels
{
    public class MealModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public string ImagePath { get; set; }
        public int Price { get; set; }
        public bool IsInCart { get; set; }

        public long GroupId { get; set; }
    }
}