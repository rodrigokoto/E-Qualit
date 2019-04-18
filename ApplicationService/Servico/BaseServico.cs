using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace ApplicationService.Servico
{
    public class BaseServico<TEntity> : IDisposable, IBaseServico<TEntity> where TEntity : class
    {
        private readonly IBaseRepositorio<TEntity> _repositorio;

        public BaseServico(IBaseRepositorio<TEntity> repositorio)
        {
            _repositorio = repositorio;
        }
        
        public void Add(TEntity obj)
        {
            _repositorio.Add(obj);
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                       Func<IQueryable<TEntity>,
                                       IOrderedQueryable<TEntity>> orderBy = null,
                                       string includeProperties = "")
        {
            return _repositorio.Get(filter, orderBy, includeProperties);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _repositorio.GetAll();
        }

        public IEnumerable<TEntity> GetAllAsNoTracking()
        {
            return _repositorio.GetAllAsNoTracking();
        }

        public TEntity GetById(int id)
        {
            return _repositorio.GetById(id);
        }
        
        public void Update(TEntity obj)
        {
            _repositorio.Update(obj);
        }

        public void Remove(TEntity obj)
        {
            _repositorio.Remove(obj);
        }

        public void Dispose()
        {
            _repositorio.Dispose();
        }
    }
}
