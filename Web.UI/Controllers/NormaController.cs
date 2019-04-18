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
    [RouteArea("Auditoria")]
    [VerificaIntegridadeLogin]
    public class NormaController : BaseController
    {
        private readonly INormaAppServico _normaAppServico;
        private readonly INormaServico _normaServico;
        private readonly IPlaiProcessoNormaAppServico _plaiProcessoNormaAppServico;

        private readonly ILogAppServico _logAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public NormaController(INormaAppServico normaAppServico, ILogAppServico logAppServico,
                               INormaServico normaServico,
                               IUsuarioAppServico usuarioAppServico,
                               IProcessoAppServico processoAppServico,
                               IPlaiProcessoNormaAppServico plaiProcessoNormaAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _normaAppServico = normaAppServico;
            _logAppServico = logAppServico;
            _normaServico = normaServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _plaiProcessoNormaAppServico = plaiProcessoNormaAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        public JsonResult Salvar(Norma norma)
        {
            
            var erros = new List<string>();

            try
            {
                
                _normaServico.Valido(norma, ref erros);

                if (erros.Count == 0)
                {
                    if (norma.IdNorma != 0)
                    {
                        var normaUpdate = _normaAppServico.GetById(norma.IdNorma);

                        normaUpdate.IdSite = Util.ObterSiteSelecionado();
                        normaUpdate.DataAlteracao = DateTime.Now;
                        normaUpdate.Titulo = norma.Titulo;
                        normaUpdate.Codigo = norma.Codigo;
                        _normaAppServico.Update(normaUpdate);
                    }
                    else
                    {
                        norma.IdSite = Util.ObterSiteSelecionado();

                        int numeroNorma = _normaAppServico.Get(x => x.IdSite == norma.IdSite && norma.Numero != null).Select(x => x.Numero.Value).LastOrDefault() + 1;

                        norma.IdUsuarioIncluiu = Convert.ToInt32(Util.ObterUsuario().IdUsuario);
                        norma.Ativo = true;
                        norma.Numero = numeroNorma;
                        norma.DataCadastro = DateTime.Now;

                        _normaAppServico.Add(norma);
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

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Norma.ResourceNorma.Norma_msg_save_valid }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult Criar(int? id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var norma = new Norma();

            if (id != null)
            {
                norma = _normaAppServico.GetById(Convert.ToInt32(id));
            }

            ViewBag.IdSite = 1;

            return View(norma);
        }

        public ActionResult Excluir(int Id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var erros = new List<string>();

            try
            {
                var ObjNorma = _normaAppServico.GetById(Id);

                _plaiProcessoNormaAppServico.Get(x => x.IdNorma == Id).ToList().ForEach(x =>
                {
                    _plaiProcessoNormaAppServico.Remove(x);
                });

                _normaAppServico.Remove(ObjNorma);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Norma.ResourceNorma.Norma_msg_del_valid }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Editar(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var norma = _normaAppServico.GetById(id);

            return View(norma);
        }

        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var idSiteSelecionado = Util.ObterSiteSelecionado();
            var normas = _normaAppServico.Get(x => x.IdSite == idSiteSelecionado);

            return View(normas);
        }

        public ActionResult PDF(int id)
        {
            var normaAppServico = _normaAppServico.GetById(id);

            return View(normaAppServico);
        }
        
        public ActionResult SalvaPDF(int id)
        {
            GeraArquivoZip(ControllerContext, "PDF", id);

            return View();
        }

        [HttpPost]
        //[AcessoUsuarioSite]
        public JsonResult AtivaInativa(Norma norma)
        {

            var erros = new List<string>();

            var resposta = _normaAppServico.AtivarInativar(norma.IdNorma);

            if (!resposta)
            {
                return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Norma.ResourceNorma.Norma_msg_icone_ativo_valid }, JsonRequestBehavior.AllowGet);
        }

    }
}