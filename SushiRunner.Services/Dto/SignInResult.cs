using System.Collections.Generic;

namespace SushiRunner.Services.Dto
{
    public class SignInResult
    {
        public bool IsSuccessful { get; set; }
        public bool UserExists { get; set; }
        public IList<string> Roles { get; set; }
    }
}