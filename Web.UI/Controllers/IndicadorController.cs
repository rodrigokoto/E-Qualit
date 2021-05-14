using Dominio.Entidade;
using ApplicationService.Interface;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.UI.Helpers;
using Dominio.Interface.Servico;
using System.Net;
using System.Data.Entity;
using DAL.Context;
using Newtonsoft.Json;
using Highsoft.Web.Mvc.Charts;
using Dominio.Enumerado;

namespace Web.UI.Controllers
{
    //[ProcessoSelecionado]
    [VerificaIntegridadeLogin]
    [SitePossuiModulo(5)]
    public class IndicadorController : BaseController
    {

        private readonly IIndicadorAppServico _indicadorAppServico;
        private readonly IIndicadorServico _indicadorServico;

        private readonly ILogAppServico _logAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;


        public IndicadorController(IIndicadorAppServico indicadorAppServico,
                                   ILogAppServico logAppServico,
                                   IIndicadorServico indicadorServico,
                                   IUsuarioAppServico usuarioAppServico,
                                   IProcessoAppServico processoAppServico,
                                   IPendenciaAppServico pendenciaAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico,  pendenciaAppServico)
        {
            _indicadorAppServico = indicadorAppServico;
            _indicadorServico = indicadorServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        public ActionResult DashBoard()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            return View();
        }

        public ActionResult Index()
        {

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var idSite = Util.ObterSiteSelecionado();


            var indicadores = _indicadorAppServico.Get(x => x.IdSite == idSite).ToList();

            return View(indicadores);
        }

        public ActionResult Criar()
        {

            ViewBag.IdSite = Util.ObterSiteSelecionado();

            return View(new Indicador());
        }

