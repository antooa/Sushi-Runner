using Microsoft.AspNetCore.Identity;

namespace SushiRunner.Data.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string DefaultAddress { get; set; }
        public bool IsAnonymous { get; set; }
    }
}
