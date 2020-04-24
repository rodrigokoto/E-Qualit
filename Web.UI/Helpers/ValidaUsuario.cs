using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using DAL.Repository;
using Dominio.Entidade;
using Web.UI.App_Start;

namespace Web.UI.Helpers
{
    public class ValidaUsuario : AuthorizeAttribute
    {
        public ValidaUsuario()
        {

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var request = httpContext.Request;
            var router = request.RequestContext.RouteData;
            string action = router.GetRequiredString("action");
            string controller = router.GetRequiredString("controller");
            var idSite = Util.ObterSiteSelecionado();
            var idUsuario = Util.ObterCodigoUsuarioLogado();
            var id = router.Values["id"];


            if (action == "Editar")
            {
                SiteModuloRepositorio siterepo = new SiteModuloRepositorio();
                UsuarioClienteSiteRepositorio clieSite = new UsuarioClienteSiteRepositorio();

                var usuSite = clieSite.GetAll().Where(x => x.IdUsuario == idUsuario).FirstOrDefault();
                switch (controller)
                {
                    case "Site":
                        if (id != null)
                        {
                            if (usuSite.IdSite == Convert.ToInt32(idSite))
                            {
                                return true;

                            }

                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    case "Cargo":
                        CargoRepositorio cargoRepositorio = new CargoRepositorio();
                        var cargo = cargoRepositorio.GetById(Convert.ToInt32(id));

                        if (id != null)
                        {
                            if (cargo.IdSite == Convert.ToInt32(idSite))
                            {
                                return true;

                            }

                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }


                    case "usuario":
                        UsuarioRepositorio usuarioRepositorio = new UsuarioRepositorio();
                        var usuario = usuarioRepositorio.GetById(Convert.ToInt32(id));

                        var userEdicao = clieSite.GetAll().Where(x => x.IdUsuario == usuario.IdUsuario).FirstOrDefault(); ;

                        if (id != null)
                        {
                            if (userEdicao.IdSite == Convert.ToInt32(idSite))
                            {
                                return true;

                            }

                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }

                    case "ControlDoc":
                        DocDocumentoRepositorio docDocumentoRepositorio = new DocDocumentoRepositorio();
                        var doc = docDocumentoRepositorio.GetById(Convert.ToInt32(id));

                        if (id != null)
                        {
                            if (doc.IdSite == Convert.ToInt32(idSite))
                            {
                                return true;

                            }

                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }


                    case "NaoConformidade":
                        RegistroConformidadesRepositorio conformidadeRepositorio = new RegistroConformidadesRepositorio();
                        var conformidade = conformidadeRepositorio.GetById(Convert.ToInt32(id));


                        if (id != null)
                        {
                            if (conformidade.IdSite == Convert.ToInt32(idSite))
                            {
                                return true;

                            }

                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }


                    case "AcaoCorretiva":

                    case "Indicador":

                    case "AnaliseCritica":
                        AnaliseCriticaRepositorio analiseCriticaRepositorio = new AnaliseCriticaRepositorio();
                        var analise = analiseCriticaRepositorio.GetById(Convert.ToInt32(id));


                        if (id != null)
                        {
                            if (analise.IdSite == Convert.ToInt32(idSite))
                            {
                                return true;

                            }

                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }


                    case "Licenca":

                        LicencaRepositorio licencaRepositorio = new LicencaRepositorio();

                        var licenca = licencaRepositorio.GetById(Convert.ToInt32(id));

                        var LicencaSite = clieSite.Get(x => x.IdCliente == licenca.Idcliente).FirstOrDefault();

                        if (id != null)
                        {
                            if (LicencaSite.IdSite == Convert.ToInt32(idSite))
                            {
                                return true;

                            }

                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }

                    case "Instrumento":

                    case "Fornecedor":
                        FornecedorRepositorio fornecedorRepositorio = new FornecedorRepositorio();

                        var fornecedor = fornecedorRepositorio.GetById(Convert.ToInt32(id));


                        if (id != null)
                        {
                            if (fornecedor.IdSite == Convert.ToInt32(idSite))
                            {
                                return true;

                            }

                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }


                    case "GestaoDeRisco":

                    case "GestaoMelhoria":
                    default:
                        break;
                }
            }
            else
            {
                return true;
            }

            return true;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Home/BloqueioModulo");
        }
    }
}