using SushiRunner.Data.Entities;

namespace SushiRunner.Models.ViewModels
{
    public class OrderItemModel
    {
        public Meal Meal { get; set; }
        public int Amount { get; set; }
    }
}