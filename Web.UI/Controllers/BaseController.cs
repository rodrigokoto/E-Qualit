using ApplicationService.Interface;
using DAL.Context;
using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using Dominio.Servico;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Web.UI.Helpers;
using Web.UI.Models;

namespace Web.UI.Controllers
{
    [EditarPossuiAcessoSite]
    public class BaseController : Controller
    {
        private string controller;
        private string action;
        public string lingua;
        private readonly ILogAppServico _logServico;
        private readonly ISiteRepositorio _siteRepositorio;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IPendenciaAppServico _pendenciaAppServico;
        


        public BaseController(ILogAppServico logServico, IUsuarioAppServico usuarioAppServico, IProcessoAppServico processoAppServico, IControladorCategoriasAppServico controladorCategoriasServico, IPendenciaAppServico pedenciaAppServico)
        {
            _logServico = logServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _pendenciaAppServico = pedenciaAppServico;


            Usuario usuarioLogadoBase = new Usuario();
            int idUsuario = 0;
            int idSite = 0;
            DateTime LastDay = DateTime.Now.AddDays(-1);

            try
            {
                idUsuario = Util.ObterCodigoUsuarioLogado();
            }
            catch
            {
            }

            try
            {
                idSite = Util.ObterSiteSelecionado();
            }
            catch
            {
            }

            try
            {

                ViewBag.CodClienteSelecionado = Util.ObterClienteSelecionado();
            }
            catch
            {
                ViewBag.CodClienteSelecionado = 0;
            }


            try
            {
                using (var db = new BaseContext())
                {
                    try
                    {

                        if (_usuarioAppServico != null)
                        {
                            usuarioLogadoBase = _usuarioAppServico.GetById(idUsuario);
                        }

                        if (usuarioLogadoBase != null)
                        {

                            ViewBag.UsuarioLogadoBase = usuarioLogadoBase;

                            if (usuarioLogadoBase.IdPerfil == 1 || usuarioLogadoBase.IdPerfil == 3)
                            {
                                List<int> usuariocargos = usuarioLogadoBase.UsuarioCargoes.Select(x => x.IdCargo).Distinct().ToList();
                                var result = (from pr in db.Processo
                                              join d in db.DocDocumento on pr.IdProcesso equals d.IdProcesso
                                              join dc in db.DocumentoCargo on d.IdDocumento equals dc.IdDocumento
                                              join c in db.ControladorCategoria on d.IdCategoria equals c.IdControladorCategorias
                                              where d.FlStatus == 3 && d.IdSite == idSite
                                              select new MenuProcessoViewModel
                                              {

                                                  IdProcesso = pr.IdProcesso,
                                                  Nome = pr.Nome,
                                                  Descricao = c.Descricao,
                                                  IdCategoria = c.IdControladorCategorias

                                              }).Distinct().ToList().GroupBy(x => x.Nome);

                                ViewData["Menu"] = result;
                                ViewBag.IdPerfil = usuarioLogadoBase.IdPerfil;
                            }

                            else
                            {
                                List<int> usuariocargos = usuarioLogadoBase.UsuarioCargoes.Select(x => x.IdCargo).Distinct().ToList();
                                var result = (from pr in db.Processo
                                              join d in db.DocDocumento on pr.IdProcesso equals d.IdProcesso
                                              join dc in db.DocumentoCargo on d.IdDocumento equals dc.IdDocumento
                                              join u in db.UsuarioCargo on dc.IdCargo equals u.IdCargo
                                              join c in db.ControladorCategoria on d.IdCategoria equals c.IdControladorCategorias
                                              where d.FlStatus == 3
                                              && d.IdSite == idSite
                                              select new MenuProcessoViewModel
                                              {

                                                  IdProcesso = pr.IdProcesso,
                                                  Nome = pr.Nome,
                                                  Descricao = c.Descricao,
                                                  IdCategoria = c.IdControladorCategorias

                                              }).Distinct().ToList().GroupBy(x => x.Nome);

                                ViewData["Menu"] = result;
                                ViewBag.IdPerfil = usuarioLogadoBase.IdPerfil;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                }
                ViewBag.FuncionalidadesSite = new List<Funcionalidade>();
                ViewBag.Funcionalidades = new List<Funcionalidade>();
                
                
                if (_usuarioAppServico != null)
                {
                    usuarioLogadoBase = _usuarioAppServico.GetById(idUsuario);

                    if (usuarioLogadoBase != null)
                    {
                        if (usuarioLogadoBase.IdPerfil == 4)
                        {
                            ViewBag.Funcionalidades = _usuarioAppServico.ObterFuncionalidadesPermitidas(idUsuario).Where(x => x.Ativo == true).ToList();
                        }
                        else
                        {
                            ViewBag.Funcionalidades = _usuarioAppServico.ObterFuncionalidadesPermitidasPorSite(idSite).Where(x => x.Ativo == true).ToList();
                        }

                        if (idSite != 0)
                        {
                            ViewBag.FuncionalidadesSite = _usuarioAppServico.ObterFuncionalidadesPermitidasPorSite(idSite).Where(x => x.Ativo == true).ToList();
                        }
                    }
                }
            }
            catch
            {
                ViewBag.Funcionalidades = new List<Funcionalidade>();
            }

            try
            {
                ViewBag.Processos = new List<Processo>();
                if (_processoAppServico != null)
                    ViewBag.Processos = _processoAppServico.Get(x => x.IdSite == idSite && !x.FlQualidade).ToList();
            }
            catch
            {
                ViewBag.Processos = new List<Processo>();
            }

            try
            {
                ViewBag.Categorias = new List<ControladorCategoria>();
                if (_controladorCategoriasServico != null)
                    ViewBag.Categorias = _controladorCategoriasServico.ListaAtivos("CATDOC", idSite);
            }
            catch
            {
            }

            try
            {
                using (var db = new BaseContext())
                {

                    var idCliente = Util.ObterClienteSelecionado();

                    var DtVencimento = DateTime.Now.AddDays(-1);

                    var IdSiteManuais = Convert.ToInt32(ConfigurationManager.AppSettings["IdSiteManuais"]);
                    var IdCategoria = Convert.ToInt32(ConfigurationManager.AppSettings["IdCategoriaManuais"]);

                    var DocManuaisQuery = (from doc in db.DocDocumento
                                           join proc in db.Processo on doc.IdProcesso equals proc.IdProcesso
                                           where doc.IdSite == IdSiteManuais && doc.IdCategoria == IdCategoria && doc.FlStatus == 3
                                           select new ManuaisViewModel
                                           {
                                               IdDocumento = doc.IdDocumento,
                                               Processo = proc.Nome,
                                               Titulo = doc.Titulo
                                           }).ToList().GroupBy(x => x.Processo);

                    ViewBag.Manuais = DocManuaisQuery;


                    if (idSite != 0)
                    {



                        if (usuarioLogadoBase.IdPerfil == 1 || usuarioLogadoBase.IdPerfil == 2 || usuarioLogadoBase.IdPerfil == 3)
                        {
                            ViewBag.Pendencia = _pendenciaAppServico.PendeciaPorSite(idSite, idCliente);
                        }
                        else
                        {
                            ViewBag.Pendencia = _pendenciaAppServico.PendeciaPorUsuario(idSite, idCliente, usuarioLogadoBase.IdUsuario);
                        }
                    }
                    else
                    {
                        ViewBag.Pendencia = new List<Pendencia>();
                    }
                }
            }

            catch (Exception)
            {

                throw;
            }

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ModelState.Clear();


            controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            action = filterContext.ActionDescriptor.ActionName;

            if (Session["siteFrase"] != null)
            {
                ViewBag.SiteFrase = Session["siteFrase"];
            }
            else
            {
                ViewBag.SiteFrase = Traducao.Resource.SiteMensagemDefault;
            }

            string parameter = string.Empty;
            if (filterContext.ActionParameters.ContainsKey("rotaDoCliente"))
            {
                //var parametro = filterContext.ActionParameters.Values.Any(x=>x != string.Empty);
                //if (parametro)
                //{

                //    //logout
                //    filterContext.Result = new LoginController(_logServico).Logout();
                //    //redirecti para login
                //    base.OnActionExecuting(filterContext);
                //}

            }

            ViewBag.Action = action;
            ViewBag.Controller = controller;

            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();

            ViewBag.UsuarioLogado = Util.ObterUsuario();
            //ViewBag.ClienteSelecionado = Util.ObterClienteSelecionado();

            ViewBag.Permissoes = Util.ObterPermissoes();
            //ViewBag.ProcessoSelecionado = Util.ObterProcessoSelecionado();
            //ViewBag.UsuarioLogadoBase = 




            try
            {
                int idCliente = Util.ObterClienteSelecionado();

                ViewBag.QuantidadeSites = 0;
                if (_siteRepositorio != null)
                    ViewBag.QuantidadeSites = _siteRepositorio.ListarSitesPorCliente(idCliente);
            }
            catch
            {
                ViewBag.QuantidadeSites = 0;
            }


            #region Parametros de Cultura - Lingua corrente

            lingua = Web.UI.Helpers.Cultura.GetCultura();
            ViewBag.Lingua = lingua;

            #endregion

            base.OnActionExecuting(filterContext);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {

            bool hasAuthorizeAttribute = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthorizeAttribute), false).Any();

            base.OnAuthorization(filterContext);
        }

        public void GeraArquivoZip(ControllerContext context,
                                     string actionName,
                                     int idObjeto)
        {
            string nomePdfGerado = string.Empty;
            string times = DateTime.Now.Ticks.ToString();

            string diretorio = context.HttpContext.Server.MapPath(@"~\Content\temp\" + times);

            Util.VerificaDiretorio(diretorio);


            // string footer = "--footer-left \"Pagina: [page] de [toPage]\" --footer-right \"Data: [date] [time]\" --footer-center \" --footer-line --footer-font-size \"9\" --footer-spacing 5";
            nomePdfGerado = "apenasUmTeste.pdf";

            string fullPath = Path.Combine(diretorio, nomePdfGerado);

            var byteArray = new byte[0];
            //var byteArray = pdfRotativa.BuildPdf(context);


            var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            fileStream.Write(byteArray, 0, byteArray.Length);
            fileStream.Close();
            fileStream.Dispose();
        }

        public void GravaLog(Exception ex)
        {

            var log = new Log(Util.ObterCodigoUsuarioLogado(),
                              Convert.ToInt32(Acao.Login),
                              Util.GetIp(HttpContext),
                              Util.GetBrowser(HttpContext),
                              ex);


            _logServico.Add(log);
        }

        public void GravarLogInclusao(int modulo, int? id)
        {

            var log = new Log(Util.ObterCodigoUsuarioLogado(),
                              Convert.ToInt32(Acao.Salvar),
                              Util.GetIp(HttpContext),
                              Util.GetBrowser(HttpContext),
                              modulo,
                              id);

            _logServico.Add(log);
        }
        public void GravarLogAlteracao(int modulo, int? id) 
        {
            var log = new Log(Util.ObterCodigoUsuarioLogado(),
                               Convert.ToInt32(Acao.Alterar),
                               Util.GetIp(HttpContext),
                               Util.GetBrowser(HttpContext),
                               modulo,
                               id);

            _logServico.Add(log);
        }
        public void GravarLogExclusao(int modulo, int? id) 
        {
            var log = new Log(Util.ObterCodigoUsuarioLogado(),
                               Convert.ToInt32(Acao.Excluir),
                               Util.GetIp(HttpContext),
                               Util.GetBrowser(HttpContext),
                               modulo,
                               id);

            _logServico.Add(log);
        }

        public void GravarLogImpressao(int modulo, int? id)
        {
            var log = new Log(Util.ObterCodigoUsuarioLogado(),
                               Convert.ToInt32(Acao.Impressao),
                               Util.GetIp(HttpContext),
                               Util.GetBrowser(HttpContext),
                               modulo,
                               id);

            _logServico.Add(log);
        }
        public void EscolheProcesso(string idProcesso, string nomeProcesso)
        {
            try
            {
                idProcesso = UtilsServico.CriptografarString(idProcesso);

                var cookieCodigo = new HttpCookie("processoSelecionadoCodigo", idProcesso)
                {
                    Expires = DateTime.Now.AddDays(1)
                };

                var cookieNome = new HttpCookie("processoSelecionadoNome", nomeProcesso)
                {
                    Expires = DateTime.Now.AddDays(1)
                };

                Response.Cookies.Add(cookieCodigo);
                Response.Cookies.Add(cookieNome);

            }
            catch (Exception ex)
            {
                GravaLog(ex);
            }
        }

        protected void RemoveTodosCookies()
        {
            HttpCookie aCookie;
            string cookieName;
            int limit = Request.Cookies.Count;
            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name;
                aCookie = new HttpCookie(cookieName);
                aCookie.Expires = DateTime.Now.AddDays(-1); // make it expire yesterday
                Response.Cookies.Add(aCookie); // overwrite it
            }
        }
        protected string RetornaApenasNumeros(string str) =>
             Regex.Replace(str, @"[^\d]", "");

        protected byte[] TransformaString64EmBase64(string strB64) =>
            strB64 == null ? Encoding.ASCII.GetBytes("") : Convert.FromBase64String(strB64);

        protected string RetornaExtensao(string nomeArquivo) =>
            nomeArquivo != null ? nomeArquivo.Substring(nomeArquivo.LastIndexOf(".")) : "";


    }

}
