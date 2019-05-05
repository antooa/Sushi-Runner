using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SushiRunner.Data.Entities;

namespace SushiRunner.Services.Dto
{
    public class SignUpResult
    {
        public bool IsSuccessful { get; set; }
        public User User { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}