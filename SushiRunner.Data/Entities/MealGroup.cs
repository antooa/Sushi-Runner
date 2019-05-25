using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SushiRunner.Data.Entities
{
    [Table("MealGroup")]
    public class MealGroup
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Meal> Meals { get; set; }
    }
}
