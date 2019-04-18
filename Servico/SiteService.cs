using System;
using System.Linq;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Servico.Validacao;
using Dominio.Enumerado;

namespace Servico
{
    public partial class Service
    {
        private UsuarioValidacao _validaUsuario = new UsuarioValidacao();
        private SiteValidacao _validaSite = new SiteValidacao();
        private ModuloValidacao _validaModulo = new ModuloValidacao();

        public Entidade.Site SiteNew()
        {
            var _entidade = new Entidade.Site();

            _entidade.LvModulo = ObterModulos(0);
            _entidade.FlAtivo = true;
            _entidade.LvProcesso.Add(new Entidade.Processo() { NmProcesso = "Qualidade", FlQualidade = true, FlAtivo = true });

            return _entidade;
        }

        private List<Entidade.Funcionalidade> ObterModulos(int idSite)
        {
            List<Entidade.Funcionalidade> _modulos = new List<Entidade.Funcionalidade>();
            //Lê todos os módulos do sistema
            IList<DAL.Entity.Funcionalidade> _funcionalidades = unitOfWork.FuncionalidadeRepository
                                                                         .GetAll()
                                                                         .Where(w => w.IdFuncionalidadePai == null)
                                                                         .OrderBy(o => o.NuOrdem)
                                                                         .ThenBy(tb => tb.NmNome)
                                                                         .ToList();

            foreach (DAL.Entity.Funcionalidade f in _funcionalidades)
            {
                List<Entidade.Funcionalidade> _feature = new List<Entidade.Funcionalidade>();

                //Verifica se foi selecionado o módulo para o site
                DAL.Entity.SiteModulo _siteModulo = unitOfWork.SiteModuloRepository
                                                              .Get(filter => filter.IdSite == idSite && filter.IdModulo == f.IdFuncionalidade)
                                                              .FirstOrDefault();

                //Verifica se o módulo tem recurso
                List<DAL.Entity.Funcionalidade> _recurso = unitOfWork.FuncionalidadeRepository
                                                              .Get(filter => filter.IdFuncionalidadePai == f.IdFuncionalidade)
                                                              .ToList();

                if (_recurso != null)
                {
                    foreach (DAL.Entity.Funcionalidade _itemRecurso in _recurso)
                    {

                        //Verifica se o recurso foi selecionado para o módulo
                        DAL.Entity.SiteModulo _siteFeature = unitOfWork.SiteModuloRepository
                                                                      .Get(filter => filter.IdSite == idSite && filter.IdModulo == _itemRecurso.IdFuncionalidade)
                                                                      .FirstOrDefault();

                        _feature.Add(new Entidade.Funcionalidade()
                        {
                            IdFuncionalidade = _itemRecurso.IdFuncionalidade,
                            IdFuncionalidadePai = _itemRecurso.IdFuncionalidadePai,
                            NmNome = _itemRecurso.NmNome,
                            CdFormulario = _itemRecurso.CdFormulario,
                            NuOrdem = _itemRecurso.NuOrdem,
                            FlSelecionado = _siteFeature == null ? false : true
                        });
                    }
                }

                //Adiciona o módulo e os recursos
                _modulos.Add(new Entidade.Funcionalidade()
                {
                    IdFuncionalidade = f.IdFuncionalidade,
                    IdFuncionalidadePai = f.IdFuncionalidadePai,
                    NmNome = f.NmNome,
                    CdFormulario = f.CdFormulario,
                    NuOrdem = f.NuOrdem,
                    FlSelecionado = _siteModulo == null ? false : true,
                    LvFeature = _feature
                });
            }

            return _modulos;
        }

        private List<Entidade.Processo> ObterProcessos(int idSite)
        {
            List<Entidade.Processo> _processos = new List<Entidade.Processo>();
            foreach (DAL.Entity.Processo p in dal.ProcessoGetByIdSite(idSite).OrderBy(o => o.NmProcesso))
            {
                _processos.Add(new Entidade.Processo()
                {
                    IdProcesso = p.IdProcesso,
                    IdSite = p.IdSite,
                    NmProcesso = p.NmProcesso,
                    FlAtivo = p.FlAtivo,
                    FlQualidade = p.FlQualidade
                });
            }
            return _processos;
        }

