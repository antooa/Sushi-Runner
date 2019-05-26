namespace SushiRunner.Services.Dto
{
    public class OrderItemDTO
    {
        public long Id { get; set; }
        public MealDTO Meal { get; set; }
        public int Amount { get; set; }
    }
}