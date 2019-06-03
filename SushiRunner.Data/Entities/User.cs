using Microsoft.AspNetCore.Identity;

namespace SushiRunner.Data.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
