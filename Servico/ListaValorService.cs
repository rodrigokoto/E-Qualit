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
        public Entidade.ListaValor ListaValorNew()
        {
            var _entidade = new Entidade.ListaValor();
            return _entidade;
        }
        
        public Entidade.ListaValor ListaValorGetByID(int id)
        {
            Entidade.ListaValor _entidade = new Entidade.ListaValor();

            try
            {
                Sucesso = true;
				DAL.Entity.ListaValor lista = dal.ListaValorGetByID(id);

				_entidade = new Entidade.ListaValor(){
								                    IdListaValor = lista.IdListaValor,
                    CdCodigo = lista.CdCodigo,
                    CdTabela = lista.CdTabela,
                    DsDescricao = lista.DsDescricao,

							};
            }
            catch (Exception ex)
            {
                Erros("ListaValorServico.ListaValorGetByID", ex);
            }

            return _entidade;
        }

        public IQueryable<Entidade.ListaValor> ListaValorGetAll()
        {
            IQueryable<Entidade.ListaValor> _entidade = null;

            try
            {
                Sucesso = true;

				_entidade = from lista in dal.ListaValorGetAll()
                            select new Entidade.ListaValor(){
								                    IdListaValor = lista.IdListaValor,
                    CdCodigo = lista.CdCodigo,
                    CdTabela = lista.CdTabela,
                    DsDescricao = lista.DsDescricao,

							};
            }
            catch (DbEntityValidationException e)
            {
                DBErros("ListaValorServico.ListaValorGetAll", e);
            }
            catch (Exception ex)
            {
                Erros("ListaValorServico.ListaValorGetAll", ex);
            }

            return _entidade;
        }

        public Entidade.ListaValor ListaValorAdd(Entidade.ListaValor item)
        {
			Entidade.ListaValor _entidade = new Entidade.ListaValor();

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
                   
					DAL.Entity.ListaValor lista = dal.ListaValorAdd(item);

					_entidade = new Entidade.ListaValor(){
								                    IdListaValor = lista.IdListaValor,
                    CdCodigo = lista.CdCodigo,
                    CdTabela = lista.CdTabela,
                    DsDescricao = lista.DsDescricao,

							};

                    Mensagem = Traducao.Resource.mensagem_registro_incluido;

                   _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
SqlException innerException = e.InnerException.InnerException as SqlException;
            if(innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_ListaValor") > 0)
            {
                        Error = new Exception(Traducao.Resource.ListaValor_PK_ListaValor);
            }

                }
                catch (DbEntityValidationException e)
                {
                   DBErros("ListaValorServico.ListaValorAdd", e);
                }
                catch (Exception ex)
                {
                   Erros("ListaValorServico.ListaValorAdd", ex);
                }
            }

			return _entidade;
        }
      
        public Entidade.ListaValor ListaValorUpdate(Entidade.ListaValor item)
        {
			Entidade.ListaValor _entidade = new Entidade.ListaValor();
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
					DAL.Entity.ListaValor lista = dal.ListaValorUpdate(item);
					_entidade = new Entidade.ListaValor(){
								                    IdListaValor = lista.IdListaValor,
                    CdCodigo = lista.CdCodigo,
                    CdTabela = lista.CdTabela,
                    DsDescricao = lista.DsDescricao,

							};

                    Mensagem = Traducao.Resource.mensagem_registro_alterado;

                   _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
SqlException innerException = e.InnerException.InnerException as SqlException;
            if(innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_ListaValor") > 0)
            {
                        Error = new Exception(Traducao.Resource.ListaValor_PK_ListaValor);
            }

                }
                catch (DbEntityValidationException e)
                {
                   DBErros("ListaValorServico.ListaValorUpdate", e);
                }
                catch (Exception ex)
                {
                   Erros("ListaValorServico.ListaValorUpdate", ex);
                }
            }
			return _entidade;
        }

       public List<Entidade.ListaValor> ListaValorSave(List<Entidade.ListaValor> items)
        {
			List<Entidade.ListaValor> returnItens = new List<Entidade.ListaValor>();

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
					List<DAL.Entity.ListaValor> _lista = dal.ListaValorSave(items);

					returnItens = (from lista in _lista
                            select new Entidade.ListaValor(){
								                    IdListaValor = lista.IdListaValor,
                    CdCodigo = lista.CdCodigo,
                    CdTabela = lista.CdTabela,
                    DsDescricao = lista.DsDescricao,

							}).ToList();

                    Mensagem = Traducao.Resource.mensagem_registros_alterados;

					_transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
SqlException innerException = e.InnerException.InnerException as SqlException;
            if(innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_ListaValor") > 0)
            {
                        Error = new Exception(Traducao.Resource.ListaValor_PK_ListaValor);
            }

                }
                catch (DbEntityValidationException e)
                {
                   DBErros("ListaValorServico.ListaValorSave", e);
                }
                catch (Exception ex)
                {
                   Erros("ListaValorServico.ListaValorSave", ex);
                }
            }

			return returnItens;
        }

		public Entidade.ListaValor ListaValorSave(Entidade.ListaValor item)
        {
            List<Entidade.ListaValor> items = new List<Entidade.ListaValor>();
            items.Add(item);

            Entidade.ListaValor entidade = ListaValorSave(items).FirstOrDefault();
			if (Sucesso)
            {
                if (item.IdListaValor == 0)
                    Mensagem = Traducao.Resource.mensagem_registro_incluido;
                else
                    Mensagem = Traducao.Resource.mensagem_registro_alterado; 
            }

            return entidade;
        }

        public void ListaValorRemove(int id)
        {
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
					dal.ListaValorRemove(id);

                    Mensagem = Traducao.Resource.mensagem_registro_excluido;

                   _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
SqlException innerException = e.InnerException.InnerException as SqlException;
            if(innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_ListaValor") > 0)
            {
                        Error = new Exception(Traducao.Resource.ListaValor_PK_ListaValor);
            }

                }
                catch (DbEntityValidationException e)
                {
                   DBErros("ListaValorServico.ListaValorRemove", e);
                }
                catch (Exception ex)
                {
                   Erros("ListaValorServico.ListaValorRemove", ex);
                }
            }
        }
    }
}
