using System;
using System.Collections.Generic;
using System.Linq;
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
            return _context.MealGroups;
        }

        public MealGroup Get(long id)
        {
            return _context.MealGroups
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