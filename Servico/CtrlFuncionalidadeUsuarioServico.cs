using Repository.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Servico
{
    public class CtrlFuncionalidadeUsuarioServico : BaseService, IDisposable, IGenericService<Entidade.CtrlFuncionalidadeUsuario>
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

        public Entidade.CtrlFuncionalidadeUsuario GetById(int id)
        {
            Entidade.CtrlFuncionalidadeUsuario _entidade = new Entidade.CtrlFuncionalidadeUsuario();

            try
            {
                this.Sucesso = true;

                DAL.Models.CtrlFuncionalidadeUsuario _entity = unitOfWork.CtrlFuncionalidadeUsuarioRepository.Get(f => f.IdUsuario == id, null, "CtrlPerfil").FirstOrDefault();
                _entidade = AutoMapper.Mapper.Map<Entidade.CtrlFuncionalidadeUsuario>(_entity);

                return _entidade;
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.Sucesso = false;
            }

            return _entidade;
        }

        public IQueryable<Entidade.CtrlFuncionalidadeUsuario> GetAll()
        {
            List<Entidade.CtrlFuncionalidadeUsuario> _entidade = new List<Entidade.CtrlFuncionalidadeUsuario>();

            try
            {
                this.Sucesso = true;

                List<DAL.Models.CtrlFuncionalidadeUsuario> _entity = unitOfWork.CtrlFuncionalidadeUsuarioRepository.GetAll().ToList();
                _entidade = AutoMapper.Mapper.Map<List<Entidade.CtrlFuncionalidadeUsuario>>(_entity);

                return _entidade.AsQueryable();
            }
            catch (Exception ex)
            {
                this.Error = ex;
                this.Sucesso = false;
            }
            return _entidade.AsQueryable();
        }

        public Entidade.CtrlFuncionalidadeUsuario Add(Entidade.CtrlFuncionalidadeUsuario item)
        {
            Entidade.CtrlFuncionalidadeUsuario retorno = new Entidade.CtrlFuncionalidadeUsuario();
            retorno = item;

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;

                    DAL.Models.CtrlFuncionalidadeUsuario _entity = AutoMapper.Mapper.Map<DAL.Models.CtrlFuncionalidadeUsuario>(item);

                    unitOfWork.CtrlFuncionalidadeUsuarioRepository.Insert(_entity);
                    unitOfWork.Save();

                    retorno.IdUsuario = _entity.IdUsuario;
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

        public void Update(Entidade.CtrlFuncionalidadeUsuario item)
        {
            Entidade.CtrlFuncionalidadeUsuario retorno = new Entidade.CtrlFuncionalidadeUsuario();
            retorno = item;

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    this.Sucesso = true;

                    DAL.Models.CtrlFuncionalidadeUsuario _entity = AutoMapper.Mapper.Map<DAL.Models.CtrlFuncionalidadeUsuario>(item);

                    unitOfWork.CtrlFuncionalidadeUsuarioRepository.Update(_entity);
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

                    unitOfWork.CtrlFuncionalidadeUsuarioRepository.Delete(id);
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
