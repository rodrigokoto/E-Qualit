using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Servico
{
    public class CtrlPerfilServico : BaseService, IDisposable, IGenericService<Entidade.CtrlPerfil>
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

        public Entidade.CtrlPerfil GetById(int id)
        {
            Entidade.CtrlPerfil _entidade = new Entidade.CtrlPerfil();

            try
            {
                this.Sucesso = true;

                DAL.Models.CtrlPerfil _entity = unitOfWork.CtrlPerfilRepository.Get(f => f.IdPefil == id, null, "CtrlPerfil").FirstOrDefault();
                _entidade = AutoMapper.Mapper.Map<Entidade.CtrlPerfil>(_entity);

                return _entidade;
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.Sucesso = false;
            }

            return _entidade;
        }

        public IQueryable<Entidade.CtrlPerfil> GetAll()
        {
            List<Entidade.CtrlPerfil> _entidade = new List<Entidade.CtrlPerfil>();

            try
            {
                this.Sucesso = true;

                List<DAL.Models.CtrlPerfil> _entity = unitOfWork.CtrlPerfilRepository.GetAll().ToList();
                _entidade = AutoMapper.Mapper.Map<List<Entidade.CtrlPerfil>>(_entity);

                return _entidade.AsQueryable();
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.Sucesso = false;
            }
            return _entidade.AsQueryable();
        }

        public Entidade.CtrlPerfil Add(Entidade.CtrlPerfil item)
        {
            Entidade.CtrlPerfil retorno = new Entidade.CtrlPerfil();
            retorno = item;

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;

                    DAL.Models.CtrlPerfil _entity = AutoMapper.Mapper.Map<DAL.Models.CtrlPerfil>(item);

                    unitOfWork.CtrlPerfilRepository.Insert(_entity);
                    unitOfWork.Save();

                    retorno.IdPefil = _entity.IdPefil;
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

        public void Update(Entidade.CtrlPerfil item)
        {
            Entidade.CtrlPerfil retorno = new Entidade.CtrlPerfil();
            retorno = item;

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;

                    DAL.Models.CtrlPerfil _entity = AutoMapper.Mapper.Map<DAL.Models.CtrlPerfil>(item);

                    unitOfWork.CtrlPerfilRepository.Update(_entity);
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

                    unitOfWork.CtrlPerfilRepository.Delete(id);
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
