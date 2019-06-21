using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SushiRunner.Data.Entities;

namespace SushiRunner.Data.Repositories
{
    public class CommentRepository : IRepository<Comment, long>
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
            _disposed = false;
        }
        public IEnumerable<Comment> Search(Expression<Func<Comment, bool>> predicate)
        {
            return _context.Comments.Where(predicate).ToList();
        }
        public IEnumerable<Comment> GetList()
        {
            return _context.Comments.ToList();
        }
        public Comment Get(long id)
        {
            return _context.Comments
                //.Include(item => item.Id)
                .AsNoTracking()
                .FirstOrDefault(item => item.Id == id);
        }
        public void Create(Comment item)
        {
            _context.Comments.Add(item);
            _context.SaveChanges();
        }
        public void Update(Comment item)
        {
            var oldComment = Get(item.Id);
            //As I understand, not required
            //oldComment.Id = item.Id;
            oldComment.Message = item.Message;
            oldComment.CreationDate = item.CreationDate;
            _context.Comments.Update(oldComment);
            _context.SaveChanges();
        }
        public void Delete(long id)
        {
            var comment = _context.Comments.Find(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
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