using Dominio.Enumerado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dominio.Interface.Repositorio
{
    public interface IBaseRepositorio<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        void AlteraEstado(TEntity obj, EstadoObjetoEF estado);
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllAsNoTracking();
        void Update(TEntity obj);
        void Remove(TEntity obj);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                               Func<IQueryable<TEntity>,
                                              IOrderedQueryable<TEntity>> orderBy = null,
                                               string includeProperties = "");
        void Dispose();
    }
}
