using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SushiRunner.Data.Entities;

namespace SushiRunner.Data.Repositories
{
    public class CartItemRepository : IRepository<CartItem, long>
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context;
            _disposed = false;
        }

        public IEnumerable<CartItem> Search(Expression<Func<CartItem, bool>> predicate)
        {
            return _context.CartItems.Where(predicate).ToList();
        }

        public IEnumerable<CartItem> GetList()
        {
            return _context.CartItems;
        }

        public CartItem Get(long id)
        {
            throw new NotImplementedException();
        }

        public void Create(CartItem entity)
        {
            _context.CartItems.Add(entity);
            _context.SaveChanges();
        }

        public void Update(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var cartItem = _context.CartItems.Find(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
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
