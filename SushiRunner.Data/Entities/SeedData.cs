using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace SushiRunner.Data.Entities
{
    public static class SeedData
    {
        private const string DefaultUsername = "ojles";
        private const string DefaultPassword = "Pa$$w0rd";

        public static void EnsurePopulated(IApplicationBuilder app)
        {
            CreateDefaultUser(app);
            PopulateProducts(app);
        }

        private static async void CreateDefaultUser(IApplicationBuilder app)
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

        private static void PopulateProducts(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (context.Meals.Any())
                {
                    return;
                }

                var groups = new[]
                {
                    new MealGroup {Name = "Піца"},
                    new MealGroup {Name = "Роли"},
                    new MealGroup {Name = "Гарячі роли"},
                    new MealGroup {Name = "Суші"},
                    new MealGroup {Name = "Сети"},
                    new MealGroup {Name = "Десерти"}
                };

                context.MealGroups.AddRange(groups);

                
                


                context.SaveChanges();
            }
        }
    }
}
