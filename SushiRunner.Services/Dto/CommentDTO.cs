using System;
namespace SushiRunner.Services.Dto
{
    public class CommentDTO
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public int Rating { get; set; }
        public MealDTO Meal { get; set; }
    }
}