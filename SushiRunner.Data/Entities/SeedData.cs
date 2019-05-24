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

                context.Meals.AddRange(
                    new Meal
                    {
                        ImagePath = "20190408_104639_апетитна.jpg",
                        Name = "Піца Апетитна",
                        Description =
                            "Неополітанський соус, Сир моцарелла, Базилік, Перець болгарський, Бекон, Салямі гостре        ",
                        Weight = 370,
                        Price = 155,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "20190523_091736_капричоза.jpg",
                        Name = "Акційна Піца Капричоза",
                        Description = "Неополітанський соус, Сир моцарелла, Шинка, Печериці свіжі, Маслини        ",
                        Weight = 390,
                        Price = 72,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "20190408_104601_ББК.jpg",
                        Name = "Піца BBQ",
                        Description =
                            "Неополітанський соус, Сир моцарелла, Салямі Чорізо, Синя цибуля, Огірки мариновані, Соус Барбекю        ",
                        Weight = 450,
                        Price = 115,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "20190408_105147_інферно.jpg",
                        Name = "Піца Інферно",
                        Description =
                            "Неополітанський соус, Сир моцарелла, Бекон, Перець чілі, Мисливські ковбаски, Салямі Чорізо        ",
                        Weight = 410,
                        Price = 125,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "20190312_125051_5.jpg",
                        Name = "Піца Бургер",
                        Description =
                            "Неополітанський соус, Сир моцарелла, Перець болгарський, Бекон, Синя цибуля, Курка смажена, Соус Бургер, Айсберг        ",
                        Weight = 450,
                        Price = 140,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "20190408_104737_Галицька.jpg",
                        Name = "Піца Галицька",
                        Description =
                            "Неополітанський соус, Сир моцарелла, Печериці свіжі, Синя цибуля, Фарш яловичий, Часникова олія        ",
                        Weight = 400,
                        Price = 105,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "20190408_104806_Грибна.jpg",
                        Name = "Піца Грибна",
                        Description =
                            "Сир моцарелла, Печериці свіжі, Вершковий соус, Сир пармезан, Опеньки консервовані, Шиітакі, Унагі        ",
                        Weight = 410,
                        Price = 145,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "20190408_104915_З білими грибами.jpg",
                        Name = "Піца з Білими грибами",
                        Description = "Сир моцарелла, Печериці свіжі, Вершковий соус, Сир пармезан, Білі гриби        ",
                        Weight = 410,
                        Price = 175,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "20190408_105107_Піца з тунцем і руколою.jpg",
                        Name = "Піца з Тунцем та Руколою",
                        Description =
                            "Неополітанський соус, Сир моцарелла, Помідори чері, Рукола, Синя цибуля, Тунець консервований        ",
                        Weight = 430,
                        Price = 190,
                        MealGroup = groups[0]
                    }
                );


                context.SaveChanges();
            }
        }
    }
}
