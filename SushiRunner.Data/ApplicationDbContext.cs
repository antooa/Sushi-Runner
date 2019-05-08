using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SushiRunner.Data.Entities;

namespace SushiRunner.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<Meal> Meals { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<MealGroup> MealGroups { get; set; }
    }
}