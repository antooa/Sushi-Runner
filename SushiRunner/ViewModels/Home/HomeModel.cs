using System.Collections.Generic;

namespace SushiRunner.ViewModels.Home
{
    public class HomeModel
    {
        public string Title { get; set; }
        public IEnumerable<MealModel> Meals { get; set; }
    }
}
