using Microsoft.AspNetCore.Identity;

namespace SushiRunner.Models
{
    public class UserRoles : IdentityRole
    {
        public static readonly string Moderator = "MODERATOR";
    }
}