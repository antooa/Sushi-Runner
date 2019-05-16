using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SushiRunner.Data.Entities
{
    public static class SeedData
    {
        private const string DefaultUsername = "ojles";
        private const string DefaultPassword = "Pa$$w0rd";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = (UserManager<User>) scope.ServiceProvider.GetService(typeof(UserManager<User>));
                var roleManager =
                    (RoleManager<IdentityRole>) scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));

                var role = await roleManager.FindByNameAsync(UserRoles.Moderator);
                if (role == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Moderator));
                }

                var user = await userManager.FindByIdAsync(DefaultUsername);
                if (user == null)
                {
                    user = new User {UserName = DefaultUsername};
                    await userManager.CreateAsync(user, DefaultPassword);
                    await userManager.AddToRoleAsync(user, UserRoles.Moderator);
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var result = await userManager.ConfirmEmailAsync(user, code);
                }
            }
        }
    }
}