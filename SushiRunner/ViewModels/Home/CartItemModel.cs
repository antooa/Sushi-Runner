using System.Collections.Generic;

namespace SushiRunner.ViewModels
{
    public class CartItemModel
    {
        public MealModel Meal { get; set; }
        public int Amount { get; set; }
    }
}