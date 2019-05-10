namespace SushiRunner.Data.Entities
{
    public class CartItem
    {
        public Meal Meal { get; set; }
        public long MealId { get; set; }
        public int Amount { get; set; }
        public long CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
