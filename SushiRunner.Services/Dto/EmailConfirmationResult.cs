using System.Collections.Generic;

namespace SushiRunner.Services.Dto
{
    public class EmailConfirmationResult
    {
        public bool IsSuccessful { get; set; }
        public List<AccountError> Errors { get; set; }
    }
}
