using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SushiRunner.Models
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
                var user = await userManager.FindByIdAsync(DefaultUsername);
                if (user == null)
                {
                    user = new User();
                    user.UserName = DefaultUsername;
                    await userManager.CreateAsync(user, DefaultPassword);
                }
            }
        }
    }
}
