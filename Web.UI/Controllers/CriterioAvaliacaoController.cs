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
    public class CriterioAvaliacaoController : BaseController
    {
        private readonly ICriterioAvaliacaoAppServico _qualificaAvaliacaoCriticidadeAppServico;
        private readonly ICriterioAvaliacaoServico _qualificaAvaliacaoCriticidadeServico;
        private readonly ILogAppServico _logAppService;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public CriterioAvaliacaoController(
            ICriterioAvaliacaoAppServico qualificaAvaliacaoCriticidadeAppServico,
            ICriterioAvaliacaoServico qualificaAvaliacaoCriticidadeServico,
            ILogAppServico logAppService, IUsuarioAppServico usuarioAppServico,
            IProcessoAppServico processoAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppService, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _qualificaAvaliacaoCriticidadeAppServico = qualificaAvaliacaoCriticidadeAppServico;
            _qualificaAvaliacaoCriticidadeServico = qualificaAvaliacaoCriticidadeServico;
            _logAppService = logAppService;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        // GET: CriterioAvaliacao
        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            return View();
        }

     

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult Salvar(CriterioAvaliacao qualificaAvaliacaoCriticidade)
        {
            var erros = new List<string>();

            _qualificaAvaliacaoCriticidadeServico.ValidaCamposCadastro(qualificaAvaliacaoCriticidade, ref erros);

            if (erros.Count == 0)
            {
                _qualificaAvaliacaoCriticidadeAppServico.Add(qualificaAvaliacaoCriticidade);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult Atualizar(CriterioAvaliacao qualificaAvaliacaoCriticidade)
        {
            var erros = new List<string>();

            _qualificaAvaliacaoCriticidadeServico.ValidaCamposCadastro(qualificaAvaliacaoCriticidade, ref erros);

            if (erros.Count == 0)
            {
                qualificaAvaliacaoCriticidade.DtAlteracao = DateTime.Now;

                _qualificaAvaliacaoCriticidadeAppServico.Update(qualificaAvaliacaoCriticidade);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
       // [Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult Excluir(CriterioAvaliacao qualificaAvaliacaoCriticidade)
        {
            var erros = new List<string>();
            _qualificaAvaliacaoCriticidadeServico.ValidaCamposCadastro(qualificaAvaliacaoCriticidade, ref erros);

            if (erros.Count == 0)
            {
                qualificaAvaliacaoCriticidade.DtAlteracao = DateTime.Now;

                _qualificaAvaliacaoCriticidadeAppServico.Remove(qualificaAvaliacaoCriticidade);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListaCriteriosPadroes()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var criterioAvaliacao = new List<CriterioAvaliacao>();
            int idSite = Util.ObterSiteSelecionado();


            criterioAvaliacao.AddRange(_qualificaAvaliacaoCriticidadeAppServico.Get(x => x.Produto.IdSite == idSite).Select(x => new CriterioAvaliacao() { IdCriterioAvaliacao=x.IdCriterioAvaliacao, Titulo = x.Titulo }).ToList());

            criterioAvaliacao = criterioAvaliacao.Distinct().ToList();

            return Json(new { StatusCode = (int)HttpStatusCode.OK, CriterioAvaliacaoPadrao = criterioAvaliacao }, JsonRequestBehavior.AllowGet);
        }
    }
}