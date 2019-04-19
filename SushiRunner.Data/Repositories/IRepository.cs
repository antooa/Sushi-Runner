using System;
using System.Collections.Generic;

namespace SushiRunner.Data.Repositories
{
    public interface IRepository<T, K> : IDisposable where T : class
    {
        void Create(T entity);

        T Get(K id);

        IEnumerable<T> GetList();

        void Update(T entity);

        void Delete(K id);

        void Save();
    }
}