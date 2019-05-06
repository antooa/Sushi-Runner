using System.Collections.Generic;
using SushiRunner.Data.Entities;

namespace SushiRunner.Services.Dto
{
    public class SignUpResult
    {
        public bool IsSuccessful { get; set; }
        public User User { get; set; }
        public List<AccountError> Errors { get; set; }
    }
}
