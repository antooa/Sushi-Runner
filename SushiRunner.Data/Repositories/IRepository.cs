using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SushiRunner.Data.Repositories
{
    public interface IRepository<T, K> : IDisposable where T : class
    {
        void Create(T entity);

        T Get(K id);

        IEnumerable<T> Search(Expression<Func<T, bool>> predicate);

        IEnumerable<T> GetList();

        void Update(T entity);

        void Delete(K id);

        void Save();
    }
}