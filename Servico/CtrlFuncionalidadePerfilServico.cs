using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Servico
{
    public class CtrlFuncionalidadePerfilServico : BaseService, IDisposable, IGenericService<Entidade.CtrlFuncionalidadePerfil>
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

        public Entidade.CtrlFuncionalidadePerfil GetById(int id)
        {
            Entidade.CtrlFuncionalidadePerfil _entidade = new Entidade.CtrlFuncionalidadePerfil();

            try
            {
                this.Sucesso = true;

                DAL.Models.CtrlFuncionalidadePerfil _entity = unitOfWork.CtrlFuncionalidadePerfilRepository.Get(f => f.IdFuncionalidadePerfil == id, null, "CtrlPerfil").FirstOrDefault();
                _entidade = AutoMapper.Mapper.Map<Entidade.CtrlFuncionalidadePerfil>(_entity);

                return _entidade;
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.Sucesso = false;
            }

            return _entidade;
        }

        public IQueryable<Entidade.CtrlFuncionalidadePerfil> GetAll()
        {
            List<Entidade.CtrlFuncionalidadePerfil> _entidade = new List<Entidade.CtrlFuncionalidadePerfil>();

            try
            {
                this.Sucesso = true;

                List<DAL.Models.CtrlFuncionalidadePerfil> _entity = unitOfWork.CtrlFuncionalidadePerfilRepository.GetAll().ToList();
                _entidade = AutoMapper.Mapper.Map<List<Entidade.CtrlFuncionalidadePerfil>>(_entity);

                return _entidade.AsQueryable();
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.Sucesso = false;
            }
            return _entidade.AsQueryable();
        }

        public Entidade.CtrlFuncionalidadePerfil Add(Entidade.CtrlFuncionalidadePerfil item)
        {
            Entidade.CtrlFuncionalidadePerfil retorno = new Entidade.CtrlFuncionalidadePerfil();
            retorno = item;

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;

                    DAL.Models.CtrlFuncionalidadePerfil _entity = AutoMapper.Mapper.Map<DAL.Models.CtrlFuncionalidadePerfil>(item);

                    unitOfWork.CtrlFuncionalidadePerfilRepository.Insert(_entity);
                    unitOfWork.Save();

                    retorno.IdFuncionalidadePerfil = _entity.IdFuncionalidadePerfil;
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

        public void Update(Entidade.CtrlFuncionalidadePerfil item)
        {
            Entidade.CtrlFuncionalidadePerfil retorno = new Entidade.CtrlFuncionalidadePerfil();
            retorno = item;

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;

                    DAL.Models.CtrlFuncionalidadePerfil _entity = AutoMapper.Mapper.Map<DAL.Models.CtrlFuncionalidadePerfil>(item);

                    unitOfWork.CtrlFuncionalidadePerfilRepository.Update(_entity);
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

                    unitOfWork.CtrlFuncionalidadePerfilRepository.Delete(id);
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
