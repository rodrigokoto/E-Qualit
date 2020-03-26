using DAL.Repository;
using Dominio.Interface.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.UI.Helpers
{
    public class EditarPossuiAcessoSite : AuthorizeAttribute
    {

        private readonly IUsuarioClienteSiteRepositorio usuarioClienteSiteRepositorio = new UsuarioClienteSiteRepositorio();
        private readonly IDocDocumentoRepositorio docDocumentoRepositorio = new DocDocumentoRepositorio();
        private readonly IAnaliseCriticaRepositorio analiseCriticaRepositorio = new AnaliseCriticaRepositorio();
        private readonly IInstrumentoRepositorio instrumentoRepositorio = new InstrumentoRepositorio();
        private readonly IRegistroConformidadesRepositorio registroConformidadesRepositorio = new RegistroConformidadesRepositorio();
        private readonly ILicencaRepositorio licencaRepositorio = new LicencaRepositorio();
        private readonly IIndicadorRepostorio indicadorRepostorio = new IndicadorRepositorio();

        public EditarPossuiAcessoSite()
        {
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            var httpContext = filterContext.RequestContext.HttpContext;
            var request = httpContext.Request;
            var router = request.RequestContext.RouteData;
            string action = router.GetRequiredString("action");
            string controller = router.GetRequiredString("controller");
            var idSite = 0;
            var id = router.Values["id"];
            if (action == "Editar")
            {
                switch (controller)
                {
                    case "AnaliseCritica":
                        idSite = Convert.ToInt32(request.Params["siteSelecionado"]);
                        var analise = analiseCriticaRepositorio.GetById(Convert.ToInt32(id));
                        if (analise.IdSite != idSite)
                        {
                            filterContext.Result = new RedirectResult("/Home/BloqueioUnauthorized");
                        }
                        break;
                    case "ControlDoc":
                        idSite = Convert.ToInt32(request.Params["siteSelecionado"]);
                        var docdocumento = docDocumentoRepositorio.GetById(Convert.ToInt32(id));
                        if (docdocumento.IdSite != idSite)
                        {
                            filterContext.Result = new RedirectResult("/Home/BloqueioUnauthorized");
                        }
                        break;
                    case "Instrumento":
                        idSite = Convert.ToInt32(request.Params["siteSelecionado"]);
                        var instrumento = instrumentoRepositorio.GetById(Convert.ToInt32(id));
                        if (instrumento.IdSite != idSite)
                        {
                            filterContext.Result = new RedirectResult("/Home/BloqueioUnauthorized");
                        }
                        break;
                    case "GestaoDeRisco":
                        idSite = Convert.ToInt32(request.Params["siteSelecionado"]);
                        var gestaoRisco = registroConformidadesRepositorio.GetById(Convert.ToInt32(id));

                        if (gestaoRisco.IdSite != idSite)
                        {
                            filterContext.Result = new RedirectResult("/Home/BloqueioUnauthorized");
                        }
                        break;
                    case "Licenca":
                        idSite = Convert.ToInt32(request.Params["siteSelecionado"]);
                        var licenca = licencaRepositorio.GetById(Convert.ToInt32(id));

                        var usuariocliente = usuarioClienteSiteRepositorio.GetAll().Where(x => x.IdCliente == licenca.Idcliente).FirstOrDefault();

                        if (usuariocliente.IdSite != idSite)
                        {
                            filterContext.Result = new RedirectResult("/Home/BloqueioUnauthorized");
                        }

                        break;
                    case "Indicador":
                        idSite = Convert.ToInt32(request.Params["siteSelecionado"]);
                        var indicador = indicadorRepostorio.GetById(Convert.ToInt32(id));
                        if (indicador.IdSite != idSite)
                        {
                            filterContext.Result = new RedirectResult("/Home/BloqueioUnauthorized");
                        }
                        break;
                    case "AcaoCorretiva":
                        idSite = Convert.ToInt32(request.Params["siteSelecionado"]);
                        var acao = registroConformidadesRepositorio.GetById(Convert.ToInt32(id));

                        if (acao.IdSite != idSite)
                        {
                            filterContext.Result = new RedirectResult("/Home/BloqueioUnauthorized");
                        }

                        break;
                    default:
                        break;
                }
            }


            //ilterContext.Result = new RedirectResult("/Home/BloqueioUnauthorized");

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            try
            {
                var routeData = httpContext.Request.RequestContext.RouteData;
                string action = routeData.GetRequiredString("action");
                string controller = routeData.GetRequiredString("controller");

            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Home/BloqueioUnauthorized");
        }
    }
}