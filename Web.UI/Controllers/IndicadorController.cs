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

namespace Web.UI.Controllers
{
    //[ProcessoSelecionado]
    [VerificaIntegridadeLogin]
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
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
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
        public JsonResult Salvar(Indicador indicador)
        {

            var erros = new List<string>();

            try
            {
                indicador.DataInclusao = DateTime.Now;
                indicador.DataAlteracao = DateTime.Now;
                indicador.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
                indicador.StatusRegistro = 1;

                //ESSE CÓDIGO AQUI TEM QUE TIRAR DAQUI
                //MAS COMO EU NÃO ESTOU AQUI, ESTOU REMOTO
                //VC TIRA ACIOLE

                foreach (var periodicidadeDeAnalises in indicador.PeriodicidadeDeAnalises)
                {
                    periodicidadeDeAnalises.Inicio = DateTime.Now;
                    periodicidadeDeAnalises.Fim = DateTime.Now.AddYears(1);

                    var metasRealizadas = periodicidadeDeAnalises.MetasRealizadas
                            .Where(x => x.Realizado != null).ToList();

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

                    }
                    else
                    {
                        _indicadorAppServico.Atualizar(indicador);
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
                var ObjIndicador = _indicadorAppServico.GetById(indicador.Id);
                _indicadorAppServico.Remove(ObjIndicador);
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

            var pdf = new ViewAsPdf
            {
                ViewName = "PDF",
                Model = indicador,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "Indicador.pdf"
            };

            return pdf;
        }

        public ActionResult SalvaPDF(int id)
        {
            GeraArquivoZip(ControllerContext, "PDF", id);

            return View();
        }

        // GET: Indicador
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

                if (IdIndicador != null)
                {
                    indicadores = _indicadorAppServico.IndicadoresPorProcessoESiteEIndicador(IdIndicador.Value);
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