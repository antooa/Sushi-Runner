using System.ComponentModel.DataAnnotations.Schema;

namespace SushiRunner.Data.Entities
{
    [Table("Comment")]
    public class Comment
    {
       public long Id { get; set; }
       public string Text { get; set; }
       public Meal Meal { get; set; }
       public long MealId { get; set; }
    }
}