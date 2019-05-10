using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SushiRunner.Data.Entities;

namespace SushiRunner.Data.Repositories
{
    public class CardItemRepository : IRepository<CardItem, long>
    {
        private readonly ApplicationDbContext _context;

        public CardItemRepository(ApplicationDbContext context)
        {
            _context = context;
            _disposed = false;
        }

        public IEnumerable<CardItem> Search(Expression<Func<CardItem, bool>> predicate)
        {
            return _context.CardItems.Where(predicate).ToList();
        }

        public IEnumerable<CardItem> GetList()
        {
            return _context.CardItems;
        }

        public CardItem Get(long id)
        {
            return _context.CardItems
                .AsNoTracking()
                .FirstOrDefault(entity => entity.Id == id);
        }

        public void Create(CardItem entity)
        {
            _context.CardItems.Add(entity);
            _context.SaveChanges();
        }

        public void Update(CardItem cardItem)
        {
            _context.CardItems.Update(cardItem);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var cardItem = _context.CardItems.Find(id);
            if (cardItem != null)
            {
                _context.CardItems.Remove(cardItem);
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