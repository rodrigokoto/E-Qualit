using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApplicationService.Interface
{
    public interface IBaseServico<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                           Func<IQueryable<TEntity>,
                                           IOrderedQueryable<TEntity>> orderBy = null,
                                           string includeProperties = "");
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllAsNoTracking();
        void Update(TEntity obj);
        void Remove(TEntity obj);
        void Dispose();
    }
}
