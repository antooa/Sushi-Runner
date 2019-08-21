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
            return _context.Orders.Include(entity => entity.Items)
                .ThenInclude(item => item.Meal)
                .ToList();
        }

        public IEnumerable<Order> Search(Expression<Func<Order, bool>> predicate)
        {
            return _context.Orders
                .Include(entity => entity.Items)
                .ThenInclude(item => item.Meal)              
                .Where(predicate).ToList();
        }

        public Order Get(long id)
        {
            return _context.Orders
                //.AsNoTracking()
                .Include(entity => entity.Items)
                .ThenInclude(item => item.Meal)
                .FirstOrDefault(entity => entity.Id == id);
        }


        public void Create(Order entity)
        {
            _context.Orders.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Order entity)
        {
            var oldOrder = Get(entity.Id);
            //oldOrder.TotalPrice = entity.TotalPrice;
            oldOrder.CustomerName = entity.CustomerName;
            oldOrder.PhoneNumber = entity.PhoneNumber;
            //oldOrder.PaymentType = entity.PaymentType;
            oldOrder.Address = entity.Address;
            oldOrder.OrderStatus = entity.OrderStatus;
            /*oldOrder.Items = entity.Items.Select(meal => new OrderItem
            {
                Amount = meal.Amount
                //Id = meal.Id
                //Meal = meal.Meal
            }).ToList();*/
            /*int sum = 0;
            foreach (var orderItem in entity.Items)
            {
                sum += orderItem.Amount * orderItem.Meal.Price;
            }

            oldOrder.TotalPrice = sum;*/
            Console.WriteLine(oldOrder.CustomerName);
            Console.WriteLine(oldOrder.PhoneNumber);
            Console.WriteLine(oldOrder.Address);
            _context.Orders.Update(oldOrder);
            _context.SaveChanges();
        }

        

        public void Delete(long id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
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
