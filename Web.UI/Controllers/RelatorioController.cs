using System;
using System.Linq;
using System.Web.Mvc;
using Dominio.Enumerado;
using ApplicationService.Interface;
using Web.UI.Helpers;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class RelatorioController : BaseController
    {
        private readonly ILogAppServico _logAppServico;
        private readonly IRegistroConformidadesAppServico _naoConformidade;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;


        public RelatorioController(ILogAppServico logAppServico, IRegistroConformidadesAppServico naoConformidade, IUsuarioAppServico usuarioAppServico, IProcessoAppServico processoAppServico,
           IPendenciaAppServico pendenciaAppServico, IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico,  pendenciaAppServico)
        {
            _logAppServico = logAppServico;
            _naoConformidade = naoConformidade;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
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

                var naoConformidade = new {
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
    }
}