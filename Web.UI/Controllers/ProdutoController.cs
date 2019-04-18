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
    [VerificaIntegridadeLogin]
    public class ProdutoController : Controller
    {
        private readonly IProdutoAppServico _produtoAppService;
        private readonly IProdutoServico _produtoServico;
        
        private readonly IAvaliacaoCriticidadeAppServico _avaliacaoAppService;
        private readonly ICriterioAvaliacaoAppServico _criterioAvaliacaoAppService;
        private readonly IAvaliaCriterioAvaliacaoAppServico _avaliaCriterioAvaliacaoAppService;
        private readonly ICriterioQualificacaoAppServico _criterioQualificacaoAppService;


        public ProdutoController(IProdutoAppServico produtoAppService,
            IAvaliacaoCriticidadeAppServico avaliacaoAppService,
            ICriterioAvaliacaoAppServico criterioAvaliacaoAppService,
            ICriterioQualificacaoAppServico criterioQualificacaoAppService,
            IAvaliaCriterioAvaliacaoAppServico avaliaCriterioAvaliacaoAppService,
            IProdutoServico produtoServico)
        {
            _produtoAppService = produtoAppService;
            _avaliacaoAppService = avaliacaoAppService;
            _criterioAvaliacaoAppService = criterioAvaliacaoAppService;
            _criterioQualificacaoAppService = criterioQualificacaoAppService;
            _avaliaCriterioAvaliacaoAppService = avaliaCriterioAvaliacaoAppService;
            _produtoServico = produtoServico;
        }
        // GET: Produto
       // [Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public ActionResult Index()
        {
            var produtoCTx = _produtoAppService.ListaPorSite(5);
            return View(produtoCTx);
        }

        [HttpGet]
       // [Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public ActionResult Criar()
        {
            //ViewBag.ListaAvaliacao = _avaliacaoAppService.ListaPorSite(5);

            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
       // [Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult Criar(Produto produto)
        {
            var erros = new List<string>();

            _produtoServico.ValidarCampos(produto, ref erros);

            if (erros.Count == 0)
            {
                produto.DtCriacao = DateTime.Now;
                produto.DtAlteracao = DateTime.Now;

                _produtoAppService.Add(produto);

                return Json(new { StatusCode = (int)HttpStatusCode.OK });
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest });

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
       // [Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult Atualizar(Produto produto)
        {
            var erros = new List<string>();

            _produtoServico.ValidarCampos(produto, ref erros);

            if (erros.Count == 0)
            {
                produto.DtAlteracao = DateTime.Now;
                _produtoAppService.Update(produto);

                return Json(new { StatusCode = (int)HttpStatusCode.OK });
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest });

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult Excluir(Produto produto)
        {
            _produtoAppService.Remove(produto);
            return Json(new { StatusCode = (int)HttpStatusCode.OK });

        }

        [HttpGet]
       // [Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public ActionResult Editar()
        {
            return View();
        }

        [HttpPost]
       // [Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult AdicionarAvaliacaoCriticidade(int idProduto, List<int> listaAvaliacoesCriticidade)
        {
            _produtoAppService.AdicionarAvaliacaoCriticidade(idProduto, listaAvaliacoesCriticidade);

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
       // [Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult AdicionarCriteriosQualificacao(int idProduto, List<int> listaCriteriosQualificacao)
        {
            _produtoAppService.AdicionarCriteriosQualificacao(idProduto, listaCriteriosQualificacao);

            return Json(new {StatusCode=(int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult AdicionarFornecedores(int idProduto, List<int> listaFornecedores)
        {
            _produtoAppService.AdicionarCriteriosQualificacao(idProduto, listaFornecedores);

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //[Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult QualificaCriterioQualificacao(Produto produto)
        {


            return Json(new {StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
       // [Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public JsonResult AvaliaCriterioAvaliacao(Produto produto)
        {
            var erros = new List<string>();

            _produtoServico.ValidaNotasAvaliacao(produto, ref erros);

            if (erros.Count == 0)
            {
                //_avaliaCriterioAvaliacaoAppService.Salvar(produto.CriteriosAvaliacao);
                
                return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest }, JsonRequestBehavior.AllowGet);
        }

    }
}