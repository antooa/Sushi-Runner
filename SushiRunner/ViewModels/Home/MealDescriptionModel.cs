using System.Collections.Generic;

namespace SushiRunner.ViewModels.Home
{
    public class MealDescriptionModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public string ImagePath { get; set; }
        public int Price { get; set; }

        
        
        //all above is from MealModel
        public IEnumerable<CommentModel> Comments { get; set; }
    }
}