namespace SushiRunner.Services.Dto
{
    public class CartItemDTO
    {
        public MealDTO Meal { get; set; }
        public long MealId { get; set; }
        public int Amount { get; set; }
    }
}