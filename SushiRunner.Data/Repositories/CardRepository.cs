using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SushiRunner.Data.Entities;

namespace SushiRunner.Data.Repositories
{
    public class CardRepository : IRepository<Card, long>
    {
        private readonly ApplicationDbContext _context;

        public CardRepository(ApplicationDbContext context)
        {
            _context = context;
            _disposed = false;
        }

        public IEnumerable<Card> Search(Expression<Func<Card, bool>> predicate)
        {
            return _context.Cards.Where(predicate).ToList();
        }

        public IEnumerable<Card> GetList()
        {
            return _context.Cards;
        }

        public Card Get(long id)
        {
            return _context.Cards
                .AsNoTracking()
                .FirstOrDefault(entity => entity.Id == id);
        }

        public void Create(Card entity)
        {
            _context.Cards.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Card cardItem)
        {
            _context.Cards.Update(cardItem);
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
