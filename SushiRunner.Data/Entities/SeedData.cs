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
                    new MealGroup {Name = "Піца", ImagePath = "/img/pizza.svg"},
                    new MealGroup {Name = "Роли", ImagePath = "/img/rolls.svg"},
                    new MealGroup {Name = "Гарячі роли", ImagePath = "/img/hot.svg"},
                    new MealGroup {Name = "Суші", ImagePath = "/img/sushi.svg"},
                    new MealGroup {Name = "Сети", ImagePath = "/img/set.svg"},
                    new MealGroup {Name = "Десерти", ImagePath = "/img/cupcake.svg"}
                };

                context.MealGroups.AddRange(groups);

                context.Meals.AddRange(
                    new Meal
                    {
                        ImagePath = "/img/20190408_104639_апетитна.jpg",
                        Name = "Піца Апетитна",
                        Description =
                            "Неополітанський соус, Сир моцарелла, Базилік, Перець болгарський, Бекон, Салямі гостре        ",
                        Weight = 370,
                        Price = 155,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190523_091736_капричоза.jpg",
                        Name = "Акційна Піца Капричоза",
                        Description = "Неополітанський соус, Сир моцарелла, Шинка, Печериці свіжі, Маслини        ",
                        Weight = 390,
                        Price = 72,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190408_104601_ББК.jpg",
                        Name = "Піца BBQ",
                        Description =
                            "Неополітанський соус, Сир моцарелла, Салямі Чорізо, Синя цибуля, Огірки мариновані, Соус Барбекю        ",
                        Weight = 450,
                        Price = 115,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190408_105147_інферно.jpg",
                        Name = "Піца Інферно",
                        Description =
                            "Неополітанський соус, Сир моцарелла, Бекон, Перець чілі, Мисливські ковбаски, Салямі Чорізо        ",
                        Weight = 410,
                        Price = 125,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190312_125051_5.jpg",
                        Name = "Піца Бургер",
                        Description =
                            "Неополітанський соус, Сир моцарелла, Перець болгарський, Бекон, Синя цибуля, Курка смажена, Соус Бургер, Айсберг        ",
                        Weight = 450,
                        Price = 140,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190408_104737_Галицька.jpg",
                        Name = "Піца Галицька",
                        Description =
                            "Неополітанський соус, Сир моцарелла, Печериці свіжі, Синя цибуля, Фарш яловичий, Часникова олія        ",
                        Weight = 400,
                        Price = 105,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190408_104806_Грибна.jpg",
                        Name = "Піца Грибна",
                        Description =
                            "Сир моцарелла, Печериці свіжі, Вершковий соус, Сир пармезан, Опеньки консервовані, Шиітакі, Унагі        ",
                        Weight = 410,
                        Price = 145,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190408_104915_З білими грибами.jpg",
                        Name = "Піца з Білими грибами",
                        Description = "Сир моцарелла, Печериці свіжі, Вершковий соус, Сир пармезан, Білі гриби        ",
                        Weight = 410,
                        Price = 175,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190408_105107_Піца з тунцем і руколою.jpg",
                        Name = "Піца з Тунцем та Руколою",
                        Description =
                            "Неополітанський соус, Сир моцарелла, Помідори чері, Рукола, Синя цибуля, Тунець консервований        ",
                        Weight = 430,
                        Price = 190,
                        MealGroup = groups[0]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190408_113533_пеппер.jpg",
                        Name = "Pepper рол",
                        Description =
                            "Рис, Норі, Кунжут білий, Кунжут чорний, Сир вершковий, Перець болгарський темпура",
                        Weight = 200,
                        Price = 75,
                        MealGroup = groups[1]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190408_113639_ізумі тай темпура.jpg",
                        Name = "Ізумі Тай Темпура",
                        Description = "Рис, Спайс майонез, Кунжут чорний, Окунь темпура, Перець болгарський темпура",
                        Weight = 205,
                        Price = 115,
                        MealGroup = groups[1]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190408_113723_філад. з ікрою тобіко.jpg",
                        Name = "Філадельфія De Luxe",
                        Description = "Рис, Норі, Крем-сир, Огірок, Авокадо, Ікра тобіко червона, Лосось свіжий",
                        Weight = 240,
                        Price = 175,
                        MealGroup = groups[1]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190523_092047_масаго з лосоем.jpg",
                        Name = "Акційний рол Масаго з лососем",
                        Description = "Рис, Норі, Крем-сир, Огірок, Ікра масаго, Лосось свіжий",
                        Weight = 210,
                        Price = 72,
                        MealGroup = groups[1]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190517_105056_рінго.jpg",
                        Name = "Рінго (половинка)",
                        Description =
                            "Рис, Норі, соус Унагі, Кунжут білий, Сир фета, Лосось копчений, Сир тостовий, Яблуко",
                        Weight = 118,
                        Price = 75,
                        MealGroup = groups[1]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190408_113809_кранч з копч. лососем.jpg",
                        Name = "Рол Crunch з копченим лососем",
                        Description = "Рис, Норі, Крем-сир, Огірок, Лосось копчений, Смажена цибуля",
                        Weight = 220,
                        Price = 155,
                        MealGroup = groups[1]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190408_114007_кранч з масляною р.jpg",
                        Name = "Рол Crunch з масляною рибою",
                        Description = "Рис, Норі, Огірок, Майонез, Смажена цибуля, Масляна риба",
                        Weight = 200,
                        Price = 115,
                        MealGroup = groups[1]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190408_114134_весняний.jpg",
                        Name = "Рол Весняний",
                        Description = "Рис, Норі, Крем-сир, Огірок, Масляна риба, Кріп",
                        Weight = 225,
                        Price = 150,
                        MealGroup = groups[1]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190409_164218_кінкі.jpg",
                        Name = "Теплий рол Кінкі",
                        Description =
                            "Омлет тамаго, Кляр темпура, Огірок, Горіховий соус, Креветка тигрова, Рис, Норі        ",
                        Weight = 220,
                        Price = 135,
                        MealGroup = groups[2]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190409_164426_окінава.jpg",
                        Name = "Теплий рол Окінава",
                        Description =
                            "Кляр темпура, Гриб шиітаке, Огірок, Крем сир, Морський окунь смажений, Рис, Норі        ",
                        Weight = 230,
                        Price = 155,
                        MealGroup = groups[2]
                    },
                    new Meal
                    {
                        ImagePath = "/img/20190409_164321_окура.jpg",
                        Name = "Теплий рол Окура",
                        Description =
                            "Сир плавлений, Кляр темпура, Салат айсберг, Соус цезар, Спайсі майонез, Курка смажена, Рис, Норі        ",
                        Weight = 240,
                        Price = 110,
                        MealGroup = groups[2]
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
