using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.UI.Helpers;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class ProcessoController : BaseController
    {
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IProcessoServico _processoServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public ProcessoController(IProcessoAppServico processoAppServico,
            IProcessoServico processoServico,
            ILogAppServico logAppServico,
            IUsuarioAppServico usuarioAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _processoAppServico = processoAppServico;
            _processoServico = processoServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        // GET: Processo
        public ActionResult Index()
        {
            return View();

        }

        public ActionResult Criar()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaProcessosPorSite(int idSite)
        {

            var lista = _processoAppServico.ListaProcessosPorSite(idSite).Select(x => new { x.IdProcesso, x.Nome });


            return Json(new { StatusCode = (int)HttpStatusCode.OK, Lista = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListaProcessosPorUsuario(int idUsuario)
        {
            List<Processo> lista = _processoAppServico.ListaProcessosPorUsuario(idUsuario);

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Lista = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarProcesso(List<Processo> processos)
        {
            var erros = new List<string>();
            try
            {
                processos.ForEach(processo => _processoServico.Valido(processo, ref erros));

                if (erros.Count != 0)
                {
                    return Json(new
                    {
                        StatusCode = 500,
                        Erro = erros
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    processos.ForEach(processo=> {
                        if (processo.IdProcesso == 0)
                        {
                            _processoAppServico.Add(processo);

                        }
                        else
                        {
                            var processoCtx = _processoAppServico.GetById(processo.IdProcesso);
                            processoCtx.Nome = processo.Nome;
                            processoCtx.IdSite = processo.IdSite;
                            processoCtx.FlAtivo = processo.FlAtivo;
                            _processoAppServico.Update(processoCtx);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new
                {
                    StatusCode = 500,
                    Erro = ex
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200 }, JsonRequestBehavior.AllowGet);
        }


    }
}