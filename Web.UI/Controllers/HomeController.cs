using System.Web.Mvc;
using Dominio.Enumerado;
using System.Linq;
using Dominio.Entidade;
using System.Collections.Generic;
using System;
using Web.UI.Helpers;
using ApplicationService.Interface;
using System.Web;
using System.Text;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class HomeController : BaseController
    {
        private readonly IClienteAppServico _clienteAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        private int idPerfil;
        private int idSite;

        public HomeController(IClienteAppServico clienteAppServico, IProcessoAppServico processoAppServico,
                              IUsuarioClienteSiteAppServico usuarioClienteAppServico, ILogAppServico logAppServico,
                              IUsuarioAppServico usuarioAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _clienteAppServico = clienteAppServico;
            _processoAppServico = processoAppServico;
            _usuarioClienteAppServico = usuarioClienteAppServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        // GET: Home
        public ActionResult Index()
        {
            var idUsuario = Util.ObterCodigoUsuarioLogado();
            idPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.IdPerfil = idPerfil;
            var listaProcessos = new List<Processo>();


            Usuario usuario = _usuarioAppServico.GetById(idUsuario);
            Cliente cliente = _clienteAppServico.ObterClientesPorUsuario(idUsuario).FirstOrDefault();

            int? nuDiasTrocaSenha = (EAdministrador() || ESuporte()) ? 30 : cliente.NuDiasTrocaSenha;

            if (DiasParaTrocaDeSenha(usuario.DtAlteracaoSenha.Value, DateTime.Now) >= nuDiasTrocaSenha && cliente.NuDiasTrocaSenha != 0)
            {
                return RedirectToAction("AlterarSenha", "Usuario");
            }

            if (EAdministrador() || ESuporte())
            {
                idSite = Util.ObterSiteSelecionado();

                //IEnumerable<Cliente> listaClientes = _clienteAppServico.ObterClientesPorUsuario(Util.ObterCodigoUsuarioLogado());

                return RedirectToAction("Index", "Cliente/Index");
            }

            if (ECoordenador())
            {

                var sitesUsuario = _usuarioClienteAppServico.ObterSitesPorUsuario(Util.ObterCodigoUsuarioLogado());

                idSite = Convert.ToInt32(sitesUsuario.FirstOrDefault().IdSite);

                EscolheSite(sitesUsuario.FirstOrDefault().Site);

                listaProcessos.AddRange(_processoAppServico.ListaProcessosPorSite(idSite));
                ViewBag.Funcionalidades = _usuarioAppServico.ObterFuncionalidadesPermitidasPorSite(idSite);
            }

            if (EColaborador())
            {

                var sitesUsuario = _usuarioClienteAppServico.ObterSitesPorUsuario(idUsuario);
                idSite = Convert.ToInt32(sitesUsuario.FirstOrDefault().IdSite);

                EscolheSite(sitesUsuario.FirstOrDefault().Site);

                listaProcessos.AddRange(_processoAppServico.ListaProcessosPorUsuario(idUsuario));
                ViewBag.Funcionalidades = _usuarioAppServico.ObterFuncionalidadesPermitidas(idUsuario);

            }

            return View(listaProcessos);

        }

        private void EscolheSite(Site site)
        {
            try
            {
                var anexo = new Anexo() {
                    Arquivo = site.SiteAnexo.FirstOrDefault().Anexo.Arquivo,
                    Extensao = site.SiteAnexo.FirstOrDefault().Anexo.Extensao,
                    Nome = site.SiteAnexo.FirstOrDefault().Anexo.Nome
                };


                HttpCookie cookie = Request.Cookies["siteSelecionado"];

                cookie.Value = site.IdSite.ToString();

                cookie.Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Set(cookie);
                                
                Session.Add("siteFrase", site.DsFrase);

                Session["siteImg"] = String.Format("data:application/" + anexo.Extensao + ";base64," + Convert.ToBase64String(anexo.Arquivo));

            }
            catch (Exception ex)
            {
                GravaLog(ex);
            }
        }

        public ActionResult HomeProcesso(int idSite)
        {
            var listaProcessos = new List<Processo>();
        
            listaProcessos.AddRange(_processoAppServico.ListaProcessosPorSite(idSite));

           

            return View("Index", listaProcessos);
        }

        public ActionResult BloqueioModulo()
        {
            return View("BloqueioModulo");
        }

        public ActionResult BloqueioProcesso()
        {
            return View("BloqueioProcesso");
        }
        public ActionResult BloqueioUnauthorized()
        {
            return View("BloqueioUnauthorized");
        }

        public bool EAdministrador()
        {
            if (idPerfil == (int)PerfisAcesso.Administrador)
            {
                return true;
            }
            return false;
        }

        public bool ESuporte()
        {
            if (idPerfil == (int)PerfisAcesso.Suporte)
            {
                return true;
            }
            return false;
        }

        public bool ECoordenador()
        {
            if (idPerfil == (int)PerfisAcesso.Coordenador)
            {
                return true;
            }
            return false;
        }

        public bool EColaborador()
        {
            if (idPerfil == (int)PerfisAcesso.Colaborador)
            {
                return true;
            }
            return false;
        }

        public int DiasParaTrocaDeSenha(DateTime data1, DateTime data2)
        {
            return (data1 - data2).Days * - 1;
        }
    }
}