        public Entidade.Site SiteGetByID(int id)
        {
            Entidade.Site _entidade = new Entidade.Site();

            try
            {
                Sucesso = true;

                DAL.Entity.Site lista = dal.SiteGetByID(id);

                _entidade = new Entidade.Site()
                {
                    IdSite = lista.IdSite,
                    DsFrase = lista.DsFrase,
                    DsObservacoes = lista.DsObservacoes,
                    FlAtivo = lista.FlAtivo,
                    IdCliente = lista.IdCliente,
                    NmFantasia = lista.NmFantasia,
                    NmLogo = lista.NmLogo,
                    NmRazaoSocial = lista.NmRazaoSocial,
                    NuCNPJ = lista.NuCNPJ,
                };

                _entidade.LvModulo = ObterModulos(_entidade.IdSite);
                _entidade.LvProcesso = ObterProcessos(_entidade.IdSite);
            }
            catch (Exception ex)
            {
                Erros("SiteServico.SiteGetByID", ex);
            }

            return _entidade;
        }

        /// <summary>
        /// O método retorna uma lista de sites conforme o perfil do usuário.
        /// Implementadas as regras RN3112 e RN4108
        /// </summary>
        /// <param name="idUsuario">ID do Usuário para validação de permissão</param>
        /// <param name="idCliente">ID do Cliente para obtenção dos Sites</param>
        /// <returns>IList<Entidade.Site></returns>
        public IList<Entidade.Site> SiteGetByPerfilUsuario(int idUsuario, int idCliente)
        {
            IList<Entidade.Site> _entidade = new List<Entidade.Site>();

            try
            {
                Sucesso = true;

                IList<DAL.Entity.Site> lista = dal.SiteGetByIdCliente(idCliente).ToList();
                if (lista.Count == 0) return _entidade;

                Entidade.CtrlUsuario _usuario = dal.CtrlUsuarioGetByID(idUsuario);

                if (_usuario.IdPerfil != (int)PerfisAcesso.Administrador)
                {
                    if (_usuario.IdPerfil == (int)PerfisAcesso.Colaborador)
                    {
                        throw new Exception(Traducao.Resource.Acesso_Negado);
                    }
                    else if (_usuario.IdPerfil == (int)PerfisAcesso.Suporte)
                    {
                        List<int> idsCliente = dal.UsuarioClienteSiteGetAll()
                                                      .Where(w => w.IdUsuario == idUsuario)
                                                      .Select(s => s.IdCliente)
                                                      .ToList();

                        lista = lista.Where(w => idsCliente.Contains(w.IdCliente)).ToList();
                    }
                    else if (_usuario.IdPerfil == (int)PerfisAcesso.Coordenador)
                    {
                        List<int> idsSite = dal.UsuarioClienteSiteGetAll()
                                                   .Where(w => w.IdUsuario == idUsuario && w.IdCliente == idCliente && w.IdSite != null)
                                                   .Select(s => s.IdSite.Value)
                                                   .ToList();

                        lista = lista.Where(w => idsSite.Contains(w.IdSite) && w.FlAtivo).ToList();
                    }
                }

                foreach (DAL.Entity.Site item in lista)
                {
                    Entidade.Site site = new Entidade.Site()
                    {
                        IdSite = item.IdSite,
                        DsFrase = item.DsFrase,
                        DsObservacoes = item.DsObservacoes,
                        FlAtivo = item.FlAtivo,
                        IdCliente = item.IdCliente,
                        NmFantasia = item.NmFantasia,
                        NmLogo = item.NmLogo,
                        NmRazaoSocial = item.NmRazaoSocial,
                        NuCNPJ = item.NuCNPJ,
                        Cliente = dal.ClienteGetByID(idCliente)
                    };
                    _entidade.Add(site);
                }
            }
            catch (Exception ex)
            {
                Erros("SiteServico.SiteGetByID", ex);
            }

            return _entidade;
        }

