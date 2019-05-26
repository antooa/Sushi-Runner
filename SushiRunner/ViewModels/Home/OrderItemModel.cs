using SushiRunner.Data.Entities;

namespace SushiRunner.ViewModels
{
    public class OrderItemModel
    {
        public MealModel Meal { get; set; }
        public int Amount { get; set; }
    }
}