using System.Collections.Generic;

namespace SushiRunner.Services.Dto
{
    public class SignInResult
    {
        public bool IsSuccessful { get; set; }
        public IList<string> Roles { get; set; }
        public List<AccountError> Errors { get; set; }
    }
}
