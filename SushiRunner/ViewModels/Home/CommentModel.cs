using System;

namespace SushiRunner.ViewModels.Home
{
    public class CommentModel
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
    }
}