        private Entidade.Site _SiteSave(Entidade.Site site, int idUsuarioLogado)
        {
            Entidade.Site _entidade = new Entidade.Site();
            DAL.Entity.Site lista = new DAL.Entity.Site();
            site.IsInsert = site.IdSite == 0;

            var idPerfilSolicitante = unitOfWork.CtrlUsuarioRepository.Get(x => x.IdUsuario == idUsuarioLogado).FirstOrDefault().IdPerfil;
            if (_validaUsuario.EAdministrador(idPerfilSolicitante))
            {
                var sites = new List<Entidade.Site>();
                sites.Add(site);


                var erros = new List<ValidationResult>();
                erros.AddRange(_validaSite.SiteValido(sites));
                erros.AddRange(_validaModulo.ModuloValido(site.LvModulo));

                if (erros.Count == 0)
                {
                    lista = dal.SiteSave(site);

                    if (site.IdSite == 0)
                        Mensagem = Traducao.Resource.mensagem_registro_incluido;
                    else
                        Mensagem = Traducao.Resource.mensagem_registro_alterado;

                    _entidade = new Entidade.Site()
                    {
                        IdSite = lista.IdSite,
                        DsFrase = lista.DsFrase,
                        DsObservacoes = lista.DsObservacoes,
                        FlAtivo = lista.FlAtivo,
                        IdCliente = lista.IdCliente,
                        NmFantasia = lista.NmFantasia,
                        NmLogo = lista.NmLogo,
                        NmRazaoSocial = lista.NmRazaoSocial,
                        NuCNPJ = lista.NuCNPJ,
                    };

                    _entidade.LvModulo = ObterModulos(_entidade.IdSite);
                    _entidade.LvProcesso = ObterProcessos(_entidade.IdSite);
                }
                else
                {
                    _entidade.ModelErros.AddRange(erros);
                    Sucesso = false;
                }
            }
            else
            {
                _entidade.ModelErros.Add(new ValidationResult("Usuário sem permissão para cadastrar site."));
                Sucesso = false;
            }

            return _entidade;
        }

        public Entidade.Site SiteSave(Entidade.Site site, int idUsuarioLogado)
        {
            Sucesso = true;

            using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    site = _SiteSave(site, idUsuarioLogado);

                    _transaction.Complete();
                    return site;
                }
                catch (DbUpdateException e)
                {
                    Sucesso = false;
                    SqlException innerException = e.InnerException.InnerException as SqlException;
                    if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Site") > 0)
                    {
                        Error = new Exception(Traducao.Resource.Site_PK_Site);
                    }
                    else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_SiteModulo_Site") > 0)
                    {
                        Error = new Exception(Traducao.Resource.Site_FK_SiteModulo_Site);
                    }
                    else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_Cargo_Site") > 0)
                    {
                        Error = new Exception(Traducao.Resource.Site_FK_Cargo_Site);
                    }
                    else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_Processo_Site") > 0)
                    {
                        Error = new Exception(Traducao.Resource.Site_FK_Processo_Site);
                    }
                    else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_UsuarioClienteSite_Site") > 0)
                    {
                        Error = new Exception(Traducao.Resource.Site_FK_UsuarioClienteSite_Site);
                    }
                    else
                    {
                        Erros("SiteServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(e.Message));
                    }
                }
                catch (DbEntityValidationException e)
                {
                    DBErros("SiteServico.SiteSave", e);
                }
                catch (Exception ex)
                {
                    Erros("SiteServico.SiteSave", ex);
                }
            }

            return site;
        }

