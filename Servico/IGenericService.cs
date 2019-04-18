using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Servico
{
    public interface IGenericService<T>
    {
        T GetById(Int32 id);
        IQueryable<T> GetAll();
        T Add(T item);
        void Update(T item);
        void Remove(Int32 id);
    }
}
