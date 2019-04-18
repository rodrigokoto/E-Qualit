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
      public Entidade.Endereco EnderecoNew()
      {
         var _entidade = new Entidade.Endereco();
         return _entidade;
      }

      public Entidade.Endereco EnderecoGetByID(int id)
      {
         Entidade.Endereco _entidade = new Entidade.Endereco();

         try
         {
                Sucesso = true;
            DAL.Entity.Endereco lista = dal.EnderecoGetByID(id);

            _entidade = new Entidade.Endereco()
            {
               IdEndereco = lista.IdEndereco,
               CdEstado = lista.CdEstado,
               DsComplemento = lista.DsComplemento,
               DsLogradouro = lista.DsLogradouro,
               DsPais = lista.DsPais,
               FlTipoEndereco = lista.FlTipoEndereco,
               IdRelacionado = lista.IdRelacionado,
               NmBairro = lista.NmBairro,
               NmCidade = lista.NmCidade,
               NuCep = lista.NuCep,
               NuNumero = lista.NuNumero,

            };
         }
         catch (Exception ex)
         {
            Erros("EnderecoServico.EnderecoGetByID", ex);
         }

         return _entidade;
      }

      public IQueryable<Entidade.Endereco> EnderecoGetAll()
      {
         IQueryable<Entidade.Endereco> _entidade = null;

         try
         {
                Sucesso = true;

            _entidade = from lista in dal.EnderecoGetAll()
                        select new Entidade.Endereco()
                        {
                           IdEndereco = lista.IdEndereco,
                           CdEstado = lista.CdEstado,
                           DsComplemento = lista.DsComplemento,
                           DsLogradouro = lista.DsLogradouro,
                           DsPais = lista.DsPais,
                           FlTipoEndereco = lista.FlTipoEndereco,
                           IdRelacionado = lista.IdRelacionado,
                           NmBairro = lista.NmBairro,
                           NmCidade = lista.NmCidade,
                           NuCep = lista.NuCep,
                           NuNumero = lista.NuNumero,

                        };
         }
         catch (DbEntityValidationException e)
         {
            DBErros("EnderecoServico.EnderecoGetAll", e);
         }
         catch (Exception ex)
         {
            Erros("EnderecoServico.EnderecoGetAll", ex);
         }

         return _entidade;
      }

      public Entidade.Endereco EnderecoAdd(Entidade.Endereco item)
      {
         Entidade.Endereco _entidade = new Entidade.Endereco();

         using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
         {
            try
            {
                    Sucesso = true;

               DAL.Entity.Endereco lista = dal.EnderecoAdd(item);

               _entidade = new Entidade.Endereco()
               {
                  IdEndereco = lista.IdEndereco,
                  CdEstado = lista.CdEstado,
                  DsComplemento = lista.DsComplemento,
                  DsLogradouro = lista.DsLogradouro,
                  DsPais = lista.DsPais,
                  FlTipoEndereco = lista.FlTipoEndereco,
                  IdRelacionado = lista.IdRelacionado,
                  NmBairro = lista.NmBairro,
                  NmCidade = lista.NmCidade,
                  NuCep = lista.NuCep,
                  NuNumero = lista.NuNumero,

               };

                    Mensagem = Traducao.Resource.mensagem_registro_incluido;

               _transaction.Complete();
            }
            catch (DbUpdateException e)
            {
               SqlException innerException = e.InnerException.InnerException as SqlException;
               if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Endereco") > 0)
               {
                  Erros("EnderecoServico.EnderecoAdd", new Exception(Traducao.Resource.Endereco_PK_Endereco));
               }
               Erros("EnderecoServico.EnderecoAdd", new Exception(e.Message));
            }
            catch (DbEntityValidationException e)
            {
               DBErros("EnderecoServico.EnderecoAdd", e);
            }
            catch (Exception ex)
            {
               Erros("EnderecoServico.EnderecoAdd", ex);
            }
         }

         return _entidade;
      }

      public Entidade.Endereco EnderecoUpdate(Entidade.Endereco item)
      {
         Entidade.Endereco _entidade = new Entidade.Endereco();
         using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
         {
            try
            {
                    Sucesso = true;
               DAL.Entity.Endereco lista = dal.EnderecoUpdate(item);
               _entidade = new Entidade.Endereco()
               {
                  IdEndereco = lista.IdEndereco,
                  CdEstado = lista.CdEstado,
                  DsComplemento = lista.DsComplemento,
                  DsLogradouro = lista.DsLogradouro,
                  DsPais = lista.DsPais,
                  FlTipoEndereco = lista.FlTipoEndereco,
                  IdRelacionado = lista.IdRelacionado,
                  NmBairro = lista.NmBairro,
                  NmCidade = lista.NmCidade,
                  NuCep = lista.NuCep,
                  NuNumero = lista.NuNumero,

               };

                    Mensagem = Traducao.Resource.mensagem_registro_alterado;

               _transaction.Complete();
            }
            catch (DbUpdateException e)
            {
                    Sucesso = false;
               SqlException innerException = e.InnerException.InnerException as SqlException;
               if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Endereco") > 0)
               {
                        Error = new Exception(Traducao.Resource.Endereco_PK_Endereco);
               }

            }
            catch (DbEntityValidationException e)
            {
               DBErros("EnderecoServico.EnderecoUpdate", e);
            }
            catch (Exception ex)
            {
               Erros("EnderecoServico.EnderecoUpdate", ex);
            }
         }
         return _entidade;
      }

      public List<Entidade.Endereco> EnderecoSave(List<Entidade.Endereco> items)
      {
         List<Entidade.Endereco> returnItens = new List<Entidade.Endereco>();

         using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
         {
            try
            {
                    Sucesso = true;
               List<DAL.Entity.Endereco> _lista = dal.EnderecoSave(items);

               returnItens = (from lista in _lista
                              select new Entidade.Endereco()
                              {
                                 IdEndereco = lista.IdEndereco,
                                 CdEstado = lista.CdEstado,
                                 DsComplemento = lista.DsComplemento,
                                 DsLogradouro = lista.DsLogradouro,
                                 DsPais = lista.DsPais,
                                 FlTipoEndereco = lista.FlTipoEndereco,
                                 IdRelacionado = lista.IdRelacionado,
                                 NmBairro = lista.NmBairro,
                                 NmCidade = lista.NmCidade,
                                 NuCep = lista.NuCep,
                                 NuNumero = lista.NuNumero,

                              }).ToList();

                    Mensagem = Traducao.Resource.mensagem_registros_alterados;

               _transaction.Complete();
            }
            catch (DbUpdateException e)
            {
                    Sucesso = false;
               SqlException innerException = e.InnerException.InnerException as SqlException;
               if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Endereco") > 0)
               {
                        Error = new Exception(Traducao.Resource.Endereco_PK_Endereco);
               }

            }
            catch (DbEntityValidationException e)
            {
               DBErros("EnderecoServico.EnderecoSave", e);
            }
            catch (Exception ex)
            {
               Erros("EnderecoServico.EnderecoSave", ex);
            }
         }

         return returnItens;
      }

      public Entidade.Endereco EnderecoSave(Entidade.Endereco item)
      {
         List<Entidade.Endereco> items = new List<Entidade.Endereco>();
         items.Add(item);

         Entidade.Endereco entidade = EnderecoSave(items).FirstOrDefault();
         if (Sucesso)
         {
            if (item.IdEndereco == 0)
                    Mensagem = Traducao.Resource.mensagem_registro_incluido;
            else
                    Mensagem = Traducao.Resource.mensagem_registro_alterado;
         }

         return entidade;
      }

      public void EnderecoRemove(int id)
      {
         using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
         {
            try
            {
                    Sucesso = true;
               dal.EnderecoRemove(id);

                    Mensagem = Traducao.Resource.mensagem_registro_excluido;

               _transaction.Complete();
            }
            catch (DbUpdateException e)
            {
                    Sucesso = false;
               SqlException innerException = e.InnerException.InnerException as SqlException;
               if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Endereco") > 0)
               {
                        Error = new Exception(Traducao.Resource.Endereco_PK_Endereco);
               }

            }
            catch (DbEntityValidationException e)
            {
               DBErros("EnderecoServico.EnderecoRemove", e);
            }
            catch (Exception ex)
            {
               Erros("EnderecoServico.EnderecoRemove", ex);
            }
         }
      }
   }
}
