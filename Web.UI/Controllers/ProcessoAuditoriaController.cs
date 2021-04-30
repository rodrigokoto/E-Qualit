using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Web.UI.Helpers;

namespace Web.UI.Controllers
{
    [ProcessoSelecionado]
    [VerificaIntegridadeLogin]
    public class ProcessoAuditoriaController : BaseController
    {
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IProcessoServico _processoServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public ProcessoAuditoriaController(IProcessoAppServico processoAppServico,
            IProcessoServico processoServico,
            ILogAppServico logAppServico,
            IUsuarioAppServico usuarioAppServico,
            IPendenciaAppServico pendenciaAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico,  pendenciaAppServico)
        {
            _processoAppServico = processoAppServico;
            _processoServico = processoServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        public JsonResult Salvar(Processo processo)
        {
            var erros = new List<string>();

            _processoServico.Valido(processo, ref erros);

            if (erros.Count == 0)
            {
                if (processo.IdProcesso != 0)
                {
                    _processoAppServico.Update(processo);
                }
                else
                {
                    processo.IdSite = Util.ObterSiteSelecionado();
                    processo.DataCadastro = DateTime.Now;
                    _processoAppServico.Add(processo);
                }
            }
            else
            {
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = 200 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Criar(int? id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var processoAuditoria = new Processo();

            if (id != null)
            {
                processoAuditoria = _processoAppServico.GetById(Convert.ToInt32(id));
            }

            ViewBag.IdSite = 1;

            return View(processoAuditoria);
        }

        [ValidateAntiForgeryToken]
        //[Autorizacao(Perfis = new PerfisAcesso[]
        //{ PerfisAcesso.Administrador, PerfisAcesso.Coordenador })]
        public ActionResult Excluir(int id)
        {
            var processo = _processoAppServico.GetById(id);

            processo.FlAtivo = false;

            _processoAppServico.Update(processo);

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        //[AutorizacaoModulo(110, 8)]
        public ActionResult Editar(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var processoAuditoria = _processoAppServico.GetById(id);

            return View(processoAuditoria);
        }

        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var idSite = 5;
            var processoAuditorias = _processoAppServico.ListaProcessosPorSite(idSite);

            return View(processoAuditorias);
        }

        public ActionResult PDF(int id)
        {
            var processoAuditoria = _processoAppServico.GetById(id);

            return View(processoAuditoria);
        }

        public ActionResult SalvaPDF(int id)
        {
            GeraArquivoZip(ControllerContext, "PDF", id);

            return View();
        }
    }
}