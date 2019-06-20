using System;
namespace SushiRunner.Services.Dto
{
    public class CommentDTO
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
    }
}