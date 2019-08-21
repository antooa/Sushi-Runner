using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SushiRunner.Data.Entities;

namespace SushiRunner.Data.Repositories
{
    public class OrderItemRepository : IRepository<OrderItem, long>
    {
        private readonly ApplicationDbContext _context;
        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
            _disposed = false;
        }
        public IEnumerable<OrderItem> Search(Expression<Func<OrderItem, bool>> predicate)
        {
            return _context.OrderItems.Where(predicate).ToList();
        }

        public IEnumerable<OrderItem> GetList()
        {
            return _context.OrderItems.ToList();
        }
        public OrderItem Get(long id)
        {
            return _context.OrderItems
                //.Include(x => x.Meal)
                //.AsNoTracking()
                .FirstOrDefault(item => item.Id == id);
        }
        public void Create(OrderItem item)
        {
            _context.OrderItems.Add(item);
            _context.SaveChanges();
        }
        public void Update(OrderItem item)
        {
            var oldOrderItem = Get(item.Id);
            oldOrderItem.Amount = item.Amount;
            //oldOrderItem.Meal = item.Meal;
            _context.OrderItems.Update(oldOrderItem);
            _context.SaveChanges();
        }
        public void Delete(long id)
        {
            var orderItem = _context.OrderItems.Find(id);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
            }
            else
            {
                Console.WriteLine("LOL NULL LOL NULL LOL NULL LOL NULL LOL NULL LOL NULL LOL NULL ");
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