        public void SiteRemove(int id)
        {
            // valida se o site está inativo
            DAL.Entity.Site Site = dal.SiteGetByID(id);
            if (Site.FlAtivo)
            {
                Sucesso = false;
                Mensagem = Traducao.Resource.Site_msg_erro_excluir_ativo;
            }
            else
            {
                using (TransactionScope _transaction = new TransactionScope(TransactionScopeOption.Required))
                {
                    try
                    {
                        Sucesso = true;
                        dal.SiteRemove(id);

                        Mensagem = Traducao.Resource.mensagem_registro_excluido;

                        _transaction.Complete();
                    }
                    catch (DbUpdateException e)
                    {
                        Sucesso = false;
                        SqlException innerException = e.InnerException.InnerException as SqlException;
                        if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Site") > 0)
                        {
                            Error = new Exception(Traducao.Resource.Site_PK_Site);
                        }
                        else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_SiteModulo_Site") > 0)
                        {
                            Error = new Exception(Traducao.Resource.Site_FK_SiteModulo_Site);
                        }
                        else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_Cargo_Site") > 0)
                        {
                            Error = new Exception(Traducao.Resource.Site_FK_Cargo_Site);
                        }
                        else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_Processo_Site") > 0)
                        {
                            Error = new Exception(Traducao.Resource.Site_FK_Processo_Site);
                        }
                        else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_UsuarioClienteSite_Site") > 0)
                        {
                            Error = new Exception(Traducao.Resource.Site_FK_UsuarioClienteSite_Site);
                        }
                        else
                        {
                            Erros("SiteServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(e.Message));
                        }
                    }
                    catch (DbEntityValidationException e)
                    {
                        DBErros("SiteServico.SiteRemove", e);
                    }
                    catch (Exception ex)
                    {
                        Erros("SiteServico.SiteRemove", ex);
                    }
                }
            }
        }

        public void SiteAtivaInativa(int idSite)
        {
            try
            {
                dal.SiteAtivaInativa(idSite);
            }
            catch (DbUpdateException e)
            {
                Sucesso = false;
                SqlException innerException = e.InnerException.InnerException as SqlException;
                if (innerException != null && (innerException.Number == 2627 || innerException.Number == 2601) && innerException.Message.IndexOf("PK_Site") > 0)
                {
                    Error = new Exception(Traducao.Resource.Site_PK_Site);
                }
                else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_SiteModulo_Site") > 0)
                {
                    Error = new Exception(Traducao.Resource.Site_FK_SiteModulo_Site);
                }
                else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_Cargo_Site") > 0)
                {
                    Error = new Exception(Traducao.Resource.Site_FK_Cargo_Site);
                }
                else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_Processo_Site") > 0)
                {
                    Error = new Exception(Traducao.Resource.Site_FK_Processo_Site);
                }
                else if (innerException != null && innerException.Number == 547 && innerException.Message.IndexOf("FK_UsuarioClienteSite_Site") > 0)
                {
                    Error = new Exception(Traducao.Resource.Site_FK_UsuarioClienteSite_Site);
                }
                else
                {
                    Erros("SiteServico." + GetMethodName(MethodBase.GetCurrentMethod()), new Exception(e.Message));
                }
            }
            catch (DbEntityValidationException e)
            {
                DBErros("SiteServico.SiteRemove", e);
            }
            catch (Exception ex)
            {
                Erros("SiteServico.SiteRemove", ex);
            }
        }

        public List<Entidade.Site> SiteObterTodos()
        {
            var _entidade = new List<Entidade.Site>();

            try
            {
                Sucesso = true;

                var sites = dal.SiteObterTodos();

                foreach (DAL.Entity.Site item in sites)
                {
                    Entidade.Site site = new Entidade.Site()
                    {
                        IdSite = item.IdSite,
                        DsFrase = item.DsFrase,
                        DsObservacoes = item.DsObservacoes,
                        FlAtivo = item.FlAtivo,
                        IdCliente = item.IdCliente,
                        NmFantasia = item.NmFantasia,
                        NmLogo = item.NmLogo,
                        NmRazaoSocial = item.NmRazaoSocial,
                        NuCNPJ = item.NuCNPJ,
                    };
                    _entidade.Add(site);
                }
            }
            catch (Exception ex)
            {
                Erros("SiteServico.SiteGetByID", ex);
            }

            return _entidade;
        }

        public List<Entidade.Site> SiteObterTodosAtivos()
        {
            var _entidade = new List<Entidade.Site>();

            try
            {
                Sucesso = true;

                var sites = dal.SiteObterTodos().Where(x => x.FlAtivo = true);

                foreach (DAL.Entity.Site item in sites)
                {
                    Entidade.Site site = new Entidade.Site()
                    {
                        IdSite = item.IdSite,
                        DsFrase = item.DsFrase,
                        DsObservacoes = item.DsObservacoes,
                        FlAtivo = item.FlAtivo,
                        IdCliente = item.IdCliente,
                        NmFantasia = item.NmFantasia,
                        NmLogo = item.NmLogo,
                        NmRazaoSocial = item.NmRazaoSocial,
                        NuCNPJ = item.NuCNPJ,
                    };
                    _entidade.Add(site);
                }
            }
            catch (Exception ex)
            {
                Erros("SiteServico.SiteGetByID", ex);
            }

            return _entidade;
        }
    }
}
