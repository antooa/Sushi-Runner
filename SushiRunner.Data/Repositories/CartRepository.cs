using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SushiRunner.Data.Entities;

namespace SushiRunner.Data.Repositories
{
    public class CartRepository : IRepository<Cart, long>
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
            _disposed = false;
        }

        public IEnumerable<Cart> Search(Expression<Func<Cart, bool>> predicate)
        {
            return _context.Cards.Where(predicate).ToList();
        }

        public IEnumerable<Cart> GetList()
        {
            return _context.Cards;
        }

        public Cart Get(long id)
        {
            return _context.Cards
                .AsNoTracking()
                .FirstOrDefault(entity => entity.Id == id);
        }

        public void Create(Cart entity)
        {
            _context.Cards.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Cart cartItem)
        {
            _context.Cards.Update(cartItem);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var cardItem = _context.Cards.Find(id);
            if (cardItem != null)
            {
                _context.Cards.Remove(cardItem);
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
