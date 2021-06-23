using System;
using System.Linq;
using System.Web.Mvc;
using Dominio.Enumerado;
using ApplicationService.Interface;
using Web.UI.Helpers;
using Dominio.Entidade;
using System.Collections.Generic;
using Highsoft.Web.Mvc.Charts;
using System.Globalization;
using DAL.Context;
using Web.UI.Models;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class RelatorioController : BaseController
    {
        private readonly ILogAppServico _logAppServico;
        private readonly IRegistroConformidadesAppServico _naoConformidade;
        private readonly ICargoProcessoAppServico _cargoProcessoAppServico;
        private readonly IUsuarioCargoAppServico _usuarioCargoAppServico;
        private readonly ICargoAppServico _cargoAppServico;
        private readonly IRelatorioAppServico _relatorioAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly ISiteModuloAppServico _siteModuloAppServico;
        private readonly IRegistroConformidadesAppServico _registroConformidadesAppServico;
        private readonly IDocDocumentoAppServico _docDocumentoAppServico;
        private readonly IDocCargoAppServico _docCargoAppServico;



        public RelatorioController(ILogAppServico logAppServico,
                                   IRegistroConformidadesAppServico naoConformidade,
                                   ISiteModuloAppServico siteModuloAppServico,
                                   ICargoProcessoAppServico cargoProcessoAppServico,
                                   IUsuarioCargoAppServico usuarioCargoAppServico,
                                   ICargoAppServico cargoAppServico,
                                   IUsuarioAppServico usuarioAppServico,
                                   IProcessoAppServico processoAppServico,
                                   IPendenciaAppServico pendenciaAppServico,
                                   IControladorCategoriasAppServico controladorCategoriasServico,
                                   IRelatorioAppServico relatorioAppServico,
                                   IRegistroConformidadesAppServico registroConformidadesAppServico,
                                   IDocDocumentoAppServico docDocumentoAppServico,
                                   IDocCargoAppServico docCargoAppServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico, pendenciaAppServico)
        {
            _logAppServico = logAppServico;
            _naoConformidade = naoConformidade;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _relatorioAppServico = relatorioAppServico;
            _registroConformidadesAppServico = registroConformidadesAppServico;
            _cargoProcessoAppServico = cargoProcessoAppServico;
            _cargoAppServico = cargoAppServico;
            _siteModuloAppServico = siteModuloAppServico;
            _usuarioCargoAppServico = usuarioCargoAppServico;
            _docDocumentoAppServico = docDocumentoAppServico;
            _docCargoAppServico = docCargoAppServico;
        }

        public ActionResult DashBoard()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            return View();
        }

        public JsonResult NaoConformidade()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            try
            {
                var naoConformidades = _naoConformidade.GetAll();

                var naoConformidade = new
                {
                    Total = naoConformidades.Count(),
                    Encerrada = naoConformidades.Where(x => x.StatusEtapa == (byte)EtapasRegistroConformidade.Encerrada).Count(),
                    Aberta = naoConformidades.Where(x => x.StatusEtapa != (byte)EtapasRegistroConformidade.Encerrada).Count()
                };

                return Json(new { StatusCode = 200, Relatorio = naoConformidade }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GravaLog(ex);

                return Json(new { StatusCode = 500, Erro = ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Index(int idRelatorio)
        {
            var relatorio = _relatorioAppServico.GetById(idRelatorio);
            var idSite = Util.ObterSiteSelecionado();
            relatorio = GetParameters(relatorio);
            return View(relatorio);
        }

        public Relatorio GetParameters(Relatorio relatorio)
        {

            switch (relatorio.Url)
            {
                case "Abertas":
                    relatorio.Parametros = new System.Collections.Generic.Dictionary<string, string>();
                    relatorio.Parametros.Add("Mês", "");
                    break;
                case "AbertasAno":
                    relatorio.Parametros = new System.Collections.Generic.Dictionary<string, string>();
                    relatorio.Parametros.Add("Ano", "");
                    break;
                case "Fechadas":
                    relatorio.Parametros = new System.Collections.Generic.Dictionary<string, string>();
                    relatorio.Parametros.Add("Mês", "");
                    break;
                case "FechadasAno":
                    relatorio.Parametros = new System.Collections.Generic.Dictionary<string, string>();
                    relatorio.Parametros.Add("Ano", "");
                    break;
                case "UsuarioCargo":
                    relatorio.Parametros = new System.Collections.Generic.Dictionary<string, string>();
                    relatorio.Parametros.Add("Modulo", "");
                    relatorio.Parametros.Add("Cargo", "");
                    break;
                default:
                    break;
            }

            return relatorio;
        }

        #region Filtros


        #region Usuario
        public ViewResult UsuarioCargoPermissoesFiltro(int idRelatorio)
        {
            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);
            var modulos = _siteModuloAppServico.ObterPorSite(idSite);


            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "UsuarioCargoPermissoesFiltro");
            relatorio.Parametros.Add("Modulo", "");
            relatorio.Parametros.Add("Cargo", "");

            var cargo = _cargoAppServico.ObtemCargosPorSite(idSite);
            var modulo = _processoAppServico.Get(x => x.IdSite == idSite);

            List<Funcionalidade> funcionalidades = new List<Funcionalidade>();

            modulos.ForEach(x =>
            {
                x.Funcionalidade.Funcoes = x.Funcionalidade.Funcoes.OrderBy(s => s.NuOrdem).ToList();
            });

            modulos.ForEach(x =>
            {
                if (!funcionalidades.Select(y => y.IdFuncionalidade).Contains(x.IdFuncionalidade))
                {
                    funcionalidades.Add(x.Funcionalidade);
                }
            });

            ViewBag.cargo = cargo;
            ViewBag.modulos = funcionalidades;


            return View("Index", relatorio);
        }
        public ViewResult UsuarioCargoUsuarioFiltro(int idRelatorio)
        {
            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);


            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "UsuarioCargoPermissoesFiltro");
            relatorio.Parametros.Add("Cargo", "");

            var cargo = _cargoAppServico.ObtemCargosPorSite(idSite);

            ViewBag.cargo = cargo;


            return View("Index", relatorio);
        }

        #endregion
        #region Documentos
        public ViewResult CargosDocumentosFiltro(int idRelatorio)
        {

            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);


            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "UsuarioCargoPermissoesFiltro");
            relatorio.Parametros.Add("Cargo", "");

            var cargo = _cargoAppServico.ObtemCargosPorSite(idSite);

            ViewBag.cargo = cargo;

            return View("Index", relatorio);
        }
        public ViewResult DocumentoAprovacaoFiltro(int idRelatorio)
        {
            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);

            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "DocumentoAprovacaoFiltro");


            return View("Index", relatorio);
        }
        public ViewResult DocumentoVerificacaoFiltro(int idRelatorio)
        {
            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);

            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "DocumentoAprovacaoFiltro");


            return View("Index", relatorio);

        }
        public ViewResult DocumentoElaboracaoFiltro(int idRelatorio)
        {
            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);

            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "DocumentoAprovacaoFiltro");


            return View("Index", relatorio);


        }
        public ViewResult DocumentoVencimentoFiltro(int idRelatorio)
        {
            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);

            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "DocumentoAprovacaoFiltro");


            return View("Index", relatorio);
        }
        public ViewResult DocumentoObsoletoFiltro(int idRelatorio)
        {
            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);

            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "DocumentoObsoletoFiltro");

            return View("Index", relatorio);
        }
        #endregion

        #region Registros
        public ViewResult GeradasTipoFiltro(int idRelatorio)
        {

            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);

            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "GeradasTipoFiltro");
            relatorio.Parametros.Add("Tipo", "");

            string Tipo = string.Empty;
            string tpRegistro = string.Empty;

            switch (relatorio.Funcionalidade.Url)
            {
                case "GestaoMelhoria":
                    Tipo = "tgm";
                    break;
                default:
                    Tipo = "tnc";
                    break;
            }

            var listaAtivos = _controladorCategoriasServico.ListaAtivos(Tipo, idSite);

            if (Tipo == "tnc")
            {
                if (!listaAtivos.Select(x => x.Descricao).Contains("Auditoria"))
                {
                    _controladorCategoriasServico.Add(new ControladorCategoria
                    {
                        IdSite = idSite,
                        Descricao = "Auditoria",
                        TipoTabela = Tipo,
                        Ativo = true
                    });

                    listaAtivos = _controladorCategoriasServico.ListaAtivos(Tipo, idSite);
                }
            }

            ViewBag.Tipo = listaAtivos;

            return View("Index", relatorio);
        }
        public ViewResult AbertasPendentesFiltro(int idRelatorio)
        {

            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);


            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "AbertasPendentesFiltro");


            return View("Index", relatorio);
        }
        public ViewResult AbertaReverificacaoAtrasoFiltro(int idRelatorio)
        {

            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);


            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "AbertaReverificacaoAtrasoFiltro");

            string tpRegistro = string.Empty;


            return View("Index", relatorio);
        }
        public ViewResult AbertasEncerradasFiltro(int idRelatorio)
        {

            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);


            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "AbertasEncerradasFiltro");

            string tpRegistro = string.Empty;


            return View("Index", relatorio);
        }
        public ViewResult TempoTratamentoRegistroFiltro(int idRelatorio)
        {

            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);


            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "TempoTratamentoRegistroFiltro");

            string tpRegistro = string.Empty;



            return View("Index", relatorio);
        }
        public ViewResult TempoAberturaAnaliseDefinicaoFiltro(int idRelatorio)
        {

            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);


            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "TempoAberturaAnaliseDefinicaoFiltro");
            relatorio.Parametros.Add("Tipo", "");




            return View("Index", relatorio);
        }
        public ViewResult GeradaProcessoFiltro(int idRelatorio)
        {

            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);


            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "GeradaProcessoFiltro");
            relatorio.Parametros.Add("Processo", "");

            string tpRegistro = string.Empty;
            var lista = _processoAppServico.ListaProcessosPorSite(idSite).Select(x => new { x.IdProcesso, x.Nome });

            ViewBag.Processo = lista;

            return View("Index", relatorio);
        }
        public ViewResult AnuladasFiltro(int idRelatorio)
        {

            var idSite = Util.ObterSiteSelecionado();
            var relatorio = _relatorioAppServico.GetById(idRelatorio);


            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "AnuladasFiltro");


            return View("Index", relatorio);
        }
        #endregion

        public ViewResult AbertasFiltro(int idRelatorio)
        {

            var relatorio = _relatorioAppServico.GetById(idRelatorio);

            relatorio.Parametros = new Dictionary<string, string>();
            relatorio.Parametros.Add("Filtro", "AbertasFiltro");
            relatorio.Parametros.Add("Mês", "");

            return View("Filtros/AbertasFiltro", relatorio);
        }
        #endregion

        #region Relatorios
        #region Usuário
        public ViewResult UsuarioCargoPermissoes(Relatorio relatorio)
        {

            var idSite = Util.ObterSiteSelecionado();
            var idCargo = Convert.ToInt32(relatorio.Parametros["Cargo"]);
            var idModulo = Convert.ToInt32(relatorio.Parametros["Modulo"]);

            var modulos = _siteModuloAppServico.ObterPorSite(idSite).Where(x => x.IdFuncionalidade == idModulo).FirstOrDefault();

            var cargo = _cargoAppServico.GetById(idCargo);

            ViewBag.Relatorio = relatorio;
            ViewBag.cargo = cargo;
            ViewBag.modulos = modulos;
            //ViewBag.Funcionalidades = funcionalidades.Where(x => x.Ativo).ToList();


            return View(cargo);
        }
        public ViewResult UsuarioCargoUsuario(Relatorio relatorio)
        {

            var idSite = Util.ObterSiteSelecionado();
            var idCargo = Convert.ToInt32(relatorio.Parametros["Cargo"]);

            var cargo = _cargoAppServico.GetById(idCargo);

            var cargoUsuario = _usuarioCargoAppServico.Get(x => x.IdCargo == idCargo).ToList();

            List<Usuario> usuarios = new List<Usuario>();

            foreach (var usuarioCargo in cargoUsuario)
            {
                usuarios.Add(usuarioCargo.Usuario);
            }

            ViewBag.Relatorio = relatorio;
            ViewBag.cargo = cargo;

            return View(usuarios);
        }
        #endregion

        #region ControlDoc
        public ViewResult CargosDocumentos(Relatorio relatorio)
        {

            var idSite = Util.ObterSiteSelecionado();
            var idCargo = Convert.ToInt32(relatorio.Parametros["Cargo"]);

            var cargo = _cargoAppServico.GetById(idCargo);

            var DocumentoCargo = _docCargoAppServico.Get(x => x.IdCargo == idCargo);

            List<DocDocumento> docDocumento = new List<DocDocumento>();

            foreach (var doc in DocumentoCargo)
            {
                docDocumento.Add(doc.DocDocumento);
            }

            ViewBag.Relatorio = relatorio;
            ViewBag.cargo = cargo;

            return View("RelatorioDocumentos", docDocumento);
        }
        public ViewResult DocumentoObsoleto(Relatorio relatorio)
        {

            var idSite = Util.ObterSiteSelecionado();

            var docDocumento = _docDocumentoAppServico.Get(x => x.IdSite == idSite && x.FlStatus == (int)StatusDocumento.Obsoleto).ToList();

            ViewBag.Relatorio = relatorio;


            return View("RelatorioDocumentos", docDocumento);
        }
        public ViewResult DocumentoAprovacao(Relatorio relatorio)
        {

            var idSite = Util.ObterSiteSelecionado();

            var docDocumento = _docDocumentoAppServico.Get(x => x.IdSite == idSite && x.FlStatus == (int)StatusDocumento.Aprovacao).ToList();

            ViewBag.Relatorio = relatorio;


            return View("RelatorioDocumentos", docDocumento);
        }
        public ViewResult DocumentoVerificacao(Relatorio relatorio)
        {

            var idSite = Util.ObterSiteSelecionado();

            var docDocumento = _docDocumentoAppServico.Get(x => x.IdSite == idSite && x.FlStatus == (int)StatusDocumento.Verificacao).ToList();

            ViewBag.Relatorio = relatorio;

            return View("RelatorioDocumentos", docDocumento);
        }
        public ViewResult DocumentoElaboracao(Relatorio relatorio)
        {

            var idSite = Util.ObterSiteSelecionado();

            var docDocumento = _docDocumentoAppServico.Get(x => x.IdSite == idSite && x.FlStatus == (int)StatusDocumento.Elaboracao).ToList();

            ViewBag.Relatorio = relatorio;

            return View("RelatorioDocumentos", docDocumento);
        }

        public ViewResult DocumentoVencimento(Relatorio relatorio)
        {

            var idSite = Util.ObterSiteSelecionado();

            var dt30 = DateTime.Now.AddDays(30);

            var docDocumento = _docDocumentoAppServico.Get(x => x.IdSite == idSite && x.FlStatus == (int)StatusDocumento.Aprovado && x.FlRevisaoPeriodica == true && x.DtVencimento > dt30).ToList();

            ViewBag.Relatorio = relatorio;

            return View("RelatorioDocumentos", docDocumento);
        }
        #endregion

        #region Gestão de Risco
        public ViewResult GeradasTipo(Relatorio relatorio)
        {
            var IdSite = Util.ObterSiteSelecionado();
            var idTipo = Convert.ToInt32(relatorio.Parametros["Tipo"]);
            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            var tpRegistro = string.Empty;

            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }
            ViewBag.Relatorio = relatorio;
            var model = _registroConformidadesAppServico.Get(x => x.IdSite == IdSite && x.TipoRegistro == tpRegistro && x.IdTipoNaoConformidade == idTipo).ToList();
            return View("RelatorioRegistroTipo", model);
        }
        public ViewResult AbertasPendentes(Relatorio relatorio)
        {

            var IdSite = Util.ObterSiteSelecionado();
            var tpRegistro = string.Empty;

            List<RelatorioPendenteViewModel> model = new List<RelatorioPendenteViewModel>();
            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    model = RetornaListaNaoConformidade(IdSite);
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    model = RetornaListaAcaoImediata(IdSite);
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    model = RetornaListaGestaoDeRisco(IdSite);
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    model = RetornaListaGestaoDeMelhoria(IdSite);
                    break;
                default:
                    break;
            }
            ViewBag.Relatorio = relatorio;
            return View("RelatorioRegistroPendente", model);
        }
        public ViewResult AbertaReverificacaoAtraso(Relatorio relatorio)
        {
            var IdSite = Util.ObterSiteSelecionado();

            var tpRegistro = string.Empty;
            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }
            ViewBag.Relatorio = relatorio;
            using (var db = new BaseContext())
            {

                var RegistroRev = (from ac in db.RegistroConformidade
                                   where ac.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && ac.TipoRegistro == tpRegistro && ac.DtPrazoImplementacao < DateTime.Now && ac.IdSite == IdSite
                                   select ac).ToList();

                return View("RelatorioRegistroPendente", RegistroRev);
            }
        }
        public ViewResult AbertasEncerradas(Relatorio relatorio)
        {
            var IdSite = Util.ObterSiteSelecionado();


            var tpRegistro = string.Empty;
            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }
            ViewBag.Relatorio = relatorio;
            var model = _registroConformidadesAppServico.Get(x => x.IdSite == IdSite && x.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && x.TipoRegistro == tpRegistro).ToList();

            return View("RelatorioRegistroEncerrada", model);
        }
        public ViewResult TempoTratamentoRegistro(Relatorio relatorio)
        {
            var IdSite = Util.ObterSiteSelecionado();
            var tpRegistro = string.Empty;
            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }
            ViewBag.Relatorio = relatorio;
            var model = _registroConformidadesAppServico.Get(x => x.IdSite == IdSite && x.StatusEtapa == (byte)EtapasRegistroConformidade.Encerrada && x.TipoRegistro == tpRegistro).ToList();
            return View("RelatorioRegistroTempo", model);
        }
        public ViewResult TempoAberturaAnaliseDefinicao(Relatorio relatorio)
        {
            var IdSite = Util.ObterSiteSelecionado();

            var tpRegistro = string.Empty;
            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }
            ViewBag.Relatorio = relatorio;
            var model = _registroConformidadesAppServico.Get(x => x.IdSite == IdSite && x.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && x.TipoRegistro == tpRegistro && x.DtEfetivaImplementacao != null).ToList();
            return View("RelatorioRegistroTempoDefinicao", model);
        }
        public ViewResult GeradaProcesso(Relatorio relatorio)
        {
            var IdSite = Util.ObterSiteSelecionado();
            var idProcesso = Convert.ToInt32(relatorio.Parametros["Processo"]);

            var tpRegistro = string.Empty;
            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }
            ViewBag.Relatorio = relatorio;
            var model = _registroConformidadesAppServico.Get(x => x.TipoRegistro == tpRegistro && x.IdProcesso == idProcesso).ToList();
            return View("RelatorioRegistroProcesso", model);
        }
        public ViewResult Anuladas(Relatorio relatorio)
        {
            var IdSite = Util.ObterSiteSelecionado();

            var tpRegistro = string.Empty;
            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }
            ViewBag.Relatorio = relatorio;
            var model = _registroConformidadesAppServico.Get(x => x.TipoRegistro == tpRegistro && x.StatusEtapa == (byte)EtapasRegistroConformidade.Anulada).ToList();
            return View("RelatorioRegistroAnulada", model);
        }

        #endregion


        public ViewResult Abertas(Relatorio relatorio)
        {
            DateTime monthParam = new DateTime();

            var IdSite = Util.ObterSiteSelecionado();

            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            string tpRegistro = string.Empty;

            Dictionary<int, int?> Meses = new Dictionary<int, int?>();

            Meses.Add(monthParam.Month, 0);


            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }


            if (relatorio.Parametros.ContainsKey("Mês"))
            {
                var param1 = "01/" + relatorio.Parametros["Mês"];
                monthParam = Convert.ToDateTime(param1);


            }

            var model = _registroConformidadesAppServico.Get(x => x.IdSite == IdSite && x.DtInclusao.Month == monthParam.Month && x.DtInclusao.Year == monthParam.Year && x.TipoRegistro == tpRegistro && x.StatusEtapa != (byte)EtapasRegistroConformidade.Encerrada).ToList();

            Meses[monthParam.Month] = model.Count();

            List<int?> AbertasMes = new List<int?> {
                Meses[monthParam.Month].Value
            };


            List<BarSeriesData> MesesData = new List<BarSeriesData>();


            AbertasMes.ForEach(p => MesesData.Add(new BarSeriesData { Y = p }));

            List<string> Categories = new List<string>();

            Categories.Add(DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthParam.Month));

            ViewData["AbertaMes"] = MesesData;
            ViewData["Categories"] = Categories;

            return View("Abertas", relatorio);
        }
        public ViewResult AbertasAno(Relatorio relatorio)
        {
            var YearParam = 0;

            var IdSite = Util.ObterSiteSelecionado();

            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            string tpRegistro = string.Empty;

            Dictionary<int, int?> Year = new Dictionary<int, int?>();

            Year.Add(YearParam, 0);


            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }


            if (relatorio.Parametros.ContainsKey("Ano"))
            {
                var param1 = relatorio.Parametros["Ano"];
                YearParam = int.Parse(param1);


            }

            var model = _registroConformidadesAppServico.Get(x => x.IdSite == IdSite && x.DtInclusao.Year == YearParam && x.TipoRegistro == tpRegistro && x.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata).ToList();

            Year[YearParam] = model.Count();

            List<int?> AbertasMes = new List<int?> {
                Year[YearParam].Value
            };


            List<BarSeriesData> MesesData = new List<BarSeriesData>();


            AbertasMes.ForEach(p => MesesData.Add(new BarSeriesData { Y = p }));

            List<string> Categories = new List<string>();

            Categories.Add(YearParam.ToString());

            ViewData["AbertasAno"] = MesesData;
            ViewData["Categories"] = Categories;

            return View("AbertasAno", relatorio);
        }
        public ViewResult FechadasAno(Relatorio relatorio)
        {
            var YearParam = 0;

            var IdSite = Util.ObterSiteSelecionado();

            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            string tpRegistro = string.Empty;

            Dictionary<int, int?> Year = new Dictionary<int, int?>();

            Year.Add(YearParam, 0);


            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }


            if (relatorio.Parametros.ContainsKey("Ano"))
            {
                var param1 = relatorio.Parametros["Ano"];
                YearParam = int.Parse(param1);


            }

            var model = _registroConformidadesAppServico.Get(x => x.IdSite == IdSite && x.DtInclusao.Year == YearParam && x.TipoRegistro == tpRegistro && x.StatusEtapa == (byte)EtapasRegistroConformidade.Encerrada).ToList();

            Year[YearParam] = model.Count();

            List<int?> AbertasMes = new List<int?> {
                Year[YearParam].Value
            };

            List<BarSeriesData> MesesData = new List<BarSeriesData>();

            AbertasMes.ForEach(p => MesesData.Add(new BarSeriesData { Y = p }));

            List<string> Categories = new List<string>();

            Categories.Add(YearParam.ToString());

            ViewData["FechadasAno"] = MesesData;
            ViewData["Categories"] = Categories;

            return View("FechadasAno", relatorio);
        }
        public ViewResult Fechadas(Relatorio relatorio)
        {
            DateTime monthParam = new DateTime();

            var IdSite = Util.ObterSiteSelecionado();

            var relatParam = _relatorioAppServico.GetById(relatorio.IdRelatorio);
            string tpRegistro = string.Empty;

            Dictionary<int, int?> Meses = new Dictionary<int, int?>();

            Meses.Add(monthParam.Month, 0);


            switch (relatParam.Funcionalidade.Url)
            {

                case "NaoConformidade":
                    tpRegistro = "NC";
                    break;
                case "AcaoCorretiva":
                    tpRegistro = "AC";
                    break;
                case "GestaoDeRisco":
                    tpRegistro = "GR";
                    break;
                case "GestaoMelhoria":
                    tpRegistro = "GM";
                    break;
                default:
                    break;
            }

            if (relatorio.Parametros.ContainsKey("Mês"))
            {
                var param1 = "01/" + relatorio.Parametros["Mês"];
                monthParam = Convert.ToDateTime(param1);

            }

            var model = _registroConformidadesAppServico.Get(x => x.IdSite == IdSite && x.DtInclusao.Month == monthParam.Month && x.DtInclusao.Year == monthParam.Year && x.TipoRegistro == tpRegistro && x.StatusEtapa == (byte)EtapasRegistroConformidade.Encerrada).ToList();

            Meses[monthParam.Month] = model.Count();

            List<int?> AbertasMes = new List<int?> {
                Meses[monthParam.Month].Value
            };

            List<BarSeriesData> MesesData = new List<BarSeriesData>();

            AbertasMes.ForEach(p => MesesData.Add(new BarSeriesData { Y = p }));

            List<string> Categories = new List<string>();

            Categories.Add(DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(monthParam.Month));

            ViewData["FechadasMes"] = MesesData;
            ViewData["Categories"] = Categories;

            return View("Fechadas", relatorio);
        }
        #endregion

        #region Metodos

        public List<RelatorioPendenteViewModel> RetornaListaNaoConformidade(int idSite)
        {
            using (var db = new BaseContext())
            {
                List<RelatorioPendenteViewModel> registroConformidades = new List<RelatorioPendenteViewModel>();



                var naoConformidade = (from nc in db.RegistroConformidade
                                       join pr in db.Processo on nc.IdProcesso equals pr.IdProcesso
                                       where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.IdSite == idSite && nc.TipoRegistro == "nc"
                                       select new RelatorioPendenteViewModel
                                       {
                                           Nr = nc.NuRegistro.ToString(),
                                           dtEmissao = nc.DtEmissao,
                                           ProcessoNome = pr.Nome,
                                           dtInclusao = nc.DtInclusao,
                                           Responsavel = nc.ResponsavelInicarAcaoImediata.NmCompleto
                                       }).ToList();


                var naoConformidadePrazo = (from nc in db.RegistroConformidade
                                            join pr in db.Processo on nc.IdProcesso equals pr.IdProcesso
                                            join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                            where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && nc.IdSite == idSite && nc.TipoRegistro == "nc"
                                            select new RelatorioPendenteViewModel
                                            {
                                                Nr = nc.NuRegistro.ToString(),
                                                dtEmissao = nc.DtEmissao,
                                                ProcessoNome = pr.Nome,
                                                dtInclusao = nc.DtInclusao,
                                                Responsavel = ai.ResponsavelImplementar.NmCompleto
                                            }).ToList();


                var naoConformidadeReverificacao = (from nc in db.RegistroConformidade
                                                    join pr in db.Processo on nc.IdProcesso equals pr.IdProcesso
                                                    where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "nc" && nc.IdSite == idSite
                                                    select new RelatorioPendenteViewModel
                                                    {
                                                        Nr = nc.NuRegistro.ToString(),
                                                        dtEmissao = nc.DtEmissao,
                                                        ProcessoNome = pr.Nome,
                                                        dtInclusao = nc.DtInclusao,
                                                        Responsavel = nc.ResponsavelReverificador.NmCompleto
                                                    }).ToList();

                registroConformidades.AddRange(naoConformidade);
                registroConformidades.AddRange(naoConformidadePrazo);
                registroConformidades.AddRange(naoConformidadeReverificacao);


                return registroConformidades;
            }
        }
        public List<RelatorioPendenteViewModel> RetornaListaGestaoDeMelhoria(int idSite)
        {
            using (var db = new BaseContext())
            {
                List<RelatorioPendenteViewModel> registroConformidades = new List<RelatorioPendenteViewModel>();

                var gestaoMelhoria = (from nc in db.RegistroConformidade
                                      join pr in db.Processo on nc.IdProcesso equals pr.IdProcesso
                                      where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.IdSite == idSite && nc.TipoRegistro == "gm"
                                      select new RelatorioPendenteViewModel
                                      {
                                          Nr = nc.NuRegistro.ToString(),
                                          dtEmissao = nc.DtEmissao,
                                          ProcessoNome = pr.Nome,
                                          dtInclusao = nc.DtInclusao,
                                          Responsavel = nc.ResponsavelInicarAcaoImediata.NmCompleto
                                      }).ToList();

                var gestaoMelhoriaPrazo = (from nc in db.RegistroConformidade
                                           join pr in db.Processo on nc.IdProcesso equals pr.IdProcesso
                                           join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                           where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && nc.IdSite == idSite && nc.TipoRegistro == "gm"
                                           select new RelatorioPendenteViewModel
                                           {
                                               Nr = nc.NuRegistro.ToString(),
                                               dtEmissao = nc.DtEmissao,
                                               ProcessoNome = pr.Nome,
                                               dtInclusao = nc.DtInclusao,
                                               Responsavel = ai.ResponsavelImplementar.NmCompleto
                                           }).ToList();

                var gestaoMelhoriaReverificacao = (from nc in db.RegistroConformidade
                                                   join pr in db.Processo on nc.IdProcesso equals pr.IdProcesso
                                                   where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "gm" && nc.IdSite == idSite
                                                   select new RelatorioPendenteViewModel
                                                   {
                                                       Nr = nc.NuRegistro.ToString(),
                                                       dtEmissao = nc.DtEmissao,
                                                       ProcessoNome = pr.Nome,
                                                       dtInclusao = nc.DtInclusao,
                                                       Responsavel = nc.ResponsavelReverificador.NmCompleto
                                                   }).ToList();

                registroConformidades.AddRange(gestaoMelhoria);
                registroConformidades.AddRange(gestaoMelhoriaPrazo);
                registroConformidades.AddRange(gestaoMelhoriaReverificacao);

                return registroConformidades;
            }
        }
        public List<RelatorioPendenteViewModel> RetornaListaAcaoImediata(int idSite)
        {
            using (var db = new BaseContext())
            {
                List<RelatorioPendenteViewModel> registroConformidades = new List<RelatorioPendenteViewModel>();

                var acaoCorretiva = (from ac in db.RegistroConformidade
                                     join pr in db.Processo on ac.IdProcesso equals pr.IdProcesso
                                     join ai in db.AcaoImediata on ac.IdRegistroConformidade equals ai.IdRegistroConformidade
                                     where ac.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && ac.IdSite == idSite && ac.TipoRegistro == "ac"
                                     select new RelatorioPendenteViewModel
                                     {
                                         Nr = ac.NuRegistro.ToString(),
                                         dtEmissao = ac.DtEmissao,
                                         ProcessoNome = pr.Nome,
                                         dtInclusao = ac.DtInclusao,
                                         Responsavel = ai.ResponsavelImplementar.NmCompleto
                                     }).ToList();


                var acaoCorretivaRev = (from ac in db.RegistroConformidade
                                        join pr in db.Processo on ac.IdProcesso equals pr.IdProcesso
                                        where ac.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && ac.TipoRegistro == "ac" && ac.DtPrazoImplementacao < DateTime.Now && ac.IdSite == idSite
                                        select new RelatorioPendenteViewModel
                                        {
                                            Nr = ac.NuRegistro.ToString(),
                                            dtEmissao = ac.DtEmissao,
                                            ProcessoNome = pr.Nome,
                                            dtInclusao = ac.DtInclusao,
                                            Responsavel = ac.ResponsavelReverificador.NmCompleto
                                        }).ToList();

                registroConformidades.AddRange(acaoCorretiva);
                registroConformidades.AddRange(acaoCorretivaRev);

                return registroConformidades;
            }

        }
        public List<RelatorioPendenteViewModel> RetornaListaGestaoDeRisco(int idSite)
        {
            using (var db = new BaseContext())
            {
                List<RelatorioPendenteViewModel> registroConformidades = new List<RelatorioPendenteViewModel>();

                var gestaoRisco = (from nc in db.RegistroConformidade
                                   join pr in db.Processo on nc.IdProcesso equals pr.IdProcesso
                                   where nc.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata && nc.IdSite == idSite && nc.TipoRegistro == "gr"
                                   select new RelatorioPendenteViewModel
                                   {
                                       Nr = nc.NuRegistro.ToString(),
                                       dtEmissao = nc.DtEmissao,
                                       ProcessoNome = pr.Nome,
                                       dtInclusao = nc.DtInclusao,
                                       Responsavel = nc.ResponsavelInicarAcaoImediata.NmCompleto
                                   }).ToList();

                var gestaoRiscoPrazo = (from nc in db.RegistroConformidade
                                        join pr in db.Processo on nc.IdProcesso equals pr.IdProcesso
                                        join ai in db.AcaoImediata on nc.IdRegistroConformidade equals ai.IdRegistroConformidade
                                        where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao && ai.DtPrazoImplementacao < DateTime.Now && ai.DtEfetivaImplementacao == null && nc.IdSite == idSite && nc.TipoRegistro == "gr"
                                        select new RelatorioPendenteViewModel
                                        {
                                            Nr = nc.NuRegistro.ToString(),
                                            dtEmissao = nc.DtEmissao,
                                            ProcessoNome = pr.Nome,
                                            dtInclusao = nc.DtInclusao,
                                            Responsavel = ai.ResponsavelImplementar.NmCompleto
                                        }).ToList();

                var gestaoRiscoReverificacao = (from nc in db.RegistroConformidade
                                                join pr in db.Processo on nc.IdProcesso equals pr.IdProcesso
                                                where nc.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao && nc.TipoRegistro == "gr" && nc.IdSite == idSite
                                                select new RelatorioPendenteViewModel
                                                {
                                                    Nr = nc.NuRegistro.ToString(),
                                                    dtEmissao = nc.DtEmissao,
                                                    ProcessoNome = pr.Nome,
                                                    dtInclusao = nc.DtInclusao,
                                                    Responsavel = nc.ResponsavelReverificador.NmCompleto
                                                }).ToList();

                registroConformidades.AddRange(gestaoRisco);
                registroConformidades.AddRange(gestaoRiscoPrazo);
                registroConformidades.AddRange(gestaoRiscoReverificacao);

                return registroConformidades;
            }

        }
        #endregion
    }
}