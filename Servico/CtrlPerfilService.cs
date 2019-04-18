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
      public Entidade.CtrlPerfil CtrlPerfilNew()
      {
         var _entidade = new Entidade.CtrlPerfil();
         return _entidade;
      }

      public Entidade.CtrlPerfil CtrlPerfilGetByID(int id)
      {
         Entidade.CtrlPerfil _entidade = new Entidade.CtrlPerfil();

         try
         {
                Sucesso = true;
            Entidade.CtrlPerfil lista = dal.CtrlPerfilGetByID(id);

            _entidade = new Entidade.CtrlPerfil()
            {
               IdPerfil = lista.IdPerfil,
               NmNome = lista.NmNome,

            };
         }
         catch (Exception ex)
         {
            Erros("CtrlPerfilServico.CtrlPerfilGetByID", ex);
         }

         return _entidade;
      }

      public IQueryable<Entidade.CtrlPerfil> CtrlPerfilGetAll()
      {
         IQueryable<Entidade.CtrlPerfil> _entidade = null;

         try
         {
                Sucesso = true;

            _entidade = from lista in dal.CtrlPerfilGetAll()
                        select new Entidade.CtrlPerfil()
                        {
                           IdPerfil = lista.IdPerfil,
                           NmNome = lista.NmNome,

                        };
         }
         catch (DbEntityValidationException e)
         {
            DBErros("CtrlPerfilServico.CtrlPerfilGetAll", e);
         }
         catch (Exception ex)
         {
            Erros("CtrlPerfilServico.CtrlPerfilGetAll", ex);
         }

         return _entidade;
      }

      public Entidade.CtrlPerfil CtrlPerfilAdd(Entidade.CtrlPerfil item)
      {
         Entidade.CtrlPerfil _entidade = new Entidade.CtrlPerfil();

         using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
         {
            try
            {
                    Sucesso = true;

               Entidade.CtrlPerfil lista = dal.CtrlPerfilAdd(item);

               _entidade = new Entidade.CtrlPerfil()
               {
                  IdPerfil = lista.IdPerfil,
                  NmNome = lista.NmNome,

               };

                    Mensagem = Traducao.Resource.mensagem_registro_incluido;

               _transaction.Complete();
            }
            catch (DbUpdateException e)
            {
                    Sucesso = false;
               SqlException innerException = e.InnerException.InnerException as SqlException;
               if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_CtrlPerfil") > 0)
               {
                        Error = new Exception(Traducao.Resource.CtrlPerfil_PK_CtrlPerfil);
               }

            }
            catch (DbEntityValidationException e)
            {
               DBErros("CtrlPerfilServico.CtrlPerfilAdd", e);
            }
            catch (Exception ex)
            {
               Erros("CtrlPerfilServico.CtrlPerfilAdd", ex);
            }
         }

         return _entidade;
      }

      public Entidade.CtrlPerfil CtrlPerfilUpdate(Entidade.CtrlPerfil item)
      {
         Entidade.CtrlPerfil _entidade = new Entidade.CtrlPerfil();
         using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
         {
            try
            {
                    Sucesso = true;
               Entidade.CtrlPerfil lista = dal.CtrlPerfilUpdate(item);
               _entidade = new Entidade.CtrlPerfil()
               {
                  IdPerfil = lista.IdPerfil,
                  NmNome = lista.NmNome,

               };

                    Mensagem = Traducao.Resource.mensagem_registro_alterado;

               _transaction.Complete();
            }
            catch (DbUpdateException e)
            {
                    Sucesso = false;
               SqlException innerException = e.InnerException.InnerException as SqlException;
               if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_CtrlPerfil") > 0)
               {
                        Error = new Exception(Traducao.Resource.CtrlPerfil_PK_CtrlPerfil);
               }

            }
            catch (DbEntityValidationException e)
            {
               DBErros("CtrlPerfilServico.CtrlPerfilUpdate", e);
            }
            catch (Exception ex)
            {
               Erros("CtrlPerfilServico.CtrlPerfilUpdate", ex);
            }
         }
         return _entidade;
      }

      public List<Entidade.CtrlPerfil> CtrlPerfilSave(List<Entidade.CtrlPerfil> items)
      {
         List<Entidade.CtrlPerfil> returnItens = new List<Entidade.CtrlPerfil>();

         using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
         {
            try
            {
                    Sucesso = true;
               List<Entidade.CtrlPerfil> _lista = dal.CtrlPerfilSave(items);

               returnItens = (from lista in _lista
                              select new Entidade.CtrlPerfil()
                              {
                                 IdPerfil = lista.IdPerfil,
                                 NmNome = lista.NmNome,

                              }).ToList();

                    Mensagem = Traducao.Resource.mensagem_registros_alterados;

               _transaction.Complete();
            }
            catch (DbUpdateException e)
            {
                    Sucesso = false;
               SqlException innerException = e.InnerException.InnerException as SqlException;
               if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_CtrlPerfil") > 0)
               {
                        Error = new Exception(Traducao.Resource.CtrlPerfil_PK_CtrlPerfil);
               }

            }
            catch (DbEntityValidationException e)
            {
               DBErros("CtrlPerfilServico.CtrlPerfilSave", e);
            }
            catch (Exception ex)
            {
               Erros("CtrlPerfilServico.CtrlPerfilSave", ex);
            }
         }

         return returnItens;
      }

      public Entidade.CtrlPerfil CtrlPerfilSave(Entidade.CtrlPerfil item)
      {
         List<Entidade.CtrlPerfil> items = new List<Entidade.CtrlPerfil>();
         items.Add(item);

         Entidade.CtrlPerfil entidade = CtrlPerfilSave(items).FirstOrDefault();
         if (Sucesso)
         {
            if (item.IdPerfil == 0)
                    Mensagem = Traducao.Resource.mensagem_registro_incluido;
            else
                    Mensagem = Traducao.Resource.mensagem_registro_alterado;
         }

         return entidade;
      }

      public void CtrlPerfilRemove(int id)
      {
         using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
         {
            try
            {
                    Sucesso = true;
               dal.CtrlPerfilRemove(id);

                    Mensagem = Traducao.Resource.mensagem_registro_excluido;

               _transaction.Complete();
            }
            catch (DbUpdateException e)
            {
                    Sucesso = false;
               SqlException innerException = e.InnerException.InnerException as SqlException;
               if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_CtrlPerfil") > 0)
               {
                        Error = new Exception(Traducao.Resource.CtrlPerfil_PK_CtrlPerfil);
               }

            }
            catch (DbEntityValidationException e)
            {
               DBErros("CtrlPerfilServico.CtrlPerfilRemove", e);
            }
            catch (Exception ex)
            {
               Erros("CtrlPerfilServico.CtrlPerfilRemove", ex);
            }
         }
      }
   }
}