        public ActionResult Editar(int? id)
        {

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();

            var indicador = new Indicador();

            if (id != null)
            {
                indicador = _indicadorAppServico.GetById(Convert.ToInt32(id));
            }

            if ((indicador.StatusRegistro == 0) && (indicador.IdResponsavel == Util.ObterCodigoUsuarioLogado()))
            {
                ViewBag.ScriptCall = "sim";
            }

            return View("Criar", indicador);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Salvar(Indicador indicador)
        {
            var erros = new List<string>();

            try
            {
                indicador.DataInclusao = DateTime.Now;
                indicador.DataAlteracao = DateTime.Now;
                indicador.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
                indicador.StatusRegistro = 1;

                foreach (var periodicidadeDeAnalises in indicador.PeriodicidadeDeAnalises)
                {
                    periodicidadeDeAnalises.Inicio = DateTime.Now;
                    periodicidadeDeAnalises.Fim = DateTime.Now.AddYears(1);

                    var metasRealizadas = periodicidadeDeAnalises.MetasRealizadas;

                    var periodicidadeObj = _indicadorAppServico.GetById(indicador.Id);

                    if (periodicidadeObj != null)
                    {
                        foreach (var item in periodicidadeObj.PeriodicidadeDeAnalises)
                        {
                            var analise = item.MetasRealizadas.Where(x => x.Analise != null).ToList();

                            foreach (var meta in analise)
                            {
                                var teste = metasRealizadas.Where(x => x.Id == meta.Id).First();

                                if (teste.Analise == null)
                                {
                                    metasRealizadas.Where(x => x.Id == meta.Id).First().Analise = meta.Analise;
                                }
                            }
                        }
                    }

                    metasRealizadas.ForEach(metaRealizada => metaRealizada.IdPeriodicidadeAnalise = periodicidadeDeAnalises.Id);
                    periodicidadeDeAnalises.MetasRealizadas = metasRealizadas;

                    foreach (var metaRealizada in periodicidadeDeAnalises.MetasRealizadas)
                    {
                        metaRealizada.DataInclusao = DateTime.Now;
                        metaRealizada.DataAlteracao = DateTime.Now;
                    }
                }

                _indicadorServico.Valido(indicador, ref erros);

                if (erros.Count == 0)
                {
                    if (indicador.Id == 0)
                    {
                        _indicadorAppServico.Add(indicador);
                        GravarLogInclusao((int)Funcionalidades.Indicadores, indicador.Id);
                    }
                    else
                    {
                        _indicadorAppServico.Atualizar(indicador);

                        var ind = _indicadorAppServico.GetById(indicador.Id);
                        ind.IdProcesso = indicador.IdProcesso;
                        ind.IdResponsavel = indicador.IdResponsavel;
                        ind.PeriodicidadeMedicao = indicador.PeriodicidadeMedicao;
                        ind.Periodicidade = indicador.Periodicidade;
                        ind.Direcao = indicador.Direcao;
                        ind.Ano = indicador.Ano;
                        ind.MetaAnual = indicador.MetaAnual;
                        ind.Medida = indicador.Medida;
                        ind.Unidade = indicador.Unidade;

                        _indicadorAppServico.Update(ind);
                        GravarLogAlteracao((int)Funcionalidades.Indicadores, indicador.Id);

                    }
                }
                else
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Indicador.ResourceIndicador.Indicador_msg_save_valid, IdIndicador = indicador.Id }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Excluir(Indicador indicador)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var erros = new List<string>();

            try
            {
                using (var db = new BaseContext())
                {
                    var ObjIndicador = _indicadorAppServico.GetById(indicador.Id);

                    foreach (var periodicidaDe in ObjIndicador.PeriodicidadeDeAnalises)
                    {

                        if (periodicidaDe.MetasRealizadas.Count > 0)
                        {

                            foreach (var item in periodicidaDe.MetasRealizadas)
                            {
                                var planovoo = db.PlanoVoo.Where(x => x.Id == item.Id).FirstOrDefault();
                                db.PlanoVoo.Remove(planovoo);
                            }
                        }
                        if (periodicidaDe.PlanoDeVoo.Count > 0)
                        {
                            foreach (var item in periodicidaDe.PlanoDeVoo)
                            {
                                var meta = db.Meta.Where(x => x.Id == item.Id).FirstOrDefault();
                                db.Meta.Remove(meta);
                            }
                        }
                        var periodicidade = db.PeriodicidaDeAnalise.Where(x => x.Id == periodicidaDe.Id).FirstOrDefault();
                        db.PeriodicidaDeAnalise.Remove(periodicidade);

                    }
                    db.Indicador.Remove(db.Indicador.Where(x => x.Id == ObjIndicador.Id).FirstOrDefault());
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Norma.ResourceNorma.Norma_msg_del_valid }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PDF(int id)
        {
            var indicador = _indicadorAppServico.GetById(id);

            return View(indicador);
        }

        public ActionResult PDFDownload(int id)
        {
            var indicador = _indicadorAppServico.GetById(id);

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();

            if ((indicador.StatusRegistro == 0) && (indicador.IdResponsavel == Util.ObterCodigoUsuarioLogado()))
            {
                ViewBag.ScriptCall = "sim";
            }

            List<double?> year1800Values = new List<double?> { 107, 31, 635, 203, 2 };
            List<double?> year1900Values = new List<double?> { 133, 156, 947, 408, 6 };
            List<double?> year2008Values = new List<double?> { 973, 914, 4054, 732, 34 };

            List<BarSeriesData> year1800Data = new List<BarSeriesData>();
            List<BarSeriesData> year1900Data = new List<BarSeriesData>();
            List<BarSeriesData> year2008Data = new List<BarSeriesData>();

            year1800Values.ForEach(p => year1800Data.Add(new BarSeriesData { Y = p }));
            year1900Values.ForEach(p => year1900Data.Add(new BarSeriesData { Y = p }));
            year2008Values.ForEach(p => year2008Data.Add(new BarSeriesData { Y = p }));

            ViewData["year1800Data"] = year1800Data;
            ViewData["year1900Data"] = year1900Data;
            ViewData["year2008Data"] = year2008Data;

            var pdf = new ViewAsPdf
            {
                ViewName = "PDF",
                Model = indicador,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "Indicador.pdf", 
                IsJavaScriptDisabled = false, 
                CustomSwitches = "--debug-javascript --no-stop-slow-scripts --javascript-delay 10000"
            };
            GravarLogImpressao((int)Funcionalidades.Indicadores, indicador.Id);
            return pdf;
        }

        public ActionResult SalvaPDF(int id)
        {
            GeraArquivoZip(ControllerContext, "PDF", id);

            return View();
        }

        // GET: Indicador
        [HttpGet]
        public ActionResult GerarPartialGestaoRisco(int Periodicidade, int mes, int? idplanovoo)
        {

            if (idplanovoo == null)
            {
                return PartialView("GestaoDeRiscoIndicador", new PlanoVoo());
            }
            else
            {
                using (var db = new BaseContext())
                {
                    var result = (from pl in db.PlanoVoo
                                  where pl.IdPeriodicidadeAnalise == Periodicidade &&
                                  pl.Id == idplanovoo
                                  select pl).FirstOrDefault();

                    if (result.GestaoDeRisco != null)
                    {
                        if (result.GestaoDeRisco.IdResponsavelInicarAcaoImediata != null)
                        {
                            result.GestaoDeRisco.ResponsavelInicarAcaoImediata = _usuarioAppServico.GetById((int)result.GestaoDeRisco.IdResponsavelInicarAcaoImediata);
                        }
                    }
                    return PartialView("GestaoDeRiscoIndicador", result);
                }
            }
        }
        public ActionResult RelatorioBarras()
        {
            return View();
        }

        // GET: Indicador
        public JsonResult RelatorioBarrasJSON(int? IdIndicador)
        {
            try
            {
                var idSiteCorrente = Util.ObterSiteSelecionado();
                //var idProcessoCorrente = Util.ObterProcessoSelecionado();

                IEnumerable<Indicador> indicadores;
                List<PlanoVoo> planoVoos = new List<PlanoVoo>();
                if (IdIndicador != null)
                {
                    var result = _indicadorAppServico.Get(x => x.Id == IdIndicador).ToList();

                    foreach (var indicador in result)
                    {
                        foreach (var periodicidade in indicador.PeriodicidadeDeAnalises)
                        {
                            //periodicidade.PlanoDeVoo = periodicidade.PlanoDeVoo.Where(x => x.Valor > 0).ToList();


                            foreach (var plano in periodicidade.PlanoDeVoo)
                            {
                                planoVoos.Add(periodicidade.MetasRealizadas.Where(x => x.DataReferencia == plano.DataReferencia).FirstOrDefault());
                            }
                            periodicidade.MetasRealizadas = planoVoos;
                        }
                    }
                    indicadores = result;
                }
                else
                {
                    indicadores = _indicadorAppServico.IndicadoresPorProcessoESite(idSiteCorrente);
                }

                foreach (var indicador in indicadores)
                {
                    indicador.IdResponsavel = 0;
                    indicador.Responsavel = null;

                    indicador.IdProcesso = 0;
                    indicador.Processo = null;

                    foreach (var periodicidaDeAnalise in indicador.PeriodicidadeDeAnalises)
                    {

                        periodicidaDeAnalise.IdIndicador = 0;
                        periodicidaDeAnalise.Indicador = null;


                        foreach (var meta in periodicidaDeAnalise.PlanoDeVoo)
                        {
                            meta.IdPeriodicidadeAnalise = 0;
                            meta.PeriodicidadeAnalise = null;
                        }

                        foreach (var planoDeVoo in periodicidaDeAnalise.MetasRealizadas)
                        {
                            planoDeVoo.IdPeriodicidadeAnalise = 0;
                            planoDeVoo.PeriodicidadeAnalise = null;
                            planoDeVoo.IdGestaoRisco = 0;
                            planoDeVoo.GestaoDeRisco = null;
                            planoDeVoo.IdProcesso = 0;
                            planoDeVoo.Processo = null;
                        }
                    }
                }



                return Json(new { StatusCode = 200, Indicadores = indicadores }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = ex }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Indicador
        public ActionResult RelatorioColuna()
        {
            return View();
        }

        public JsonResult RelatorioIndicador(int id)
        {
            var indicador = _indicadorAppServico.GetById(id);

            var serieJson = new
            {
                Name = "QualquerCoisa",
                Metas = indicador.PeriodicidadeDeAnalises.FirstOrDefault().PlanoDeVoo.Select(x => x.Valor),
                PlanoDeVoo = indicador.PeriodicidadeDeAnalises.FirstOrDefault().MetasRealizadas.Select(x => x.Realizado),
                Categorias = indicador.PeriodicidadeDeAnalises.FirstOrDefault().PlanoDeVoo.Select(x => x.DataReferencia.ToString("MMMM")),
                Unidade = indicador.Unidade
            };

            //string json = JsonConvert.SerializeObject(serieJson);

            return Json(new { StatusCode = 200, Dados = serieJson }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAll()
        {
            var indicadores = _indicadorAppServico.GetAll().ToList();

            return Json(new { StatusCode = 200, indicadores }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DestravarDocumento(string idIndicador)
        {
            var erros = new List<string>();

            try
            {
                var indicador = _indicadorAppServico.GetById(int.Parse(idIndicador));
                indicador.StatusRegistro = 0;
                _indicadorAppServico.Update(indicador);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Indicador.ResourceIndicador.Indicador_msg_save_valid }, JsonRequestBehavior.AllowGet);


        }




    }
}