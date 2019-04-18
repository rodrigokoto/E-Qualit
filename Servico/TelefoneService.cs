using System;
using System.Linq;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;

namespace Servico
{
    public partial class Service
    {
        public Entidade.Telefone TelefoneNew()
        {
            var _entidade = new Entidade.Telefone();
            return _entidade;
        }
        
        public Entidade.Telefone TelefoneGetByID(int id)
        {
            Entidade.Telefone _entidade = new Entidade.Telefone();

            try
            {
                Sucesso = true;
				DAL.Entity.Telefone lista = dal.TelefoneGetByID(id);

				_entidade = new Entidade.Telefone(){
								                    IdTelefone = lista.IdTelefone,
                    DsObservacao = lista.DsObservacao,
                    FlTipoTelefone = lista.FlTipoTelefone,
                    IdRelacionado = lista.IdRelacionado,
                    NuRamal = lista.NuRamal,
                    NuTelefone = lista.NuTelefone,

							};
            }
            catch (Exception ex)
            {
                Erros("TelefoneServico.TelefoneGetByID", ex);
            }

            return _entidade;
        }

        public IQueryable<Entidade.Telefone> TelefoneGetAll()
        {
            IQueryable<Entidade.Telefone> _entidade = null;

            try
            {
                Sucesso = true;

				_entidade = from lista in dal.TelefoneGetAll()
                            select new Entidade.Telefone(){
								                    IdTelefone = lista.IdTelefone,
                    DsObservacao = lista.DsObservacao,
                    FlTipoTelefone = lista.FlTipoTelefone,
                    IdRelacionado = lista.IdRelacionado,
                    NuRamal = lista.NuRamal,
                    NuTelefone = lista.NuTelefone,

							};
            }
            catch (DbEntityValidationException e)
            {
                DBErros("TelefoneServico.TelefoneGetAll", e);
            }
            catch (Exception ex)
            {
                Erros("TelefoneServico.TelefoneGetAll", ex);
            }

            return _entidade;
        }

        public Entidade.Telefone TelefoneAdd(Entidade.Telefone item)
        {
			Entidade.Telefone _entidade = new Entidade.Telefone();

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
                   
					DAL.Entity.Telefone lista = dal.TelefoneAdd(item);

					_entidade = new Entidade.Telefone(){
								                    IdTelefone = lista.IdTelefone,
                    DsObservacao = lista.DsObservacao,
                    FlTipoTelefone = lista.FlTipoTelefone,
                    IdRelacionado = lista.IdRelacionado,
                    NuRamal = lista.NuRamal,
                    NuTelefone = lista.NuTelefone,

							};

                    Mensagem = Traducao.Resource.mensagem_registro_incluido;

                   _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
SqlException innerException = e.InnerException.InnerException as SqlException;
            if(innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Telefone") > 0)
            {
                        Error = new Exception(Traducao.Resource.Telefone_PK_Telefone);
            }

                }
                catch (DbEntityValidationException e)
                {
                   DBErros("TelefoneServico.TelefoneAdd", e);
                }
                catch (Exception ex)
                {
                   Erros("TelefoneServico.TelefoneAdd", ex);
                }
            }

			return _entidade;
        }
      
        public Entidade.Telefone TelefoneUpdate(Entidade.Telefone item)
        {
			Entidade.Telefone _entidade = new Entidade.Telefone();
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
					DAL.Entity.Telefone lista = dal.TelefoneUpdate(item);
					_entidade = new Entidade.Telefone(){
								                    IdTelefone = lista.IdTelefone,
                    DsObservacao = lista.DsObservacao,
                    FlTipoTelefone = lista.FlTipoTelefone,
                    IdRelacionado = lista.IdRelacionado,
                    NuRamal = lista.NuRamal,
                    NuTelefone = lista.NuTelefone,

							};

                    Mensagem = Traducao.Resource.mensagem_registro_alterado;

                   _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
SqlException innerException = e.InnerException.InnerException as SqlException;
            if(innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Telefone") > 0)
            {
                        Error = new Exception(Traducao.Resource.Telefone_PK_Telefone);
            }

                }
                catch (DbEntityValidationException e)
                {
                   DBErros("TelefoneServico.TelefoneUpdate", e);
                }
                catch (Exception ex)
                {
                   Erros("TelefoneServico.TelefoneUpdate", ex);
                }
            }
			return _entidade;
        }

       public List<Entidade.Telefone> TelefoneSave(List<Entidade.Telefone> items)
        {
			List<Entidade.Telefone> returnItens = new List<Entidade.Telefone>();

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
					List<DAL.Entity.Telefone> _lista = dal.TelefoneSave(items);

					returnItens = (from lista in _lista
                            select new Entidade.Telefone(){
								                    IdTelefone = lista.IdTelefone,
                    DsObservacao = lista.DsObservacao,
                    FlTipoTelefone = lista.FlTipoTelefone,
                    IdRelacionado = lista.IdRelacionado,
                    NuRamal = lista.NuRamal,
                    NuTelefone = lista.NuTelefone,

							}).ToList();

                    Mensagem = Traducao.Resource.mensagem_registros_alterados;

					_transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
SqlException innerException = e.InnerException.InnerException as SqlException;
            if(innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Telefone") > 0)
            {
                        Error = new Exception(Traducao.Resource.Telefone_PK_Telefone);
            }

                }
                catch (DbEntityValidationException e)
                {
                   DBErros("TelefoneServico.TelefoneSave", e);
                }
                catch (Exception ex)
                {
                   Erros("TelefoneServico.TelefoneSave", ex);
                }
            }

			return returnItens;
        }

		public Entidade.Telefone TelefoneSave(Entidade.Telefone item)
        {
            List<Entidade.Telefone> items = new List<Entidade.Telefone>();
            items.Add(item);

            Entidade.Telefone entidade = TelefoneSave(items).FirstOrDefault();
			if (Sucesso)
            {
                if (item.IdTelefone == 0)
                    Mensagem = Traducao.Resource.mensagem_registro_incluido;
                else
                    Mensagem = Traducao.Resource.mensagem_registro_alterado; 
            }

            return entidade;
        }

        public void TelefoneRemove(int id)
        {
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
					dal.TelefoneRemove(id);

                    Mensagem = Traducao.Resource.mensagem_registro_excluido;

                   _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
SqlException innerException = e.InnerException.InnerException as SqlException;
            if(innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Telefone") > 0)
            {
                        Error = new Exception(Traducao.Resource.Telefone_PK_Telefone);
            }

                }
                catch (DbEntityValidationException e)
                {
                   DBErros("TelefoneServico.TelefoneRemove", e);
                }
                catch (Exception ex)
                {
                   Erros("TelefoneServico.TelefoneRemove", ex);
                }
            }
        }
    }
}
