using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace Servico
{
    public partial class Service
    {
        public Entidade.Cadastro CadastroNew()
        {
            var _entidade = new Entidade.Cadastro();
            return _entidade;
        }

        public Entidade.Cadastro CadastroGetByID(int id)
        {
            Entidade.Cadastro _entidade = new Entidade.Cadastro();

            try
            {
                Sucesso = true;
                DAL.Entity.Cadastro lista = dal.CadastroGetByID(id);  //service.CadastroGetByID(id);

                _entidade = new Entidade.Cadastro()
                {
                    IdCadastro = lista.IdCadastro,
                    CdTabela = lista.CdTabela,
                    DsDescricao = lista.DsDescricao,
                    FlAtivo = lista.FlAtivo,
                    IdSite = lista.IdSite,

                };
            }
            catch (Exception ex)
            {
                Erros("CadastroServico.CadastroGetByID", ex);
            }

            return _entidade;
        }

        public IQueryable<Entidade.Cadastro> CadastroGetAll()
        {
            IQueryable<Entidade.Cadastro> _entidade = null;

            try
            {
                Sucesso = true;

                _entidade = from lista in dal.CadastroGetAll()
                            select new Entidade.Cadastro()
                            {
                                IdCadastro = lista.IdCadastro,
                                CdTabela = lista.CdTabela,
                                DsDescricao = lista.DsDescricao,
                                FlAtivo = lista.FlAtivo,
                                IdSite = lista.IdSite,

                            };
            }
            catch (DbEntityValidationException e)
            {
                DBErros("CadastroServico.CadastroGetAll", e);
            }
            catch (Exception ex)
            {
                Erros("CadastroServico.CadastroGetAll", ex);
            }

            return _entidade;
        }

        public Entidade.Cadastro CadastroAdd(Entidade.Cadastro item)
        {
            Entidade.Cadastro _entidade = new Entidade.Cadastro();

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;

                    DAL.Entity.Cadastro lista = dal.CadastroAdd(item);

                    _entidade = new Entidade.Cadastro()
                    {
                        IdCadastro = lista.IdCadastro,
                        CdTabela = lista.CdTabela,
                        DsDescricao = lista.DsDescricao,
                        FlAtivo = lista.FlAtivo,
                        IdSite = lista.IdSite,

                    };

                    Mensagem = Traducao.Resource.mensagem_registro_incluido;

                    _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Cadastro") > 0)
                    {
                        //Erros("CadastroServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cadastro_PK_Cadastro));
                    }
                    else
                    {
                        Erros("CadastroServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(e.Message));
                    }

                }
                catch (DbEntityValidationException e)
                {
                    DBErros("CadastroServico.CadastroAdd", e);
                }
                catch (Exception ex)
                {
                    Erros("CadastroServico.CadastroAdd", ex);
                }
            }

            return _entidade;
        }

        public Entidade.Cadastro CadastroUpdate(Entidade.Cadastro item)
        {
            Entidade.Cadastro _entidade = new Entidade.Cadastro();
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
                    DAL.Entity.Cadastro lista = dal.CadastroUpdate(item);
                    _entidade = new Entidade.Cadastro()
                    {
                        IdCadastro = lista.IdCadastro,
                        CdTabela = lista.CdTabela,
                        DsDescricao = lista.DsDescricao,
                        FlAtivo = lista.FlAtivo,
                        IdSite = lista.IdSite,

                    };

                    Mensagem = Traducao.Resource.mensagem_registro_alterado;

                    _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Cadastro") > 0)
                    {
                        //Erros("CadastroServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cadastro_PK_Cadastro));
                    }
                    else
                    {
                        Erros("CadastroServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(e.Message));
                    }

                }
                catch (DbEntityValidationException e)
                {
                    DBErros("CadastroServico.CadastroUpdate", e);
                }
                catch (Exception ex)
                {
                    Erros("CadastroServico.CadastroUpdate", ex);
                }
            }
            return _entidade;
        }

        public List<Entidade.Cadastro> CadastroSave(List<Entidade.Cadastro> items)
        {
            List<Entidade.Cadastro> returnItens = new List<Entidade.Cadastro>();

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
                    List<DAL.Entity.Cadastro> _lista = dal.CadastroSave(items);

                    returnItens = (from lista in _lista
                                   select new Entidade.Cadastro()
                                   {
                                       IdCadastro = lista.IdCadastro,
                                       CdTabela = lista.CdTabela,
                                       DsDescricao = lista.DsDescricao,
                                       FlAtivo = lista.FlAtivo,
                                       IdSite = lista.IdSite,

                                   }).ToList();

                    Mensagem = Traducao.Resource.mensagem_registros_alterados;

                    _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Cadastro") > 0)
                    {
                        //Erros("CadastroServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cadastro_PK_Cadastro));
                    }
                    else
                    {
                        Erros("CadastroServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(e.Message));
                    }

                }
                catch (DbEntityValidationException e)
                {
                    DBErros("CadastroServico.CadastroSave", e);
                }
                catch (Exception ex)
                {
                    Erros("CadastroServico.CadastroSave", ex);
                }
            }

            return returnItens;
        }

        public Entidade.Cadastro CadastroSave(Entidade.Cadastro item)
        {
            List<Entidade.Cadastro> items = new List<Entidade.Cadastro>();
            items.Add(item);

            Entidade.Cadastro entidade = CadastroSave(items).FirstOrDefault();
            if (Sucesso)
            {
                if (item.IdCadastro == 0)
                    Mensagem = Traducao.Resource.mensagem_registro_incluido;
                else
                    Mensagem = Traducao.Resource.mensagem_registro_alterado;
            }

            return entidade;
        }

        public void CadastroRemove(int id)
        {
            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    Sucesso = true;
                    dal.CadastroRemove(id);

                    Mensagem = Traducao.Resource.mensagem_registro_excluido;

                    _transaction.Complete();
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Cadastro") > 0)
                    {
                        //Erros("CadastroServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(Traducao.Resource.Cadastro_PK_Cadastro));
                    }
                    else
                    {
                        Erros("CadastroServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(e.Message));
                    }

                }
                catch (DbEntityValidationException e)
                {
                    DBErros("CadastroServico.CadastroRemove", e);
                }
                catch (Exception ex)
                {
                    Erros("CadastroServico.CadastroRemove", ex);
                }
            }
        }
        public List<Entidade.Cadastro> CadastroGetByTipo(string tipo)
        {
            List<Entidade.Cadastro> _entidade = null;

            try
            {
                Sucesso = true;

                _entidade = (from x in unitOfWork.CadastroRepository.Get(x => x.CdTabela == tipo, o => o.OrderBy(ob => ob.DsDescricao))
                            select new Entidade.Cadastro()
                            {
                                IdCadastro= x.IdCadastro,
                                DsDescricao = x.DsDescricao,
                                CdTabela = x.CdTabela,
                                FlAtivo = x.FlAtivo,
                                IdSite = x.IdSite,
                                
                            }).ToList();
                            
            }
            catch (DbEntityValidationException e)
            {
                DBErros("CadastroServico.CadastroGetAll", e);
            }
            catch (Exception ex)
            {
                Erros("CadastroServico.CadastroGetAll", ex);
            }

            return _entidade;
        }
    }
}
