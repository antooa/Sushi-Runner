using System.Collections.Generic;

namespace SushiRunner.ViewModels
{
    public class HeaderModel
    {
        public int CartItemsAmount { get; set; }
        public int CartItemsPrice { get; set; }
        public IEnumerable<MealGroupModel> AvailableGroups { get; set; }
    }
}