using System.Collections.Generic;

namespace SushiRunner.ViewModels
{
    public class HomeModel
    {
        public IEnumerable<MealModel> Meals { get; set; }
        public IEnumerable<MealGroupModel> AvailableGroups { get; set; }
    }
}
