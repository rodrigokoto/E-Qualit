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
    [VerificaIntegridadeLogin]
    public class ControladorCategoriasController : BaseController
    {
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        

        public ControladorCategoriasController(IControladorCategoriasAppServico controladorCategoriasServico,
                                                ILogAppServico logAppServico,
                                                IUsuarioAppServico usuarioAppServico,
                                                IPendenciaAppServico pendenciaAppServico,
                                                IProcessoAppServico processoAppServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico, pendenciaAppServico)
        {
            _controladorCategoriasServico = controladorCategoriasServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
        }

        // GET: ControladorCategorias
        //[Autorizacao(Perfis = "Administrador")]
        //[Autorizacao(Perfis = new PerfisAcesso[] { PerfisAcesso.Administrador, PerfisAcesso.Coordenador, PerfisAcesso.Suporte })]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastro(string tipo, int site)
        {

            var listaControladorCadastro = new List<ControladorCategoria>();
            listaControladorCadastro = _controladorCategoriasServico.ListaGetByTipoAndSite(tipo, site);

            ViewBag.TipoCadastro = listaControladorCadastro.Select(x => x.TipoTabela).FirstOrDefault();
            ViewBag.IdSite = listaControladorCadastro.Select(x => x.IdSite).FirstOrDefault();

            if (base.Request.IsAjaxRequest())
                return PartialView(listaControladorCadastro);
            else
                return View(listaControladorCadastro);

        }

        [HttpGet]
        public ActionResult AtualizaTabela(string tipo, int site)
        {
            var listaControladorCadastro = new List<ControladorCategoria>();
            listaControladorCadastro = _controladorCategoriasServico.ListaGetByTipoAndSite(tipo, site);

            ViewBag.TipoCadastro = listaControladorCadastro.Select(x => x.TipoTabela).FirstOrDefault();
            ViewBag.IdSite = listaControladorCadastro.Select(x => x.IdSite).FirstOrDefault();

            return PartialView("Cadastro", listaControladorCadastro);
        }

        [HttpGet]
        public JsonResult ListaAtivos(string tipo, int site)
        {
            var listaAtivos = _controladorCategoriasServico.ListaAtivos(tipo, site);

            if (tipo == "tnc")
            {
                if (!listaAtivos.Select(x => x.Descricao).Contains("Auditoria"))
                {
                    _controladorCategoriasServico.Add(new ControladorCategoria
                    {
                        IdSite = site,
                        Descricao = "Auditoria",
                        TipoTabela = tipo,
                        Ativo = true
                    });

                    listaAtivos = _controladorCategoriasServico.ListaAtivos(tipo, site);
                }
            }



            if (listaAtivos != null && listaAtivos.Count > 0)
            {
                return Json(new { StatusCode = (int)HttpStatusCode.Accepted, Lista = listaAtivos }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Salvar(ControladorCategoria controladorCategorias)
        {
            var erros = new List<string>();
            try
            {
                _controladorCategoriasServico.SalvarCadastro(controladorCategorias, ref erros);

                if (erros.Count > 0)
                {
                    return Json(new { Erros = erros });
                }
                return Json(new { Success = true });
            }
            catch (Exception ex)
            {
                GravaLog(ex);

                erros.Add(Traducao.Resource.MsgErroContatoADM);
                return Json(new
                {
                    StatusCode = 500,
                    Erro = erros
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Excluir(int idControladorCategorias)
        {
            ControladorCategoria cadasto = _controladorCategoriasServico.GetByIdAsNoTracking(idControladorCategorias);

            if (cadasto != null)
            {
                _controladorCategoriasServico.Remove(cadasto);
            }

            //if (!service.Sucesso)
            //{
            //    Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    return Json(new { Success = false, Message = service.Error.Message });
            //}
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AtivarDesativa(int idControladorCategorias)
        {
            ControladorCategoria cadasto = _controladorCategoriasServico.GetById(idControladorCategorias);
            cadasto.Ativo = cadasto.Ativo == true ? cadasto.Ativo = false : cadasto.Ativo = true;

            _controladorCategoriasServico.Update(cadasto);

            cadasto.Site = null;

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

    }
}
