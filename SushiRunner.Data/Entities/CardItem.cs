namespace SushiRunner.Data.Entities
{
    public class CardItem
    {
        public long Id { get; set; }
        public Meal Meal { get; set; }
        public int Amount { get; set; }
    }
}
