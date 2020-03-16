using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Dominio.Entidade;
using Newtonsoft.Json;
using System.Text;
using Web.UI.Helpers;
using ApplicationService.Interface;
using System.Text.RegularExpressions;
using Dominio.Interface.Servico;
using System.Web;
using ApplicationService.Entidade;
using Dominio.Enumerado;
using System.Data.Entity.Infrastructure;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class SiteController : BaseController
    {
        public readonly ISiteAppServico _siteAppServico;
        public readonly IClienteAppServico _clienteAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly ISiteModuloAppServico _siteModuloAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IAnexoAppServico _anexoAppServico;
        public readonly ISiteServico _siteServico;
        private readonly IFuncionalidadeAppServico _funcionalidadeAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public SiteController(ISiteAppServico siteAppServico, IClienteAppServico clienteAppServico,
                              ISiteModuloAppServico siteModuloAppServico,
                              IProcessoAppServico processoAppServico, ILogAppServico logAppServico,
                              ISiteServico siteServico,
                              IAnexoAppServico anexoAppServico,
                              IFuncionalidadeAppServico funcionalidadeAppServico,
                              IUsuarioAppServico usuarioAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _siteAppServico = siteAppServico;
            _clienteAppServico = clienteAppServico;
            _processoAppServico = processoAppServico;
            _logAppServico = logAppServico;
            _siteModuloAppServico = siteModuloAppServico;
            _usuarioAppServico = usuarioAppServico;

            _siteServico = siteServico;

            _anexoAppServico = anexoAppServico;
            _funcionalidadeAppServico = funcionalidadeAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        [HttpGet]
        [AcessoUsuarioSite]
        public ActionResult Index(int? id)
        {

            if (!id.HasValue)
            {
                id = Util.ObterClienteSelecionado();
            }
            else
            {
                SetCookieClienteSelecionado(id.Value);
            }

            var sites = _siteAppServico.Get(x => x.IdCliente == id.Value);

            ViewBag.IdCliente = id.Value;

            return View(sites);
        }

        [HttpGet]
        [AcessoUsuarioSite]
        public ActionResult Editar(int id)
        {
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();
            var site = _siteAppServico.GetById(id);
            ViewBag.Funcionalidades = _funcionalidadeAppServico.Get(x => x.Ativo == true);
            return View("Criar", site);
        }

        [HttpPost]
        [AcessoAdmin]
        public JsonResult Excluir(int id)
        {
            var erros = new List<string>();

            try
            {
                _siteServico.Excluir(id);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_erro_ExcluirSite);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Site.ResourceSite.Site_msg_exluir }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AcessoUsuarioSite]
        public ActionResult Criar(int idCliente)
        {
            var site = new Site();
            site.IdCliente = idCliente;
            ViewBag.Funcionalidades = _funcionalidadeAppServico.Get(x => x.Ativo == true);
            return View(site);
        }

        [HttpGet]
        public ActionResult ObterSitesPorCliente(int? idCliente)
        {
            var sites = new List<Site>();
            var listaJson = new List<Site>();

            if (idCliente == null)
            {
                idCliente = Util.ObterClienteSelecionado();
                sites.AddRange(_siteAppServico.ObterSitesPorCliente(Convert.ToInt32(idCliente)));
            }
            else
            {
                sites.AddRange(_siteAppServico.ObterSitesPorCliente(Convert.ToInt32(idCliente)));
            }

            foreach (var site in sites)
            {
                var siteLogoAux = site.SiteAnexo.FirstOrDefault().Anexo;
                siteLogoAux.ArquivoB64 = Convert.ToBase64String(siteLogoAux.Arquivo);
                siteLogoAux.Arquivo = null;
                siteLogoAux.FotosSite = new List<SiteAnexo>();
                listaJson.Add(new Site()
                {
                    IdCliente = site.IdCliente,
                    IdSite = site.IdSite,
                    NmFantasia = site.NmFantasia,
                    SiteLogoAux = siteLogoAux
                });
            }

            var json = JsonConvert.SerializeObject(listaJson, Formatting.Indented);

            return new ContentResult
            {
                Content = json,
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };
        }

        [HttpGet]
        public ActionResult EscolheSite(int idSite)
        {
            if (idSite == 0)
            {
                HttpCookie cookie = Request.Cookies["siteSelecionado"];

                cookie.Value = null;
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(cookie);

                Session.Add("siteFrase", Traducao.Resource.SiteMensagemDefault);

                Session["siteImg"] = "/Content/assets/imagens/cliente-padrao.png";

                return RedirectToAction("Index", "Cliente/Index");
            }
            else
            {

                try
                {
                    var site = _siteAppServico.GetById(idSite);

                    HttpCookie cookie = Request.Cookies["siteSelecionado"];

                    if (cookie == null)
                    {
                        cookie = new HttpCookie("siteSelecionado");
                    }
                  
                    cookie.Value = site.IdSite.ToString();
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(cookie);

                    SetCookieClienteSelecionado(site.IdCliente);

                    HttpCookie cookie1 = Request.Cookies["siteSelecionado"];

                    Session.Add("siteFrase", site.DsFrase);

                    Session["siteImg"] = String.Format("data:image/" + site.SiteAnexo.FirstOrDefault().Anexo.Extensao + ";base64," + Convert.ToBase64String(site.SiteAnexo.FirstOrDefault().Anexo.Arquivo));
                }
                catch (Exception ex)
                {
                    GravaLog(ex);
                }
                if (Util.ObterPerfilUsuarioLogado() == (int)PerfisAcesso.Administrador || Util.ObterPerfilUsuarioLogado() == (int)PerfisAcesso.Coordenador)
                {

                    return RedirectToAction("HomeProcesso", "Home", new { idSite = idSite });
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
        }

        [HttpPost]
        [AcessoUsuarioSite]
        public JsonResult AtivaInativa(int idSite)
        {

            var erros = new List<string>();

            var resposta = _siteAppServico.AtivarInativar(idSite);

            if (!resposta)
            {
                return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Site.ResourceSite.Site_msg_icone_ativo_valid }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AcessoUsuarioSite]
        public JsonResult Salvar(Site site)
        {
            var erros = new List<string>();

            try
            {
                TrataSite(site);

                _siteServico.Valida(site, ref erros);

                if (erros.Count > 0)
                {
                    return Json(new { statuscode = 505, erros = erros }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    if (site.IdSite == 0)
                    {
                        site.SiteFuncionalidades.ToList().ForEach(x => x.Site = site);
                        site.Processos.ToList().ForEach(processo =>
                        {
                            processo.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
                            processo.DataCadastro = DateTime.Now;
                            processo.Site = site;
                        });

                        site.SiteAnexo.Add(new SiteAnexo
                        {
                            Site = site,
                            Anexo = site.SiteLogoAux
                        });

                        _siteAppServico.Add(site);

                    }
                    else
                    {
                        AtualizaAnexo(site);
                        AtualizaProcesso(site);
                        DeletaOuCriaProcessos(site.Processos.ToList(), site.IdSite);

                        DeletaOuCriaSiteFuncionalidades(site.SiteFuncionalidades.ToList(), site.IdSite);

                        _siteAppServico.Update(site);
                    }

                }


            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Site.ResourceSite.Site_msg_criar_valid }, JsonRequestBehavior.AllowGet);
        }

        private void SetCookieClienteSelecionado(int idCliente)
        {
            HttpCookie cookie = Request.Cookies["clienteSelecionado"];


            cookie.Value = idCliente.ToString();

            cookie.Expires = DateTime.Now.AddDays(1);

            Response.Cookies.Set(cookie);
        }

        private void AtualizaProcesso(Site site)
        {
            foreach (var x in site.Processos.Where(y => y.IdProcesso > 0).ToList())
            {
                var processo = _processoAppServico.GetById(x.IdProcesso);
                processo.Nome = x.Nome;
                processo.FlAtivo = x.FlAtivo;
                processo.FlQualidade = x.FlQualidade;
                _processoAppServico.Update(processo);
            }

            site.Processos.Where(y => y.IdProcesso == 0).ToList().ForEach(x => _processoAppServico.Add(x));
        }

        private void AtualizaAnexo(Site site)
        {
            var anexoCtx = _anexoAppServico.GetById(site.SiteLogoAux.IdAnexo);
            anexoCtx.Nome = site.SiteLogoAux.Nome;
            anexoCtx.Extensao = site.SiteLogoAux.Extensao;
            anexoCtx.Arquivo = site.SiteLogoAux.Arquivo;
            _anexoAppServico.Update(anexoCtx);

        }

        private void TrataSite(Site site)
        {
            if (site.SiteLogoAux.ArquivoB64 != null)
            {
                site.SiteLogoAux.Tratar();
            }
            site.NuCNPJ = RetornaApenasNumeros(site.NuCNPJ);
        }

        private void DeletaOuCriaProcessos(List<Processo> listaProcessos, int idSite)
        {
            var listaCtx = _processoAppServico.Get(x => x.IdSite == idSite).ToList();
            var listaQueSeraoAdicionados = listaProcessos.Where(x => x.IdProcesso == 0).ToList();
            var listaQueSeraDeletados = new List<Processo>();

            listaQueSeraoAdicionados.ForEach(processo => _processoAppServico.Add(processo));

            listaCtx.ForEach(processoQueSeraDeletado =>
            {
                if (!listaProcessos.Any(y => y.IdProcesso == processoQueSeraDeletado.IdProcesso))
                {
                    listaQueSeraDeletados.Add(processoQueSeraDeletado);
                }
            });

            listaQueSeraDeletados.ForEach(processo => _processoAppServico.Remove(processo));

        }

        private void DeletaOuCriaSiteFuncionalidades(List<SiteFuncionalidade> listaSiteFuncionalidades, int idSite)
        {
            var listaQueSeraoAdicionados = listaSiteFuncionalidades.Where(x => x.IdSiteFuncionalidade == 0).ToList();
            var listaQueSeraDeletados = new List<SiteFuncionalidade>();
            var exceptList = new List<SiteFuncionalidade>();

            listaQueSeraoAdicionados.ForEach(x =>
            {
                if (x.IdFuncionalidade == 2)
                {
                    _siteModuloAppServico.Add(x);
                    exceptList.Add(x);

                    SiteFuncionalidade funcionalidade = new SiteFuncionalidade
                    {
                        IdFuncionalidade = 13,
                        IdSite = x.IdSite
                    };

                    _siteModuloAppServico.Add(funcionalidade);
                    exceptList.Add(funcionalidade);
                }
                else
                {
                    _siteModuloAppServico.Add(x);
                    exceptList.Add(x);
                }
            });

            var listaCtx = _siteModuloAppServico.Get(x => x.IdSite == idSite).ToList().Except(exceptList).ToList();

            listaCtx.ForEach(funcionalidade => _siteModuloAppServico.Remove(funcionalidade));
        }
    }

}