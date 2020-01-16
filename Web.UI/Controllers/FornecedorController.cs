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
    //[ProcessoSelecionado]
    [VerificaIntegridadeLogin]
    public class FornecedorController : BaseController
    {
        private readonly ICriterioAvaliacaoAppServico _qualificaAvaliacaoCriticidadeAppServico;
        private readonly IProdutoAppServico _produtoAppServico;
        private readonly IProdutoServico _produtoServico;
        private readonly IAnexoAppServico _anexoAppServico;
        private readonly IArquivosEvidenciaCriterioQualificacaoAppServico _arquivosEvidenciaCriterioQualificacaoAppServico;
        private readonly IFornecedorAppServico _fornecedorAppServico;
        private readonly IFornecedorServico _fornecedorServico;

        private readonly IProdutoFornecedorAppServico _produtoFornecedorAppServico;

        private readonly IAvaliacaoCriticidadeAppServico _avaliacaoCriticidadeAppServico;
        private readonly IAvaliacaoCriticidadeServico _avaliacaoCriticidadeServico;

        private readonly IAvaliaCriterioAvaliacaoAppServico _avaliaCriterioAvaliacaoAppServico;
        private readonly IAvaliaCriterioAvaliacaoServico _avaliaCriterioAvaliacaoServico;


        private readonly ICriterioQualificacaoAppServico _criterioQualificacaoAppServico;
        private readonly IAvaliaCriterioQualificacaoAppServico _avaliaCriterioQualificacaoAppServico;
        private readonly ICriterioQualificacaoServico _criterioQualificacaoServico;

        private readonly ICriterioAvaliacaoAppServico _criterioAvaliacaoAppServico;
        private readonly ICriterioAvaliacaoServico _criterioAvaliacaoServico;


        private readonly ILogAppServico _logAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        private int EditarFornecedor = 79;
        private int EditarProduto = 65;

        public FornecedorController(IFornecedorAppServico fornecedorAppServico,
            IProdutoAppServico produtoAppServico,
            IProdutoFornecedorAppServico produtoFornecedorAppServico,
            IAvaliacaoCriticidadeAppServico avaliacaoCriticidadeAppServico,
            IAvaliaCriterioAvaliacaoAppServico avaliaCriterioAvaliacaoAppServico,
            ICriterioQualificacaoAppServico criterioQualificacaoAppServico,
            ICriterioAvaliacaoAppServico criterioAvaliacaoAppServico,
            ILogAppServico logAppServico,
            IProdutoServico produtoServico,
            IAvaliacaoCriticidadeServico avaliacaoCriticidadeServico,
            IAvaliaCriterioAvaliacaoServico avaliaCriterioAvaliacaoServico,
            IFornecedorServico fornecedorServico,
            ICriterioQualificacaoServico criterioQualificacaoServico,
            IAvaliaCriterioQualificacaoAppServico AvaliaCriterioQualificacaoAppServico,
            ICriterioAvaliacaoServico criterioAvaliacaoServico,
            IUsuarioAppServico usuarioAppServico,
            IProcessoAppServico processoAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico,
            ICriterioAvaliacaoAppServico qualificaAvaliacaoCriticidadeAppServico,
            IAnexoAppServico anexoAppServico,
            IArquivosEvidenciaCriterioQualificacaoAppServico arquivosEvidenciaCriterioQualificacaoAppServico
            ) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _fornecedorAppServico = fornecedorAppServico;
            _arquivosEvidenciaCriterioQualificacaoAppServico = arquivosEvidenciaCriterioQualificacaoAppServico;
            _produtoFornecedorAppServico = produtoFornecedorAppServico;
            _avaliacaoCriticidadeAppServico = avaliacaoCriticidadeAppServico;
            _avaliaCriterioAvaliacaoAppServico = avaliaCriterioAvaliacaoAppServico;
            _avaliacaoCriticidadeServico = avaliacaoCriticidadeServico;
            _criterioQualificacaoAppServico = criterioQualificacaoAppServico;
            _criterioAvaliacaoAppServico = criterioAvaliacaoAppServico;
            _avaliaCriterioQualificacaoAppServico = AvaliaCriterioQualificacaoAppServico;
            _produtoAppServico = produtoAppServico;
            _logAppServico = logAppServico;
            _produtoServico = produtoServico;
            _avaliaCriterioAvaliacaoServico = avaliaCriterioAvaliacaoServico;
            _fornecedorServico = fornecedorServico;
            _criterioQualificacaoServico = criterioQualificacaoServico;
            _criterioAvaliacaoServico = criterioAvaliacaoServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _qualificaAvaliacaoCriticidadeAppServico = qualificaAvaliacaoCriticidadeAppServico;
            _anexoAppServico = anexoAppServico;
        }


        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            return RedirectToAction("Produtos");
        }

        public ActionResult Produtos()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var idSite = Util.ObterSiteSelecionado();

            ViewBag.idFuncao = EditarProduto;

            var lista = _produtoAppServico.ListaPorSite(idSite);

            return View("IndexProdutos", lista);
        }

        public ActionResult AcoesProdutos(int? id, string Ancora = "")
        {
            var produto = new Produto();

            ViewBag.idFuncao = EditarProduto;
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            int idSite = Util.ObterSiteSelecionado();
            //ViewBag.IdProcesso = Util.ObterProcessoSelecionado();
            ViewBag.IdProduto = id.HasValue ? id.Value.ToString() : "";
            ViewBag.Ancora = Ancora;

            if (id != null)
            {
                produto = _produtoAppServico.GetById(id.Value);
                ViewBag.IdSite = produto.IdSite;
                //ViewBag.IdProduto = produto.IdProduto;
            }

            var avaliacoesCriticidade = _avaliacaoCriticidadeAppServico.Get(x => x.Produto.IdSite == idSite).Select(x => new AvaliacaoCriticidade() { IdAvaliacaoCriticidade = x.IdAvaliacaoCriticidade, Titulo = x.Titulo }).ToList();

            if (avaliacoesCriticidade.Count() > 0)
            {
                foreach (var item in avaliacoesCriticidade)
                {
                    if (!produto.AvaliacoesCriticidade.Select(x => x.Titulo).Contains(item.Titulo))
                    {
                        item.IdAvaliacaoCriticidade = 0;
                        item.Ativo = false;
                        produto.AvaliacoesCriticidade.Add(item);
                    }
                }
            }
            else
            {

                produto.AvaliacoesCriticidade.Add(new AvaliacaoCriticidade
                {
                    Titulo = "Este fornecedor tem impacto direto no meu produto/ serviços?",
                    IdAvaliacaoCriticidade = 0,
                    Ativo = false,
                });

                produto.AvaliacoesCriticidade.Add(new AvaliacaoCriticidade
                {
                    Titulo = "Este fornecedor paralisa minhas atividades?",
                    IdAvaliacaoCriticidade = 0,
                    Ativo = false,
                });

                produto.AvaliacoesCriticidade.Add(new AvaliacaoCriticidade
                {
                    Titulo = "Este produto/serviço compromete o nome ou imagem de minha empresa?",
                    IdAvaliacaoCriticidade = 0,
                    Ativo = false,
                });

                produto.AvaliacoesCriticidade.Add(new AvaliacaoCriticidade
                {
                    Titulo = "A falta ou ausência deste fornecedor no mercado compromete meu trabalho?",
                    IdAvaliacaoCriticidade = 0,
                    Ativo = false,
                });

                produto.AvaliacoesCriticidade.Add(new AvaliacaoCriticidade
                {
                    Titulo = "O produto/serviço tem impacto legal?",
                    IdAvaliacaoCriticidade = 0,
                    Ativo = false,
                });
            }

            //// Adiciona Qualificação disponivel no mesmo site
            //var avaliacoesQualidade = _criterioQualificacaoAppServico.Get(x => x.Produto.IdSite == idSite).Select(x => new CriterioQualificacao() { IdCriterioQualificacao = x.IdCriterioQualificacao, Titulo = x.Titulo }).ToList();

            //foreach (var item in avaliacoesQualidade)
            //{
            //    if (!produto.CriteriosQualificacao.Select(x => x.Titulo).Contains(item.Titulo))
            //    {
            //        item.IdCriterioQualificacao = 0;
            //        item.Ativo = false;
            //        produto.CriteriosQualificacao.Add(item);
            //    }
            //}


            //// Adiciona Critério de Avaliação disponivel no mesmo site
            //var criteriosAvaliacao = _qualificaAvaliacaoCriticidadeAppServico.Get(x => x.Produto.IdSite == idSite).Select(x => new CriterioAvaliacao() { IdCriterioAvaliacao = x.IdCriterioAvaliacao, Titulo = x.Titulo }).ToList();
            //foreach (var item in criteriosAvaliacao)
            //{
            //    if (!produto.CriteriosAvaliacao.Select(x => x.Titulo).Contains(item.Titulo))
            //    {
            //        item.IdCriterioAvaliacao = 0;
            //        item.Ativo = false;
            //        produto.CriteriosAvaliacao.Add(item);
            //    }
            //}

            return View(produto);
        }

        [HttpPost]
        public JsonResult SalvaProdutos(Produto produto)
        {
            var erros = new List<string>();

            try
            {
                produto.IdSite = Util.ObterSiteSelecionado();
                _produtoServico.ValidarCampos(produto, ref erros);

                if (erros.Count == 0)
                {
                    _produtoAppServico.Add(produto);
                    return Json(new { StatusCode = (int)HttpStatusCode.OK, IdProduto = produto.IdProduto }, JsonRequestBehavior.AllowGet);
                    //return View("IndexProdutos");
                }

            }
            catch (Exception ex)
            {

                GravaLog(ex);
            }
            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
            //return View("IndexProdutos");

        }

        [HttpPost]
        public JsonResult SalvaAvaliacoesCriticidadeCriteriosQualificaoCriterioAvaliacao(Produto produto)
        {
            int idSite = Util.ObterSiteSelecionado();

            var erros = new List<string>();
            try
            {
                produto.AvaliacoesCriticidade.ToList().ForEach(avaliacaoCriticidade =>
                {
                    _avaliacaoCriticidadeServico.ValidaCampos(avaliacaoCriticidade, ref erros);

                    if (avaliacaoCriticidade.Ativo)
                    {
                        produto.Status = 1;
                    }
                });

                produto.CriteriosQualificacao.ToList().ForEach(criterioQualificacao =>
                {
                    _criterioQualificacaoServico.ValidaCampos(criterioQualificacao, ref erros);
                });

                produto.CriteriosAvaliacao.ToList().ForEach(CriterioAvaliacao =>
                {
                    _criterioAvaliacaoServico.ValidaCamposCadastro(CriterioAvaliacao, ref erros);
                });


                _produtoServico.ValidarCampos(produto, ref erros);

                if (erros.Count == 0)
                {

                    produto.AvaliacoesCriticidade.ToList().ForEach(avaliacaoCriticidade =>
                    {
                        var avaliacao = _avaliacaoCriticidadeAppServico.Get(x => x.Titulo == avaliacaoCriticidade.Titulo && x.Produto.IdSite == idSite && x.IdProduto == produto.IdProduto).FirstOrDefault();
                        if (avaliacao == null)
                        {
                            _avaliacaoCriticidadeAppServico.Add(avaliacaoCriticidade);
                        }
                        else
                        {
                            avaliacao.Titulo = avaliacaoCriticidade.Titulo;
                            avaliacao.Ativo = avaliacaoCriticidade.Ativo;
                            _avaliacaoCriticidadeAppServico.Update(avaliacao);

                        }

                    });

                    produto.CriteriosQualificacao.ToList().ForEach(criteriosQualificacao =>
                    {
                        var qualificacao = _criterioQualificacaoAppServico.Get(x => x.Titulo == criteriosQualificacao.Titulo && x.Produto.IdSite == idSite && x.IdProduto == produto.IdProduto).FirstOrDefault();
                        if (qualificacao == null)
                        {
                            _criterioQualificacaoAppServico.Add(criteriosQualificacao);
                        }
                        else
                        {
                            qualificacao.Ativo = criteriosQualificacao.Ativo;
                            qualificacao.Titulo = criteriosQualificacao.Titulo;
                            qualificacao.TemControleVencimento = criteriosQualificacao.TemControleVencimento;
                            _criterioQualificacaoAppServico.Update(qualificacao);
                        }
                    });

                    produto.CriteriosAvaliacao.ToList().ForEach(criteriosAvaliacao =>
                    {
                        var avaliacao = _criterioAvaliacaoAppServico.Get(x => x.Titulo == criteriosAvaliacao.Titulo && x.Produto.IdSite == idSite && x.IdProduto == produto.IdProduto).FirstOrDefault();
                        if (avaliacao == null)
                        {
                            _criterioAvaliacaoAppServico.Add(criteriosAvaliacao);
                        }
                        else
                        {
                            avaliacao.Titulo = criteriosAvaliacao.Titulo;
                            avaliacao.Ativo = criteriosAvaliacao.Ativo;
                            _criterioAvaliacaoAppServico.Update(avaliacao);
                        }
                    });

                    var produtoAtualiza = _produtoAppServico.GetById(produto.IdProduto);

                    produtoAtualiza.Status = produto.Status;
                    produtoAtualiza.MinAprovado = produto.MinAprovado;
                    produtoAtualiza.MaxAprovado = produto.MaxAprovado;
                    produtoAtualiza.MinAprovadoAnalise = produto.MinAprovadoAnalise;
                    produtoAtualiza.MaxAprovadoAnalise = produto.MaxAprovadoAnalise;
                    produtoAtualiza.MinReprovado = produto.MinReprovado;
                    produtoAtualiza.MaxReprovado = produto.MaxReprovado;
                    _produtoAppServico.Update(produtoAtualiza);

                    return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("Produtos");
                }

            }
            catch (Exception ex)
            {

                GravaLog(ex);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Produtos");
        }

        public JsonResult ExcluirAvaliacoesCriticidade(int id)
        {
            List<string> erros = new List<string>();

            try
            {
                var avaliacaoCriticidade = _avaliacaoCriticidadeAppServico.GetById(id);
                _avaliacaoCriticidadeAppServico.Remove(avaliacaoCriticidade);

                return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Resource.SucessoAoExcluirORegistro }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                erros.Add(Traducao.Resource.ParaExcluirEsseRegistroVoceDeveExcluirNoCriterioOriginal);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExcluirCriterioFormQualificacao(int id)
        {
            List<string> erros = new List<string>();

            try
            {
                var criteriosQualificacao = _criterioQualificacaoAppServico.GetById(id);
                _criterioQualificacaoAppServico.Remove(criteriosQualificacao);

                return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Resource.SucessoAoExcluirORegistro }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                erros.Add(Traducao.Resource.ParaExcluirEsseRegistroVoceDeveExcluirNoCriterioOriginal);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExcluirCriterioFormAvaliacao(int id)
        {
            List<string> erros = new List<string>();

            try
            {
                var criteriosAvaliacao = _criterioAvaliacaoAppServico.GetById(id);
                _criterioAvaliacaoAppServico.Remove(criteriosAvaliacao);

                return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Resource.SucessoAoExcluirORegistro }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                erros.Add(Traducao.Resource.ParaExcluirEsseRegistroVoceDeveExcluirNoCriterioOriginal);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarAvaliacaoCriticidade(int id, string valor)
        {
            List<string> erros = new List<string>();

            try
            {
                var avaliacaoCriticidade = _avaliacaoCriticidadeAppServico.GetById(id);
                avaliacaoCriticidade.Titulo = valor;
                _avaliacaoCriticidadeAppServico.Update(avaliacaoCriticidade);

                return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Resource.SucessoAoSalvarORegistro }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                erros.Add(ex.Message);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarCriterioQualificacaoFornecedor(int id, string valor)
        {
            List<string> erros = new List<string>();

            try
            {
                var criterioQualificacao = _criterioQualificacaoAppServico.GetById(id);
                criterioQualificacao.Titulo = valor;
                _criterioQualificacaoAppServico.Update(criterioQualificacao);

                return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Resource.SucessoAoSalvarORegistro }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                erros.Add(ex.Message);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarCriterioAvaliacao(int id, string valor)
        {
            List<string> erros = new List<string>();

            try
            {
                var criteriosAvaliacao = _criterioAvaliacaoAppServico.GetById(id);
                criteriosAvaliacao.Titulo = valor;
                _criterioAvaliacaoAppServico.Update(criteriosAvaliacao);

                return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Resource.SucessoAoSalvarORegistro }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                erros.Add(ex.Message);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult IndexFornecedores(int idProduto)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var produto = _produtoAppServico.GetById(idProduto);
            ViewBag.IdProduto = idProduto;

            return View(produto);
        }

        public ActionResult AcoesFornecedores(int? id, int idProduto, string Ancora = "")
        {
            var produto = new Produto();
            var fornecedor = new Fornecedor();
            ViewBag.idFuncao = EditarFornecedor;
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.Ancora = Ancora;
            ViewBag.IdProduto = idProduto;

            produto = _produtoAppServico.GetById(idProduto);
            ViewBag.IdSite = produto.IdSite;

            if (id != null)
            {
                fornecedor = _fornecedorAppServico.GetById(id.Value);
                ViewBag.avaliacao = _avaliaCriterioAvaliacaoAppServico.Get().Where(x => fornecedor.IdFornecedor == x.IdFornecedor).ToList();
            }
            else
            {
                ViewBag.avaliacao = new List<AvaliaCriterioAvaliacao>();
            }

            var idscriteriosQualificacaoAtual = fornecedor.AvaliaCriteriosQualificacao.Select(avaliacaoCriterioQualificacao => avaliacaoCriterioQualificacao.CriterioQualificacao.IdCriterioQualificacao).ToList();
            var criteriosQualificacaoAtual = fornecedor.AvaliaCriteriosQualificacao.ToList();
            var idscriteriosQualificacao = _criterioQualificacaoAppServico.Get(x => x.IdProduto == produto.IdProduto).Select(x => x.IdCriterioQualificacao).ToList();
            if (fornecedor.AvaliaCriteriosQualificacao == null || fornecedor.AvaliaCriteriosQualificacao.Count == 0 || idscriteriosQualificacao.Count != idscriteriosQualificacaoAtual.Count)
            {
                var criteriosQualificacao = _criterioQualificacaoAppServico.Get(x => x.IdProduto == produto.IdProduto).Where(x => !idscriteriosQualificacaoAtual.Contains(x.IdCriterioQualificacao)).ToList();

                criteriosQualificacao.ForEach(criterioQualificacao =>
                {
                    if (criterioQualificacao.Ativo)
                    {
                        AvaliaCriterioQualificacao avaliaCriterioQualificacao = new AvaliaCriterioQualificacao();
                        avaliaCriterioQualificacao.CriterioQualificacao = criterioQualificacao;
                        fornecedor.AvaliaCriteriosQualificacao.Add(avaliaCriterioQualificacao);
                    }
                });
            }

            criteriosQualificacaoAtual.ForEach(criterioQualificacao =>
            {

                var criterioQualificacaoExcluir = _criterioQualificacaoAppServico.Get(x => x.IdProduto == produto.IdProduto && x.IdCriterioQualificacao == criterioQualificacao.IdCriterioQualificacao).FirstOrDefault();

                if (criterioQualificacaoExcluir.Ativo == false)
                {
                    fornecedor.AvaliaCriteriosQualificacao.Remove(criterioQualificacao);
                }

            });

            if (fornecedor.AvaliaCriteriosAvaliacao == null || fornecedor.AvaliaCriteriosAvaliacao.Count == 0)
            {

                AvaliaCriterioAvaliacao avaliaCriterioAvaliacao = new AvaliaCriterioAvaliacao();
                _criterioAvaliacaoAppServico.Get(x => x.IdProduto == produto.IdProduto).ToList().ForEach(criterioAvaliacao =>
                {

                    avaliaCriterioAvaliacao.CriterioAvaliacao = criterioAvaliacao;
                    fornecedor.AvaliaCriteriosAvaliacao.Add(avaliaCriterioAvaliacao);
                });

            }
            ViewBag.Produto = produto;

            return View(fornecedor);
        }

        [HttpPost]
        public JsonResult SalvaFornecedor(Fornecedor fornecedor)
        {
            var idProduto = fornecedor.Produtos.FirstOrDefault().IdProduto; //ViewBag.IdProduto;
            var erros = new List<string>();

            try
            {
                _fornecedorServico.ValidaCampos(fornecedor, ref erros);

                if (erros.Count == 0)
                {
                    _fornecedorAppServico.Add(fornecedor);
                    return Json(new { StatusCode = (int)HttpStatusCode.OK, IdFornecedor = fornecedor.IdFornecedor, Success = Traducao.Fornecedores.ResourceFornecedores.FornecedorSalvoComSucesso }, JsonRequestBehavior.AllowGet);
                    //return View("IndexFornecedores", idProduto);
                    //return RedirectToAction("IndexFornecedores");

                }

            }
            catch (Exception ex)
            {
                GravaLog(ex);
            }
            //return View("IndexFornecedores", idProduto);
            //return RedirectToAction("IndexFornecedores");
            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvaQualificacoes(List<AvaliaCriterioQualificacao> criteriosQualificacao)
        {
            var erros = new List<string>();
            try
            {

                criteriosQualificacao.ForEach(criterioQualificacao =>
                {
                    var temControleVencimento = _criterioQualificacaoAppServico.GetById(criterioQualificacao.IdCriterioQualificacao).TemControleVencimento;

                    if (temControleVencimento)
                        _criterioQualificacaoServico.ValidaCamposQualificacao(criterioQualificacao, ref erros);
                    else
                        _criterioQualificacaoServico.ValidaCamposQualificacaoSemControleVencimento(criterioQualificacao, ref erros);
                });

                if (erros.Count == 0)
                {
                    criteriosQualificacao.ForEach(criterioQualificacao =>
                    {


                        criterioQualificacao.ArquivosDeEvidenciaAux.ForEach(anexo =>
                        {
                            anexo.Tratar();
                            if (anexo.IdAnexo == 0 && criterioQualificacao.IdAvaliaCriterioQualificacao == 0)
                            {
                                criterioQualificacao.ArquivosEvidenciaCriterioQualificacao.Add(new ArquivosEvidenciaCriterioQualificacao
                                {
                                    Anexo = anexo,
                                    AvaliaCriterioQualificacao = criterioQualificacao
                                });
                            }
                            else if (anexo.IdAnexo == 0 && criterioQualificacao.IdAvaliaCriterioQualificacao > 0)
                            {
                                var anexoCtx = _arquivosEvidenciaCriterioQualificacaoAppServico.Get(x => x.Anexo.Nome == anexo.Nome && x.IdAvaliaCriterioQualificacao == criterioQualificacao.IdAvaliaCriterioQualificacao).FirstOrDefault();

                                if (anexoCtx == null)
                                {
                                    criterioQualificacao.ArquivosEvidenciaCriterioQualificacao.Add(new ArquivosEvidenciaCriterioQualificacao
                                    {
                                        Anexo = anexo,
                                        IdAvaliaCriterioQualificacao = criterioQualificacao.IdAvaliaCriterioQualificacao,
                                    });

                                    criterioQualificacao.ArquivosEvidenciaCriterioQualificacao.ForEach(x =>
                                    {
                                        _arquivosEvidenciaCriterioQualificacaoAppServico.Add(x);
                                    });
                                }

                            }

                        });

                        if (criterioQualificacao.IdAvaliaCriterioQualificacao == 0)
                        {
                            //criterioQualificacao.CriterioQualificacao = _criterioQualificacaoAppServico.GetById(criterioQualificacao.IdCriterioQualificacao);
                            _avaliaCriterioQualificacaoAppServico.Add(criterioQualificacao);
                        }
                        else
                            _avaliaCriterioQualificacaoAppServico.Update(criterioQualificacao);
                    });

                    return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                GravaLog(ex);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvaAvaliacoes(List<AvaliaCriterioAvaliacao> avaliacoes)
        {

            var guidAvaliacao = Guid.NewGuid(); 

            var erros = new List<string>();
            try
            {
                var primeiraAvaliacao = avaliacoes.FirstOrDefault();

                if (primeiraAvaliacao != null && primeiraAvaliacao.IdUsuarioAvaliacao > 0)
                {
                    var fornecedor = _fornecedorAppServico.GetById(primeiraAvaliacao.IdFornecedor);
                    fornecedor.IdUsuarioAvaliacao = primeiraAvaliacao.IdUsuarioAvaliacao;
                    _fornecedorAppServico.Update(fornecedor);
                }
                else
                {
                    erros.Add(Traducao.Resource.MsgResposavelObrigatorio);
                }

                
                avaliacoes.ForEach(avaliaCriterioAvaliacao =>
                {
                    avaliaCriterioAvaliacao.DtAvaliacao = DateTime.Now;
                    avaliaCriterioAvaliacao.GuidAvaliacao = guidAvaliacao.ToString();

                    if (avaliaCriterioAvaliacao.NotaAvaliacao == null)
                        avaliaCriterioAvaliacao.NotaAvaliacao = new int();
                    _avaliaCriterioAvaliacaoServico.ValidaAvaliaCriterioAvaliacao(avaliaCriterioAvaliacao, ref erros);
                });



                if (erros.Count == 0)
                {

                    _avaliaCriterioAvaliacaoAppServico.Salvar(avaliacoes);


                    return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ObterPorId(int idFornecedor)
        {
            var fornecedor = _fornecedorAppServico.GetById(idFornecedor);
            if (fornecedor != null)
            {
                return Json(new { StatusCode = (int)HttpStatusCode.OK, Fornecedor = fornecedor }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erro = Traducao.Resource.Fornecedor_msg_erro_IdFornecedor_invalido }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Atualizar(Fornecedor fornecedor)
        {
            var erros = new List<string>();

            _fornecedorServico.ValidaCampos(fornecedor, ref erros);

            if (erros.Count == 0)
            {
                _fornecedorAppServico.Update(fornecedor);

                return Json(new { StatusCode = (int)HttpStatusCode.OK, Fornecedor = fornecedor }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Salvar(Fornecedor fornecedor)
        {
            var erros = new List<string>();

            _fornecedorServico.ValidaCampos(fornecedor, ref erros);

            if (erros.Count == 0)
            {
                _fornecedorAppServico.Add(fornecedor);

                return Json(new { StatusCode = (int)HttpStatusCode.OK, Fornecedor = fornecedor }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExcluirProduto(int idProduto)
        {

            try
            {
                _produtoAppServico.Excluir(idProduto);
                return Json(new { Success = Traducao.Resource.SucessoAoExcluirORegistro, StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Excluir(int idFornecedor)
        {
            var fornecedor = _fornecedorAppServico.GetById(idFornecedor);

            if (fornecedor != null)
            {
                var avaliacoes = _avaliaCriterioAvaliacaoAppServico.Get(x => x.IdFornecedor == idFornecedor).ToList();

                foreach (var item in avaliacoes)
                {
                    _avaliaCriterioAvaliacaoAppServico.Remove(item);
                }

                var qualificacoes = _avaliaCriterioQualificacaoAppServico.Get(x => x.IdFornecedor == idFornecedor).ToList();

                foreach (var item in qualificacoes)
                {
                    _avaliaCriterioQualificacaoAppServico.Remove(item);
                }

                _fornecedorAppServico.Remove(fornecedor);

                return Json(new { Success = Traducao.Resource.SucessoAoExcluirORegistro, StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erro = Traducao.Resource.Fornecedor_msg_erro_IdFornecedor_invalido }, JsonRequestBehavior.AllowGet);

        }
    }
}