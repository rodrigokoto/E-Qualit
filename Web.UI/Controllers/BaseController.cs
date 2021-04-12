using ApplicationService.Interface;
using DAL.Context;
using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using Dominio.Servico;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Web.UI.Helpers;
using Web.UI.Models;

namespace Web.UI.Controllers
{
    [EditarPossuiAcessoSite]
    public class BaseController : Controller
    {
        private string controller;
        private string action;
        public string lingua;
        private readonly ILogAppServico _logServico;
        private readonly ISiteRepositorio _siteRepositorio;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;


        public BaseController(ILogAppServico logServico, IUsuarioAppServico usuarioAppServico, IProcessoAppServico processoAppServico, IControladorCategoriasAppServico controladorCategoriasServico)
        {
            _logServico = logServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;


            Usuario usuarioLogadoBase = new Usuario();
            int idUsuario = 0;
            int idSite = 0;
            DateTime LastDay = DateTime.Now.AddDays(-1);

            try
            {
                idUsuario = Util.ObterCodigoUsuarioLogado();
            }
            catch
            {
            }

            try
            {
                idSite = Util.ObterSiteSelecionado();
            }
            catch
            {
            }

            try
            {

                ViewBag.CodClienteSelecionado = Util.ObterClienteSelecionado();
            }
            catch
            {
                ViewBag.CodClienteSelecionado = 0;
            }


            try
            {
                using (var db = new BaseContext())
                {
                    try
                    {

                        if (_usuarioAppServico != null)
                        {
                            usuarioLogadoBase = _usuarioAppServico.GetById(idUsuario);
                        }

                        if (usuarioLogadoBase != null)
                        {

                            if (usuarioLogadoBase.IdPerfil == 1 || usuarioLogadoBase.IdPerfil == 3)
                            {
                                List<int> usuariocargos = usuarioLogadoBase.UsuarioCargoes.Select(x => x.IdCargo).Distinct().ToList();
                                var result = (from pr in db.Processo
                                              join d in db.DocDocumento on pr.IdProcesso equals d.IdProcesso
                                              join dc in db.DocumentoCargo on d.IdDocumento equals dc.IdDocumento
                                              join c in db.ControladorCategoria on d.IdCategoria equals c.IdControladorCategorias
                                              where d.FlStatus == 3 && d.IdSite == idSite
                                              select new MenuProcessoViewModel
                                              {

                                                  IdProcesso = pr.IdProcesso,
                                                  Nome = pr.Nome,
                                                  Descricao = c.Descricao,
                                                  IdCategoria = c.IdControladorCategorias

                                              }).Distinct().ToList().GroupBy(x => x.Nome);

                                ViewData["Menu"] = result;
                                ViewBag.IdPerfil = usuarioLogadoBase.IdPerfil;
                            }

                            else
                            {
                                List<int> usuariocargos = usuarioLogadoBase.UsuarioCargoes.Select(x => x.IdCargo).Distinct().ToList();
                                var result = (from pr in db.Processo
                                              join d in db.DocDocumento on pr.IdProcesso equals d.IdProcesso
                                              join dc in db.DocumentoCargo on d.IdDocumento equals dc.IdDocumento
                                              join u in db.UsuarioCargo on dc.IdCargo equals u.IdCargo
                                              join c in db.ControladorCategoria on d.IdCategoria equals c.IdControladorCategorias
                                              where d.FlStatus == 3
                                              && d.IdSite == idSite
                                              select new MenuProcessoViewModel
                                              {

                                                  IdProcesso = pr.IdProcesso,
                                                  Nome = pr.Nome,
                                                  Descricao = c.Descricao,
                                                  IdCategoria = c.IdControladorCategorias

                                              }).Distinct().ToList().GroupBy(x => x.Nome);

                                ViewData["Menu"] = result;
                                ViewBag.IdPerfil = usuarioLogadoBase.IdPerfil;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }

                ViewBag.Funcionalidades = new List<Funcionalidade>();
                if (_usuarioAppServico != null)
                {
                    usuarioLogadoBase = _usuarioAppServico.GetById(idUsuario);

                    if (usuarioLogadoBase != null)
                    {
                        if (usuarioLogadoBase.IdPerfil == 4)
                        {
                            ViewBag.Funcionalidades = _usuarioAppServico.ObterFuncionalidadesPermitidas(idUsuario).Where(x => x.Ativo == true).ToList();
                        }
                        else
                        {
                            ViewBag.Funcionalidades = _usuarioAppServico.ObterFuncionalidadesPermitidasPorSite(idSite).Where(x => x.Ativo == true).ToList();
                        }
                    }
                }
            }
            catch
            {
                ViewBag.Funcionalidades = new List<Funcionalidade>();
            }

            try
            {
                ViewBag.Processos = new List<Processo>();
                if (_processoAppServico != null)
                    ViewBag.Processos = _processoAppServico.Get(x => x.IdSite == idSite && !x.FlQualidade).ToList();
            }
            catch
            {
                ViewBag.Processos = new List<Processo>();
            }

            try
            {
                ViewBag.Categorias = new List<ControladorCategoria>();
                if (_controladorCategoriasServico != null)
                    ViewBag.Categorias = _controladorCategoriasServico.ListaAtivos("CATDOC", idSite);
            }
            catch
            {
            }

            try
            {
                using (var db = new BaseContext())
                {

                    var idCliente = Util.ObterClienteSelecionado();

                    var DtVencimento = DateTime.Now.AddDays(-1);

                    var IdSiteManuais = Convert.ToInt32(ConfigurationManager.AppSettings["IdSiteManuais"]);
                    var IdCategoria = Convert.ToInt32(ConfigurationManager.AppSettings["IdCategoriaManuais"]);

                    var DocManuaisQuery = (from doc in db.DocDocumento
                                           join proc in db.Processo on doc.IdProcesso equals proc.IdProcesso
                                           where doc.IdSite == IdSiteManuais && doc.IdCategoria == IdCategoria && doc.FlStatus == 3
                                           select new ManuaisViewModel
                                           {
                                               IdDocumento = doc.IdDocumento,
                                               Processo = proc.Nome,
                                               Titulo = doc.Titulo
                                           }).ToList().GroupBy(x => x.Processo);

                    ViewBag.Manuais = DocManuaisQuery;


                    if (idSite != 0)
                    {
                        var licenca = (from lc in db.Licenca
                                       where lc.DataVencimento.Value < DtVencimento && lc.Idcliente == idCliente && lc.Idcliente == idCliente

                                       select new PendenciaViewModel
                                       {
                                           Id = lc.IdLicenca,
                                           Titulo = lc.Titulo,
                                           IdResponsavel = lc.IdResponsavel,
                                           Modulo = "Licenca",
                                           Url = "Licenca/Editar/" + lc.IdLicenca


                                       });

                        var indicadores = (from ind in db.Indicador
                                           join per in db.PeriodicidaDeAnalise on ind.Id equals per.IdIndicador
                                           join meta in db.PlanoVoo on per.Id equals meta.IdPeriodicidadeAnalise
                                           where meta.DataReferencia < DateTime.Now && meta.Realizado == null && ind.IdSite == idSite

                                           select new PendenciaViewModel
                                           {
                                               Id = ind.Id,
                                               Titulo = ind.Objetivo,
                                               IdResponsavel = ind.IdResponsavel,
                                               Modulo = "Indicador",
                                               Url = "Indicador/Editar/" + ind.Id
                                           });
                        var docDocumentoAprov = (from doc in db.DocDocumento
                                                 where doc.StatusRegistro == (int)StatusDocumento.Aprovacao && doc.IdSite == idSite
                                                 select doc);

                        var docDocumentoVer = (from doc in db.DocDocumento
                                               where doc.StatusRegistro == (int)StatusDocumento.Verificacao && doc.IdSite == idSite
                                               select doc);

                        var doDocumentoRev = (from doc in db.DocDocumento
                                              where doc.FlRevisaoPeriodica == true && doc.DtNotificacao < DateTime.Now && doc.IdSite == idSite
                                              select doc);




                        var naoConformidade = (from nc in db.RegistroConformidade
                                               where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.IdSite == idSite && nc.TipoRegistro == "nc"
                                               select new PendenciaViewModel
                                               {
                                                   Id = (int)nc.IdRegistroConformidade,
                                                   Titulo = nc.NuRegistro.ToString(),
                                                   IdResponsavel = nc.ResponsavelInicarAcaoImediata.IdUsuario,
                                                   Modulo = "NC",
                                                   Url = "NaoConformidade/Editar/" + (int)nc.IdRegistroConformidade
                                               }); ;

                        var naoConformidadePrazo = (from nc in db.RegistroConformidade
                                                    join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                                    where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && nc.IdSite == idSite && nc.TipoRegistro == "nc"
                                                    select new PendenciaViewModel
                                                    {
                                                        Id = (int)nc.IdRegistroConformidade,
                                                        Titulo = nc.NuRegistro.ToString(),
                                                        IdResponsavel = ai.ResponsavelImplementar.IdUsuario,
                                                        Modulo = "NC",
                                                        Url = "NaoConformidade/Editar/" + (int)nc.IdRegistroConformidade
                                                    });

                        var naoConformidadeReverificacao = (from nc in db.RegistroConformidade
                                                            where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "nc" && nc.IdSite == idSite
                                                            select new PendenciaViewModel
                                                            {
                                                                Id = (int)nc.IdRegistroConformidade,
                                                                Titulo = nc.NuRegistro.ToString(),
                                                                IdResponsavel = nc.ResponsavelReverificador.IdUsuario,
                                                                Modulo = "NC",
                                                                Url = "NaoConformidade/Editar/" + (int)nc.IdRegistroConformidade
                                                            });

                        var acaoCorretiva = (from ac in db.RegistroConformidade
                                             join ai in db.AcaoImediata on ac.IdRegistroConformidade equals ai.IdRegistroConformidade
                                             where ac.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && ac.IdSite == idSite && ac.TipoRegistro == "ac"
                                             select new PendenciaViewModel
                                             {
                                                 Id = ac.IdRegistroConformidade,
                                                 Titulo = ac.NuRegistro.ToString(),
                                                 IdResponsavel = ai.ResponsavelImplementar.IdUsuario,
                                                 Modulo = "AC",
                                                 Url = "AcaoCorretiva/Editar/" + ac.IdRegistroConformidade

                                             });

                        var acaoCorretivaRev = (from ac in db.RegistroConformidade
                                                where ac.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && ac.TipoRegistro == "ac" && ac.DtPrazoImplementacao < DateTime.Now && ac.IdSite == idSite
                                                select new PendenciaViewModel
                                                {
                                                    Id = ac.IdRegistroConformidade,
                                                    Titulo = ac.NuRegistro.ToString(),
                                                    IdResponsavel = ac.ResponsavelImplementar.IdUsuario,
                                                    Modulo = "AC",
                                                    Url = "AcaoCorretiva/Editar/" + ac.IdRegistroConformidade
                                                });

                        var Auditoria = (from plai in db.Plai
                                         where plai.DataReuniaoAbertura.Day <= DateTime.Now.AddDays(-15).Day && plai.Pai.IdSite == idSite
                                         select new PendenciaViewModel
                                         {
                                             Id = plai.IdPlai,
                                             Titulo = "Auditoria",
                                             IdResponsavel = plai.IdElaborador,
                                             Modulo = "Auditoria",
                                             Url = "Auditoria/Editar/" + plai.IdPlai
                                         });

                        var AnaliseCritica = (from anc in db.AnaliseCritica
                                              where anc.IdSite == idSite && anc.DataProximaAnalise == LastDay
                                              select new PendenciaViewModel
                                              {
                                                  Id = anc.IdAnaliseCritica,
                                                  Titulo = anc.Ata,
                                                  IdResponsavel = anc.IdResponsavel,
                                                  Modulo = "AnaliseCritica",
                                                  Url = "AnaliseCritica/Editar/" + anc.IdAnaliseCritica
                                              }).ToList();

                        var InstrumentoQuery = (from ins in db.Instrumento
                                                join cal in db.Calibracao on ins.IdInstrumento equals cal.IdInstrumento
                                                where ins.IdSite == idSite && ins.Status == (byte)EquipamentoStatus.NaoCalibrado
                                                group ins by ins.IdInstrumento into idIsntrumentoOrder
                                                select idIsntrumentoOrder).ToList(); 

                        var Instrumento = new List<PendenciaViewModel>();
                        foreach (var item in InstrumentoQuery)
                        {
                            var a = item.FirstOrDefault();

                            Instrumento.Add(new PendenciaViewModel()
                            {
                                Id = a.IdInstrumento,
                                Titulo = a.Equipamento,
                                IdResponsavel = (int)a.IdResponsavel,
                                Modulo = "Instrumento",
                                Url = "Instrumento/Editar/" + a.IdInstrumento
                            });

                        }

                        var Fornecedores = (from forn in db.Fornecedor
                                            join avaq in db.AvaliaCriterioQualificacao on forn.IdFornecedor equals avaq.IdFornecedor
                                            join prodf in db.ProdutoFornecedor on forn.IdFornecedor equals prodf.IdFornecedor
                                            where avaq.IdResponsavelPorControlarVencimento != null && avaq.DtVencimento != null && avaq.DtVencimento <= DateTime.Now && forn.IdSite == idSite
                                            select new PendenciaViewModel
                                            {
                                                Id = forn.IdFornecedor,
                                                Titulo = forn.Nome,
                                                IdResponsavel = avaq.IdResponsavelPorQualificar,
                                                Modulo = "Fornecedor",
                                                Url = "fornecedor/acoesfornecedores/" + forn.IdFornecedor + "?idProduto=" + prodf.IdProduto + "&Ancora=Qualificar"
                                            });

                        var FornecedoresVal = (from forn in db.Fornecedor
                                               join avaa in db.AvaliaCriterioAvaliacao on forn.IdFornecedor equals avaa.IdFornecedor
                                               join prodf in db.ProdutoFornecedor on forn.IdFornecedor equals prodf.IdFornecedor
                                               where avaa.DtProximaAvaliacao == DateTime.Now && avaa.IdUsuarioAvaliacao != null
                                               select new PendenciaViewModel
                                               {
                                                   Id = forn.IdFornecedor,
                                                   Titulo = forn.Nome,
                                                   IdResponsavel = (int)avaa.IdUsuarioAvaliacao,
                                                   Modulo = "Fornecedor",
                                                   Url = "fornecedor/acoesfornecedores/" + forn.IdFornecedor + "?idProduto=" + prodf.IdProduto + "&Ancora=Avaliar"
                                               });

                        var gestaoRisco = (from nc in db.RegistroConformidade
                                           where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.IdSite == idSite && nc.TipoRegistro == "gr"
                                           select new PendenciaViewModel
                                           {
                                               Id = (int)nc.IdRegistroConformidade,
                                               Titulo = nc.NuRegistro.ToString(),
                                               IdResponsavel = nc.ResponsavelInicarAcaoImediata.IdUsuario,
                                               Modulo = "GR",
                                               Url = "GestaoDeRisco/Editar/" + (int)nc.IdRegistroConformidade
                                           });

                        var gestaoRiscoPrazo = (from nc in db.RegistroConformidade
                                                join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                                where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && nc.IdSite == idSite && nc.TipoRegistro == "gr"
                                                select new PendenciaViewModel
                                                {
                                                    Id = (int)nc.IdRegistroConformidade,
                                                    Titulo = nc.NuRegistro.ToString(),
                                                    IdResponsavel = ai.ResponsavelImplementar.IdUsuario,
                                                    Modulo = "GR",
                                                    Url = "GestaoDeRisco/Editar/" + (int)nc.IdRegistroConformidade
                                                });

                        var gestaoRiscoReverificacao = (from nc in db.RegistroConformidade
                                                        where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "gr" && nc.IdSite == idSite
                                                        select new PendenciaViewModel
                                                        {
                                                            Id = (int)nc.IdRegistroConformidade,
                                                            Titulo = nc.NuRegistro.ToString(),
                                                            IdResponsavel = nc.ResponsavelReverificador.IdUsuario,
                                                            Modulo = "GR",
                                                            Url = "GestaoDeRisco/Editar/" + (int)nc.IdRegistroConformidade,
                                                        });



                        var gestaoMelhoria = (from nc in db.RegistroConformidade
                                              where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.IdSite == idSite && nc.TipoRegistro == "gm"
                                              select new PendenciaViewModel
                                              {
                                                  Id = (int)nc.IdRegistroConformidade,
                                                  Titulo = nc.NuRegistro.ToString(),
                                                  IdResponsavel = nc.ResponsavelInicarAcaoImediata.IdUsuario,
                                                  Modulo = "GM",
                                                  Url = "GestaoMelhoria/Editar/" + (int)nc.IdRegistroConformidade
                                              });

                        var gestaoMelhoriaPrazo = (from nc in db.RegistroConformidade
                                                   join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                                   where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && nc.IdSite == idSite && nc.TipoRegistro == "gm"
                                                   select new PendenciaViewModel
                                                   {
                                                       Id = (int)nc.IdRegistroConformidade,
                                                       Titulo = nc.NuRegistro.ToString(),
                                                       IdResponsavel = ai.ResponsavelImplementar.IdUsuario,
                                                       Modulo = "GM",
                                                       Url = "GestaoMelhoria/Editar/" + (int)nc.IdRegistroConformidade
                                                   });

                        var gestaoMelhoriaReverificacao = (from nc in db.RegistroConformidade
                                                           where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "gm" && nc.IdSite == idSite
                                                           select new PendenciaViewModel
                                                           {
                                                               Id = (int)nc.IdRegistroConformidade,
                                                               Titulo = nc.NuRegistro.ToString(),
                                                               IdResponsavel = nc.ResponsavelImplementar.IdUsuario,
                                                               Modulo = "GM",
                                                               Url = "GestaoMelhoria/Editar/" + (int)nc.IdRegistroConformidade
                                                           });


                        List<PendenciaViewModel> docPendencia = new List<PendenciaViewModel>();

                        foreach (var docap in docDocumentoAprov.ToList())
                        {
                            if (docap.DocUsuarioVerificaAprova != null)
                            {
                                var docResult = docap.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "A").OrderBy(x => x.Usuario.NmCompleto).ToList();

                                foreach (var res in docResult)
                                {
                                    PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                                    {
                                        Id = docap.IdDocumento,
                                        Titulo = docap.Titulo,
                                        IdResponsavel = res.IdUsuario,
                                        Modulo = "ControlDoc",
                                        Url = "ControlDoc/Editar/" + docap.IdDocumento
                                    };

                                    docPendencia.Add(pendenciaViewModel);
                                }
                            }
                        }

                        foreach (var docver in docDocumentoVer.ToList())
                        {
                            if (docver.DocUsuarioVerificaAprova != null)
                            {
                                var docResult = docver.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "V").OrderBy(x => x.Usuario.NmCompleto).ToList();

                                foreach (var res in docResult)
                                {
                                    PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                                    {
                                        Id = docver.IdDocumento,
                                        Titulo = docver.Titulo,
                                        IdResponsavel = res.IdUsuario,
                                        Modulo = "ControlDoc",
                                        Url = "ControlDoc/Editar/" + docver.IdDocumento
                                    };

                                    docPendencia.Add(pendenciaViewModel);
                                }
                            }
                        }

                        foreach (var docrev in doDocumentoRev.ToList())
                        {
                            PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                            {
                                Id = docrev.IdDocumento,
                                Titulo = docrev.Titulo,
                                IdResponsavel = docrev.IdElaborador,
                                Modulo = "ControlDoc",
                                Url = "ControlDoc/Editar/" + docrev.IdDocumento
                            };

                            docPendencia.Add(pendenciaViewModel);
                        }


                        Dictionary<int, bool> queryIndicadores = new Dictionary<int, bool>();

                        var indicadores1 = (from ind in db.Indicador
                                            where ind.IdSite == idSite
                                            select ind).ToList();
                        List<PendenciaViewModel> lstPendencia = new List<PendenciaViewModel>();

                        foreach (var indicador in indicadores1)
                        {
                            foreach (var periodo in indicador.PeriodicidadeDeAnalises)
                            {
                                var query = periodo.MetasRealizadas.Where(x => x.DataReferencia < DateTime.Now && x.Realizado == null).ToList();

                                if (query.Count > 0)
                                {
                                    foreach (var plano in query)
                                    {
                                        var mes = plano.DataReferencia.Date.Month;

                                        switch (indicador.PeriodicidadeMedicao)
                                        {
                                            case 1:
                                                if ((mes % 1) == 0)
                                                {
                                                    if (mes != DateTime.Now.Month)
                                                    {
                                                        PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                                                        {
                                                            Id = indicador.Id,
                                                            IdResponsavel = indicador.IdResponsavel,
                                                            Titulo = indicador.Objetivo,
                                                            Modulo = "Indicador",
                                                            Url = "Indicador/Editar/" + indicador.Id
                                                        };

                                                        lstPendencia.Add(pendenciaViewModel);
                                                    }
                                                }
                                                break;
                                            case 2:
                                                if ((mes % 2) == 0)
                                                {
                                                    if (mes != DateTime.Now.Month)
                                                    {
                                                        PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                                                        {
                                                            Id = indicador.Id,
                                                            IdResponsavel = indicador.IdResponsavel,
                                                            Titulo = indicador.Objetivo,
                                                            Modulo = "Indicador",
                                                            Url = "Indicador/Editar/" + indicador.Id
                                                        };

                                                        lstPendencia.Add(pendenciaViewModel);
                                                    }
                                                }
                                                break;
                                            case 3:
                                                if ((mes % 3) == 0)
                                                {
                                                    if (mes != DateTime.Now.Month)
                                                    {
                                                        PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                                                        {
                                                            Id = indicador.Id,
                                                            IdResponsavel = indicador.IdResponsavel,
                                                            Titulo = indicador.Objetivo,
                                                            Modulo = "Indicador",
                                                            Url = "Indicador/Editar/" + indicador.Id
                                                        };

                                                        lstPendencia.Add(pendenciaViewModel);
                                                    }
                                                }
                                                break;
                                            case 4:
                                                if ((mes % 6) == 0)
                                                {
                                                    if (mes != DateTime.Now.Month)
                                                    {
                                                        PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                                                        {
                                                            Id = indicador.Id,
                                                            IdResponsavel = indicador.IdResponsavel,
                                                            Titulo = indicador.Objetivo,
                                                            Modulo = "Indicador",
                                                            Url = "Indicador/Editar/" + indicador.Id
                                                        };

                                                        lstPendencia.Add(pendenciaViewModel);
                                                    }
                                                }
                                                break;
                                            case 5:
                                                if ((mes % 12) == 0)
                                                {
                                                    if (mes != DateTime.Now.Month)
                                                    {
                                                        PendenciaViewModel pendenciaViewModel = new PendenciaViewModel()
                                                        {
                                                            Id = indicador.Id,
                                                            IdResponsavel = indicador.IdResponsavel,
                                                            Titulo = indicador.Objetivo,
                                                            Modulo = "Indicador",
                                                            Url = "Indicador/Editar/" + indicador.Id
                                                        };

                                                        lstPendencia.Add(pendenciaViewModel);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                            }
                        }

                        lstPendencia.AddRange(licenca.ToList());
                        lstPendencia.AddRange(naoConformidade.ToList());
                        lstPendencia.AddRange(naoConformidadePrazo.ToList());
                        lstPendencia.AddRange(naoConformidadeReverificacao.ToList());
                        lstPendencia.AddRange(acaoCorretiva.ToList());
                        lstPendencia.AddRange(acaoCorretivaRev.ToList());
                        lstPendencia.AddRange(Instrumento.ToList());
                        lstPendencia.AddRange(Fornecedores.ToList());
                        lstPendencia.AddRange(AnaliseCritica);
                        lstPendencia.AddRange(gestaoRisco.ToList());
                        lstPendencia.AddRange(gestaoRiscoPrazo.ToList());
                        lstPendencia.AddRange(gestaoRiscoReverificacao.ToList());
                        lstPendencia.AddRange(gestaoMelhoria.ToList());
                        lstPendencia.AddRange(gestaoMelhoriaPrazo.ToList());
                        lstPendencia.AddRange(gestaoMelhoriaReverificacao.ToList());
                        lstPendencia.AddRange(docPendencia);

                        lstPendencia = lstPendencia.GroupBy(x => x.Id).Select(j => new PendenciaViewModel()
                        {
                            Id = j.First().Id,
                            IdResponsavel = j.First().IdResponsavel,
                            Modulo = j.First().Modulo,
                            Titulo = j.First().Titulo,
                            Url = j.First().Url
                        }).ToList();

                        if (usuarioLogadoBase.IdPerfil == 1 || usuarioLogadoBase.IdPerfil == 2 || usuarioLogadoBase.IdPerfil == 3)
                        {
                            ViewBag.Pendencia = lstPendencia;
                        }
                        else
                        {
                            ViewBag.Pendencia = lstPendencia.Where(x => x.IdResponsavel == usuarioLogadoBase.IdUsuario).ToList();
                        }
                    }
                    else
                    {
                        ViewBag.Pendencia = new List<PendenciaViewModel>();
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ModelState.Clear();


            controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            action = filterContext.ActionDescriptor.ActionName;

            if (Session["siteFrase"] != null)
            {
                ViewBag.SiteFrase = Session["siteFrase"];
            }
            else
            {
                ViewBag.SiteFrase = Traducao.Resource.SiteMensagemDefault;
            }

            string parameter = string.Empty;
            if (filterContext.ActionParameters.ContainsKey("rotaDoCliente"))
            {
                //var parametro = filterContext.ActionParameters.Values.Any(x=>x != string.Empty);
                //if (parametro)
                //{

                //    //logout
                //    filterContext.Result = new LoginController(_logServico).Logout();
                //    //redirecti para login
                //    base.OnActionExecuting(filterContext);
                //}

            }

            ViewBag.Action = action;
            ViewBag.Controller = controller;

            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();

            ViewBag.UsuarioLogado = Util.ObterUsuario();
            //ViewBag.ClienteSelecionado = Util.ObterClienteSelecionado();

            ViewBag.Permissoes = Util.ObterPermissoes();
            //ViewBag.ProcessoSelecionado = Util.ObterProcessoSelecionado();

            try
            {
                int idCliente = Util.ObterClienteSelecionado();

                ViewBag.QuantidadeSites = 0;
                if (_siteRepositorio != null)
                    ViewBag.QuantidadeSites = _siteRepositorio.ListarSitesPorCliente(idCliente);
            }
            catch
            {
                ViewBag.QuantidadeSites = 0;
            }


            #region Parametros de Cultura - Lingua corrente

            lingua = Web.UI.Helpers.Cultura.GetCultura();
            ViewBag.Lingua = lingua;

            #endregion

            base.OnActionExecuting(filterContext);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {

            bool hasAuthorizeAttribute = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthorizeAttribute), false).Any();

            base.OnAuthorization(filterContext);
        }

        public void GeraArquivoZip(ControllerContext context,
                                     string actionName,
                                     int idObjeto)
        {
            string nomePdfGerado = string.Empty;
            string times = DateTime.Now.Ticks.ToString();

            string diretorio = context.HttpContext.Server.MapPath(@"~\Content\temp\" + times);

            Util.VerificaDiretorio(diretorio);


            // string footer = "--footer-left \"Pagina: [page] de [toPage]\" --footer-right \"Data: [date] [time]\" --footer-center \" --footer-line --footer-font-size \"9\" --footer-spacing 5";
            nomePdfGerado = "apenasUmTeste.pdf";

            string fullPath = Path.Combine(diretorio, nomePdfGerado);

            var byteArray = new byte[0];
            //var byteArray = pdfRotativa.BuildPdf(context);


            var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            fileStream.Write(byteArray, 0, byteArray.Length);
            fileStream.Close();
            fileStream.Dispose();
        }

        public void GravaLog(Exception ex)
        {

            var log = new Log(Util.ObterCodigoUsuarioLogado(),
                              Convert.ToInt32(Acao.Login),
                              Util.GetIp(HttpContext),
                              Util.GetBrowser(HttpContext),
                              ex);


            _logServico.Add(log);
        }

        public void EscolheProcesso(string idProcesso, string nomeProcesso)
        {
            try
            {
                idProcesso = UtilsServico.CriptografarString(idProcesso);

                var cookieCodigo = new HttpCookie("processoSelecionadoCodigo", idProcesso)
                {
                    Expires = DateTime.Now.AddDays(1)
                };

                var cookieNome = new HttpCookie("processoSelecionadoNome", nomeProcesso)
                {
                    Expires = DateTime.Now.AddDays(1)
                };

                Response.Cookies.Add(cookieCodigo);
                Response.Cookies.Add(cookieNome);

            }
            catch (Exception ex)
            {
                GravaLog(ex);
            }
        }

        protected void RemoveTodosCookies()
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1); // make it expire yesterday
                Response.Cookies.Add(aCookie); // overwrite it
            }
        }
        protected string RetornaApenasNumeros(string str) =>
             Regex.Replace(str, @"[^\d]", "");

        protected byte[] TransformaString64EmBase64(string strB64) =>
            strB64 == null ? Encoding.ASCII.GetBytes("") : Convert.FromBase64String(strB64);

        protected string RetornaExtensao(string nomeArquivo) =>
            nomeArquivo != null ? nomeArquivo.Substring(nomeArquivo.LastIndexOf(".")) : "";


    }

}
