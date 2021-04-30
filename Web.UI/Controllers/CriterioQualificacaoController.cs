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
    public class CriterioQualificacaoController : BaseController
    {
        private readonly ICriterioQualificacaoAppServico _criterioQualificacaoAppServico;
        private readonly ICriterioQualificacaoServico _criterioQualificacaoServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public CriterioQualificacaoController(ICriterioQualificacaoAppServico criterioQualificacaoAppServico, 
                                              ILogAppServico logAppServico,
                                              ICriterioQualificacaoServico criterioQualificacaoServico,
                                              IUsuarioAppServico usuarioAppServico,
                                              IProcessoAppServico processoAppServico,
                                              IPendenciaAppServico pendenciaAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico,  pendenciaAppServico)
        {
            _criterioQualificacaoAppServico = criterioQualificacaoAppServico;
            _logAppServico = logAppServico;
            _criterioQualificacaoServico = criterioQualificacaoServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;

        }

        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            return View();
        }

        [HttpGet]
        public JsonResult ObterPorId(int idCriterioQualidicacao)
        {

            var criterioQualificacao = _criterioQualificacaoAppServico.GetById(idCriterioQualidicacao);
            if (criterioQualificacao != null)
            {
                return Json(new {StatusCode = (int)HttpStatusCode.OK, CriterioQualificacao = criterioQualificacao }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erro = Traducao.Resource.CriterioQualificacao_msg_no_exist }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult Salvar(CriterioQualificacao criterioQualificacao)
        {
            var erros = new List<string>();

            _criterioQualificacaoServico.ValidaCampos(criterioQualificacao, ref erros);

            if (erros.Count == 0)
            {
                criterioQualificacao.DtCriacao = DateTime.Now;
                criterioQualificacao.DtAlteracao = DateTime.Now;

                _criterioQualificacaoAppServico.Add(criterioQualificacao);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult SalvarQualificacao(CriterioQualificacao criterioQualificacao)
        {
            var erros = new List<string>();

            _criterioQualificacaoServico.ValidaCampos(criterioQualificacao, ref erros);

            if (erros.Count == 0)
            {
                criterioQualificacao.DtCriacao = DateTime.Now;
                criterioQualificacao.DtAlteracao = DateTime.Now;

                _criterioQualificacaoAppServico.SalvarQualificacao(criterioQualificacao);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult Atualizar(CriterioQualificacao criterioQualificacao)
        {
            var erros = new List<string>();

            _criterioQualificacaoServico.ValidaCampos(criterioQualificacao, ref erros);

            if (erros.Count == 0)
            {
                criterioQualificacao.DtCriacao = DateTime.Now;
                criterioQualificacao.DtAlteracao = DateTime.Now;

                _criterioQualificacaoAppServico.Update(criterioQualificacao);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult Excluir(CriterioQualificacao criterioQualificacao)
        {
            var erros = new List<string>();

            _criterioQualificacaoServico.ValidaCampos(criterioQualificacao, ref erros);

            if (erros.Count == 0)
            {
                criterioQualificacao.DtCriacao = DateTime.Now;
                criterioQualificacao.DtAlteracao = DateTime.Now;

                _criterioQualificacaoAppServico.Remove(criterioQualificacao);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListaCriteriosPadroes()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var criterioQualificacaoPadrao = new List<CriterioQualificacao>();
            int idSite = Util.ObterSiteSelecionado();

            criterioQualificacaoPadrao.AddRange(_criterioQualificacaoAppServico.Get(x => x.Produto.IdSite == idSite).Select(x => new CriterioQualificacao() { IdCriterioQualificacao=x.IdCriterioQualificacao, Titulo = x.Titulo }).ToList());

            criterioQualificacaoPadrao = criterioQualificacaoPadrao.Distinct().ToList();

            return Json(new { StatusCode = (int)HttpStatusCode.OK, CriterioQualificacaoPadrao = criterioQualificacaoPadrao }, JsonRequestBehavior.AllowGet);
        }

    }
}