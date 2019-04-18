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
        public Entidade.UsuarioClienteSite UsuarioClienteSiteNew()
        {
            var _entidade = new Entidade.UsuarioClienteSite();
            return _entidade;
        }

        public Entidade.UsuarioClienteSite UsuarioClienteSiteGetByID(int id)
        {
            Entidade.UsuarioClienteSite _entidade = new Entidade.UsuarioClienteSite();

            try
            {
                Sucesso = true;
                Entidade.UsuarioClienteSite lista = dal.UsuarioClienteSiteGetByID(id);

                _entidade = new Entidade.UsuarioClienteSite()
                {
                    IdUsuarioClienteSite = lista.IdUsuarioClienteSite,
                    IdCliente = lista.IdCliente,
                    IdSite = lista.IdSite,
                    IdUsuario = lista.IdUsuario,

                };
            }
            catch (Exception ex)
            {
                Erros("UsuarioClienteSiteServico.UsuarioClienteSiteGetByID", ex);
            }

            return _entidade;
        }

        public IQueryable<Entidade.UsuarioClienteSite> UsuarioClienteSiteGetAll()
        {
            IQueryable<Entidade.UsuarioClienteSite> _entidade = null;

            try
            {
                Sucesso = true;

                _entidade = from lista in dal.UsuarioClienteSiteGetAll()
                            select new Entidade.UsuarioClienteSite()
                            {
                                IdUsuarioClienteSite = lista.IdUsuarioClienteSite,
                                IdCliente = lista.IdCliente,
                                IdSite = lista.IdSite,
                                IdUsuario = lista.IdUsuario,

                            };
            }
            catch (DbEntityValidationException e)
            {
                DBErros("UsuarioClienteSiteServico.UsuarioClienteSiteGetAll", e);
            }
            catch (Exception ex)
            {
                Erros("UsuarioClienteSiteServico.UsuarioClienteSiteGetAll", ex);
            }

            return _entidade;
        }

        public Entidade.UsuarioClienteSite UsuarioClienteSiteAdd(Entidade.UsuarioClienteSite item)
        {
            Entidade.UsuarioClienteSite _entidade = new Entidade.UsuarioClienteSite();

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;

                    Entidade.UsuarioClienteSite lista = dal.UsuarioClienteSiteAdd(item);

                    _entidade = new Entidade.UsuarioClienteSite()
                    {
                        IdUsuarioClienteSite = lista.IdUsuarioClienteSite,
                        IdCliente = lista.IdCliente,
                        IdSite = lista.IdSite,
                        IdUsuario = lista.IdUsuario,

                    };

                    Mensagem = Traducao.Resource.mensagem_registro_incluido;

                    _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("AK_UsuarioCliente") > 0)
                    {
                        Error = new Exception(Traducao.Resource.UsuarioClienteSite_AK_UsuarioCliente);
                    }
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_UsuarioClienteSite") > 0)
                    {
                        Error = new Exception(Traducao.Resource.UsuarioClienteSite_PK_UsuarioClienteSite);
                    }

                }
                catch (DbEntityValidationException e)
                {
                    DBErros("UsuarioClienteSiteServico.UsuarioClienteSiteAdd", e);
                }
                catch (Exception ex)
                {
                    Erros("UsuarioClienteSiteServico.UsuarioClienteSiteAdd", ex);
                }
            }

            return _entidade;
        }

        public Entidade.UsuarioClienteSite UsuarioClienteSiteUpdate(Entidade.UsuarioClienteSite item)
        {
            Entidade.UsuarioClienteSite _entidade = new Entidade.UsuarioClienteSite();
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
                    Entidade.UsuarioClienteSite lista = dal.UsuarioClienteSiteUpdate(item);
                    _entidade = new Entidade.UsuarioClienteSite()
                    {
                        IdUsuarioClienteSite = lista.IdUsuarioClienteSite,
                        IdCliente = lista.IdCliente,
                        IdSite = lista.IdSite,
                        IdUsuario = lista.IdUsuario,

                    };

                    Mensagem = Traducao.Resource.mensagem_registro_alterado;

                    _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("AK_UsuarioCliente") > 0)
                    {
                        Error = new Exception(Traducao.Resource.UsuarioClienteSite_AK_UsuarioCliente);
                    }
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_UsuarioClienteSite") > 0)
                    {
                        Error = new Exception(Traducao.Resource.UsuarioClienteSite_PK_UsuarioClienteSite);
                    }

                }
                catch (DbEntityValidationException e)
                {
                    DBErros("UsuarioClienteSiteServico.UsuarioClienteSiteUpdate", e);
                }
                catch (Exception ex)
                {
                    Erros("UsuarioClienteSiteServico.UsuarioClienteSiteUpdate", ex);
                }
            }
            return _entidade;
        }

        private List<Entidade.UsuarioClienteSite> UsuarioClienteSiteSave(List<Entidade.UsuarioClienteSite> items)
        {
            List<Entidade.UsuarioClienteSite> returnItens = new List<Entidade.UsuarioClienteSite>();

            Sucesso = true;
            List<Entidade.UsuarioClienteSite> _lista = dal.UsuarioClienteSiteSave(items);

            returnItens = (from lista in _lista
                           select new Entidade.UsuarioClienteSite()
                           {
                               IdUsuarioClienteSite = lista.IdUsuarioClienteSite,
                               IdCliente = lista.IdCliente,
                               IdSite = lista.IdSite,
                               IdUsuario = lista.IdUsuario,

                           }).ToList();

            return returnItens;
        }

        public Entidade.UsuarioClienteSite UsuarioClienteSiteSave(Entidade.UsuarioClienteSite item)
        {
            List<Entidade.UsuarioClienteSite> items = new List<Entidade.UsuarioClienteSite>();
            items.Add(item);

            Entidade.UsuarioClienteSite entidade = UsuarioClienteSiteSave(items).FirstOrDefault();
            if (Sucesso)
            {
                if (item.IdUsuarioClienteSite == 0)
                    Mensagem = Traducao.Resource.mensagem_registro_incluido;
                else
                    Mensagem = Traducao.Resource.mensagem_registro_alterado;
            }

            return entidade;
        }

        public void UsuarioClienteSiteRemove(int id)
        {
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
                    dal.UsuarioClienteSiteRemove(id);

                    Mensagem = Traducao.Resource.mensagem_registro_excluido;

                    _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("AK_UsuarioCliente") > 0)
                    {
                        Error = new Exception(Traducao.Resource.UsuarioClienteSite_AK_UsuarioCliente);
                    }
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_UsuarioClienteSite") > 0)
                    {
                        Error = new Exception(Traducao.Resource.UsuarioClienteSite_PK_UsuarioClienteSite);
                    }

                }
                catch (DbEntityValidationException e)
                {
                    DBErros("UsuarioClienteSiteServico.UsuarioClienteSiteRemove", e);
                }
                catch (Exception ex)
                {
                    Erros("UsuarioClienteSiteServico.UsuarioClienteSiteRemove", ex);
                }
            }
        }

        public List<Entidade.UsuarioClienteSite> UsuarioClienteSiteObterPorUsuario(int idUsuario)
        {
            List<Entidade.UsuarioClienteSite> _entidades = new List<Entidade.UsuarioClienteSite>();

            try
            {
                var usuariosClientesSitesCTX = unitOfWork.UsuarioClienteSiteRepository.Get(x => x.IdUsuario == idUsuario);

                foreach (var usuarioClienteSite in usuariosClientesSitesCTX)
                {
                    _entidades.Add(new Entidade.UsuarioClienteSite()
                    {
                        IdUsuarioClienteSite = usuarioClienteSite.IdUsuarioClienteSite,
                        IdCliente = usuarioClienteSite.IdCliente,
                        IdSite = usuarioClienteSite.IdSite,
                        IdUsuario = usuarioClienteSite.IdUsuario,
                    });
                }

            }
            catch (Exception ex)
            {
                Erros("UsuarioClienteSiteServico.UsuarioClienteSiteGetByID", ex);
            }

            return _entidades;
        }

    }
}
