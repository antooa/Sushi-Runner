namespace SushiRunner.Data.Entities
{
    public class OrderItem
    {
        public long Id { get; set; }
        public Meal Meal { get; set; }
        public int Amount { get; set; }
    }
}
