using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Servico
{
    public class CtrlFuncionalidadeServico : BaseService, IDisposable, IGenericService<Entidade.CtrlFuncionalidade>
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    unitOfWork.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Entidade.CtrlFuncionalidade GetById(int id)
        {
            Entidade.CtrlFuncionalidade _entidade = new Entidade.CtrlFuncionalidade();

            try
            {
                this.Sucesso = true;

                DAL.Models.CtrlFuncionalidade _entity = unitOfWork.CtrlFuncionalidadeRepository.Get(f => f.IdFuncionalidade == id, null, "CtrlPerfil").FirstOrDefault();
                _entidade = AutoMapper.Mapper.Map<Entidade.CtrlFuncionalidade>(_entity);

                return _entidade;
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.Sucesso = false;
            }

            return _entidade;
        }

        public IQueryable<Entidade.CtrlFuncionalidade> GetAll()
        {
            List<Entidade.CtrlFuncionalidade> _entidade = new List<Entidade.CtrlFuncionalidade>();

            try
            {
                this.Sucesso = true;

                List<DAL.Models.CtrlFuncionalidade> _entity = unitOfWork.CtrlFuncionalidadeRepository.GetAll().ToList();
                _entidade = AutoMapper.Mapper.Map<List<Entidade.CtrlFuncionalidade>>(_entity);

                return _entidade.AsQueryable();
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.Sucesso = false;
            }
            return _entidade.AsQueryable();
        }

        public Entidade.CtrlFuncionalidade Add(Entidade.CtrlFuncionalidade item)
        {
            Entidade.CtrlFuncionalidade retorno = new Entidade.CtrlFuncionalidade();
            retorno = item;

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;

                    DAL.Models.CtrlFuncionalidade _entity = AutoMapper.Mapper.Map<DAL.Models.CtrlFuncionalidade>(item);

                    unitOfWork.CtrlFuncionalidadeRepository.Insert(_entity);
                    unitOfWork.Save();

                    retorno.IdFuncionalidade = _entity.IdFuncionalidade;
                    _transaction.Complete();
                    return retorno;
                }
                catch (Exception ex)
                {
                    this.Error = ex;
                    this.Sucesso = false;
                }
            }
            return retorno;
        }

        public void Update(Entidade.CtrlFuncionalidade item)
        {
            Entidade.CtrlFuncionalidade retorno = new Entidade.CtrlFuncionalidade();
            retorno = item;

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;

                    DAL.Models.CtrlFuncionalidade _entity = AutoMapper.Mapper.Map<DAL.Models.CtrlFuncionalidade>(item);

                    unitOfWork.CtrlFuncionalidadeRepository.Update(_entity);
                    unitOfWork.Save();
                    _transaction.Complete();
                }
                catch (Exception ex)
                {
                    this.Error = ex;
                    this.Sucesso = false;
                }
            }
        }

        public void Remove(int id)
        {
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;

                    unitOfWork.CtrlFuncionalidadeRepository.Delete(id);
                    unitOfWork.Save();
                    _transaction.Complete();
                }
                catch (Exception ex)
                {
                    this.Error = ex;
                    this.Sucesso = false;
                }
            }
        }
    }
}
