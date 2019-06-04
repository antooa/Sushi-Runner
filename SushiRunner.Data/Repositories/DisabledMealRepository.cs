using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SushiRunner.Data.Entities;

namespace SushiRunner.Data.Repositories
{
    public class DisabledMealRepository:IRepository<DisabledMeal,long>
    {
        private readonly ApplicationDbContext _context;

        public DisabledMealRepository(ApplicationDbContext context)
        {
            _context = context;
            _disposed = false;
        }

        public IEnumerable<DisabledMeal> GetList()
        {
            return _context.DisabledMeals;
        }

        public IEnumerable<DisabledMeal> Search(Expression<Func<DisabledMeal, bool>> predicate)
        {
            return _context.DisabledMeals.Where(predicate).ToList();
        }

        public DisabledMeal Get(long id)
        {
            return _context.DisabledMeals
                .AsNoTracking()
                .FirstOrDefault(entity => entity.Id == id);
        }

        public void Create(DisabledMeal entity)
        {
            _context.DisabledMeals.Add(entity);
            _context.SaveChanges();
        }

        public void Update(DisabledMeal disabledMeal)
        {
            _context.DisabledMeals.Update(disabledMeal);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var disableMeal = _context.DisabledMeals.Find(id);
            if (disableMeal != null)
            {
                _context.DisabledMeals.Remove(disableMeal);
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