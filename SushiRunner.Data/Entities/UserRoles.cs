using Microsoft.AspNetCore.Identity;

namespace SushiRunner.Data.Entities
{
    public class UserRoles : IdentityRole
    {
        public const string Moderator = "MODERATOR";
    }
}