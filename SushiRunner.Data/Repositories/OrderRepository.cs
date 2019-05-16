using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SushiRunner.Data.Entities;
namespace SushiRunner.Data.Repositories
{
    public class OrderRepository : IRepository<Order, long>
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
            _disposed = false;
        }

        public IEnumerable<Order> GetList()
        {
            return _context.Orders;
        }

        public IEnumerable<Order> Search(Expression<Func<Order, bool>> predicate)
        {
            return _context.Orders
                .Include(entity => entity.Items).ThenInclude(item => item.Meal)              
                .Where(predicate).ToList();
        }

        public Order Get(long id)
        {
            return _context.Orders
                .AsNoTracking()
                .Include(entity => entity.Items)
                .FirstOrDefault(entity => entity.Id == id);
        }


        public void Create(Order entity)
        {
            _context.Orders.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Order entity)
        {
            _context.Orders.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var book = _context.Orders.Find(id);
            if (book != null)
            {
                _context.Orders.Remove(book);
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