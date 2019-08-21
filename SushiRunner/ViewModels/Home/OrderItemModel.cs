using SushiRunner.Data.Entities;

namespace SushiRunner.ViewModels
{
    public class OrderItemModel
    {
        public long Id { get; set; }
        public MealModel Meal { get; set; }
        public int Amount { get; set; }
    }
}