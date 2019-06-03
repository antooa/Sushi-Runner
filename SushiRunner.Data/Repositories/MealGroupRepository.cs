using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SushiRunner.Data.Entities;

namespace SushiRunner.Data.Repositories
{
    public class MealGroupRepository : IRepository<MealGroup, long>
    {
        private readonly ApplicationDbContext _context;

        public MealGroupRepository(ApplicationDbContext context)
        {
            _context = context;
            _disposed = false;
        }

        public IEnumerable<MealGroup> GetList()
        {
            return _context.MealGroups.Include(group => group.Meals);
        }

        public IEnumerable<MealGroup> Search(Expression<Func<MealGroup, bool>> predicate)
        {
            return _context.MealGroups.Include(group=>group.Meals).Where(predicate).ToList();
        }

        public MealGroup Get(long id)
        {
            return _context.MealGroups
                .Include(group => group.Meals)
                .AsNoTracking()
                .FirstOrDefault(entity => entity.Id == id);
        }

        public void Create(MealGroup entity)
        {
            _context.MealGroups.Add(entity);
            _context.SaveChanges();
        }

        public void Update(MealGroup mealGroup)
        {
            _context.MealGroups.Update(mealGroup);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var mealGroup = _context.MealGroups.Find(id);
            if (mealGroup != null)
            {
                _context.MealGroups.Remove(mealGroup);
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
