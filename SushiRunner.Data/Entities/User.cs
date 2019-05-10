using Microsoft.AspNetCore.Identity;

namespace SushiRunner.Data.Entities
{
    public class User : IdentityUser
    {
        public bool IsAnonymous { get; set; }
    }
}