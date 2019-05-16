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

                var group1 = new MealGroup
                {
                    Name = "Group1",
                    Description = "Description1"
                };

                var group2 = new MealGroup
                {
                    Name = "Group2",
                    Description = "Description2"
                };

                context.MealGroups.AddRange(group1, group2);

                context.Meals.AddRange(
                    new Meal
                    {
                        Name = "Kayak",
                        Description = "A boat for one person",
                        Price = 275,
                        ImagePath = "/img/sushi.jpg",
                        Weight = 121.34,
                        MealGroup = group1
                    },
                    new Meal
                    {
                        Name = "Lifejacket",
                        Description = "Protective and fashionable",
                        Price = 48,
                        ImagePath = "/img/sushi.jpg",
                        Weight = 121.34,
                        MealGroup = group1
                    },
                    new Meal
                    {
                        Name = "Soccer Ball",
                        Description = "FIFA-approved size and weight",
                        Price = 19,
                        ImagePath = "/img/sushi.jpg",
                        Weight = 300.00,
                        MealGroup = group1
                    },
                    new Meal
                    {
                        Name = "Corner Flags",
                        Description = "Give your playing field a professional touch",
                        Price = 34,
                        ImagePath = "/img/sushi.jpg",
                        Weight = 300.00,
                        MealGroup = group1
                    },
                    new Meal
                    {
                        Name = "Stadium",
                        Description = "Flat-packed 35,000-seat stadium",
                        Price = 7950,
                        ImagePath = "/img/sushi.jpg",
                        Weight = 300.00,
                        MealGroup = group1
                    },
                    new Meal
                    {
                        Name = "Thinking Cap",
                        Description = "Improve brain efficiency by 75%",
                        Price = 16,
                        ImagePath = "/img/sushi.jpg",
                        Weight = 232.23,
                        MealGroup = group2
                    },
                    new Meal
                    {
                        Name = "Unsteady Chair",
                        Description = "Secretly give your opponent a disadvantage",
                        Price = 29,
                        ImagePath = "/img/sushi.jpg",
                        Weight = 121.34,
                        MealGroup = group2
                    },
                    new Meal
                    {
                        Name = "Human Chess Board",
                        Description = "A fun game for the family",
                        Price = 75,
                        ImagePath = "/img/sushi.jpg",
                        Weight = 121.34,
                        MealGroup = group2
                    },
                    new Meal
                    {
                        Name = "Bling-Bling King",
                        Description = "Gold-plated, diamond-studded King",
                        Price = 120,
                        ImagePath = "/img/sushi.jpg",
                        Weight = 121.34,
                        MealGroup = group2
                    }
                );
                context.SaveChanges();
            }
        }
    }
}