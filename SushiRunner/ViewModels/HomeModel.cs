using System.Collections.Generic;

namespace SushiRunner.ViewModels
{
    public class HomeModel
    {
        public IEnumerable<MealModel> Meals { get; set; }
        public int CartAmount { get; set; }
    }
}