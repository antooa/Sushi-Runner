using System;
using System.Collections.Generic;

namespace SushiRunner.Services.Interfaces
{
    public interface ICrudService<T, K> : IDisposable where T : class
    {
        void Create(T entity);

        IEnumerable<T> GetList();

        T Get(K id);

        void Update(T entity);

        void Delete(K id);
    }
}