using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SushiRunner.Data.Entities;

namespace SushiRunner.Data.Repositories
{
    public class MealRepository : IRepository<Meal, long>
    {
        private readonly ApplicationDbContext _context;

        public MealRepository(ApplicationDbContext context)
        {
            _context = context;
            _disposed = false;
        }

        public IEnumerable<Meal> Search(Expression<Func<Meal, bool>> predicate)
        {
            return _context.Meals.Where(predicate).ToList();
        }

        public IEnumerable<Meal> GetList()
        {
            return _context.Meals;
        }

        public Meal Get(long id)
        {
            return _context.Meals
                .Include(item => item.MealGroup)
                .AsNoTracking()
                .FirstOrDefault(item => item.Id == id);
        }

        public void Create(Meal item)
        {
            _context.Meals.Add(item);
            _context.SaveChanges();
        }

        public void Update(Meal item)
        {
            var oldMeal = Get(item.Id);
            oldMeal.Name = item.Name;
            oldMeal.Description = item.Description;
            oldMeal.Price = item.Price;
            oldMeal.Weight = item.Weight;
            oldMeal.ImagePath = item.ImagePath;
            _context.Meals.Update(oldMeal);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var book = _context.Meals.Find(id);
            if (book != null)
            {
                _context.Meals.Remove(book);
            }

            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
