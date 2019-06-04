using System.Collections.Generic;
using SushiRunner.Data.Entities;
using SushiRunner.ViewModels.Home;

namespace SushiRunner.ViewModels
{
    public class MealModelWithComments
    {
        public MealModel Meal { get; set; }
        public ICollection<CommentModel> Comments { get; set; }
    }
}