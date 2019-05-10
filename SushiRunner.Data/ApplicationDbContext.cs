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

        public DbSet<Meal> Meals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<MealGroup> MealGroups { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Meal>().HasKey(meal => meal.Id);
            modelBuilder.Entity<Meal>().Property(meal => meal.Name).IsRequired();
            modelBuilder.Entity<Meal>().Property(meal => meal.Description).IsRequired();
            modelBuilder.Entity<Meal>().Property(meal => meal.Weight).IsRequired();
            modelBuilder.Entity<Meal>().Property(meal => meal.ImagePath).IsRequired();
            modelBuilder.Entity<Meal>().Property(meal => meal.Price).IsRequired();
            modelBuilder.Entity<Meal>()
                .HasOne(meal => meal.MealGroup)
                .WithMany(group => group.Meals)
                .HasForeignKey(meal => meal.MealGroupId)
                .IsRequired();

            modelBuilder.Entity<MealGroup>().HasKey(group => group.Id);
            modelBuilder.Entity<MealGroup>().Property(group => group.Name).IsRequired();
            modelBuilder.Entity<MealGroup>().Property(group => group.Description).IsRequired();

            modelBuilder.Entity<CartItem>().HasKey(item => new {item.MealId, item.CartId});
            modelBuilder.Entity<CartItem>().Property(item => item.Amount).IsRequired();
            modelBuilder.Entity<CartItem>().HasOne(item => item.Meal);
            modelBuilder.Entity<CartItem>()
                .HasOne(item => item.Cart)
                .WithMany(cart => cart.Items)
                .HasForeignKey(item => item.CartId)
                .IsRequired();

            modelBuilder.Entity<Cart>().HasKey(item => item.Id);
            modelBuilder.Entity<Cart>()
                .HasMany(cart => cart.Items)
                .WithOne(item => item.Cart)
                .IsRequired();
        }
    }
}
