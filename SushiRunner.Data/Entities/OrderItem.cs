using System.ComponentModel.DataAnnotations.Schema;

namespace SushiRunner.Data.Entities
{
    [Table("OrderItem")]
    public class OrderItem
    {
        public long Id { get; set; }
        public Meal Meal { get; set; }
        public int Amount { get; set; }
    }
}
