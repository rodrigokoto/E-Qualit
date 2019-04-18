using ApplicationService.Interface;
using Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.UI.Helpers;

namespace Web.UI.Controllers
{
    //[ProcessoSelecionado]
    [VerificaIntegridadeLogin]
    public class AvaliacaoCriticidadeController : BaseController
    {
        private readonly IAvaliacaoCriticidadeAppServico _avaliacaoCriticidadeAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public AvaliacaoCriticidadeController(IAvaliacaoCriticidadeAppServico avaliacaoCriticidadeAppServico, 
                                              ILogAppServico logAppServico,
                                              IUsuarioAppServico usuarioAppServico,
                                              IProcessoAppServico processoAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _avaliacaoCriticidadeAppServico = avaliacaoCriticidadeAppServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        [HttpGet]
        public ActionResult Index(int idSite)
        {
            // var listaAvaliacoes = _avaliacaoCriticidadeAppServico.ListaPorSite(idSite);
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            return View();

        }

        [HttpGet]
        public JsonResult ListaCriteriosPadroes()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var avaliacoesCriticidade = new List<AvaliacaoCriticidade>();
            int idSite = Util.ObterSiteSelecionado();

            avaliacoesCriticidade.AddRange(_avaliacaoCriticidadeAppServico.Get(x => x.Produto.IdSite == idSite).Select(x => new AvaliacaoCriticidade() { IdAvaliacaoCriticidade=x.IdAvaliacaoCriticidade, Titulo = x.Titulo}) .ToList());

            if(avaliacoesCriticidade.Count == 0)
            {
                avaliacoesCriticidade.Add(new AvaliacaoCriticidade
                {
                    Titulo = Traducao.Resource.QstFornecedorImpacto,
                });
                avaliacoesCriticidade.Add(new AvaliacaoCriticidade
                {
                    Titulo = Traducao.Resource.QstFornecedorAtividade,
                });
                avaliacoesCriticidade.Add(new AvaliacaoCriticidade
                {
                    Titulo = Traducao.Resource.QstFornecedorCompromete,
                });
                avaliacoesCriticidade.Add(new AvaliacaoCriticidade
                {
                    Titulo = Traducao.Resource.QstFaltaCompromete,
                });
                avaliacoesCriticidade.Add(new AvaliacaoCriticidade
                {
                    Titulo = Traducao.Resource.QstQualidadeImpacta,
                });
                avaliacoesCriticidade.Add(new AvaliacaoCriticidade
                {
                    Titulo = Traducao.Resource.QstImpactoLegal,
                });
            }

            avaliacoesCriticidade = avaliacoesCriticidade.Distinct().ToList();

            return Json(new {StatusCode = (int)HttpStatusCode.OK, AvaliacoesCriticidadePadrao = avaliacoesCriticidade }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObterPorId(int id)
        {
            var avaliacaoCriticidade = _avaliacaoCriticidadeAppServico.GetById(id);

            return Json(new { StatusCode = (int)HttpStatusCode.OK, AvaliacaoCriticidade = avaliacaoCriticidade.Titulo }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Salvar(AvaliacaoCriticidade avaliacaoCriticidade)
        {
            avaliacaoCriticidade.DtAlteracao = DateTime.Now;
            avaliacaoCriticidade.DtCriacao = DateTime.Now;
            _avaliacaoCriticidadeAppServico.Add(avaliacaoCriticidade);

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Atualizar(AvaliacaoCriticidade avaliacaoCriticidade)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var ctx = _avaliacaoCriticidadeAppServico.GetById(avaliacaoCriticidade.IdAvaliacaoCriticidade);

            if (ctx != null)
            {
                ctx.DtAlteracao = DateTime.Now;
                ctx.Titulo = avaliacaoCriticidade.Titulo;
                _avaliacaoCriticidadeAppServico.Update(ctx);

                return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadGateway }, JsonRequestBehavior.AllowGet);

        }

    }
}