using System.Collections.Generic;

namespace SushiRunner.ViewModels
{
    public class HomePageModel
    {
        public IEnumerable<MealModel> Meals { get; set; }
        public int CartAmount;
    }
}
