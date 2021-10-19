using ApplicationService.Enum;
using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Rotativa;
using Rotativa.Options;
using Web.UI.Helpers;
using System.Linq;
using DAL.Context;
using System.Data.Entity;

namespace Web.UI.Controllers
{
   
    public class LogController : BaseController
    {

        
        private readonly IProcessoAppServico _processoAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IFilaEnvioServico _filaEnvioServico;
        private readonly ICargoProcessoAppServico _cargoProcessoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteAppServico;
        private readonly IArquivoLicencaAnexoAppServico _arquivoLicencaAnexoAppServico;

        public LogController(         ILogAppServico logAppServico,
                                      IUsuarioAppServico usuarioAppServico,
                                      ICargoProcessoAppServico cargoProcessoAppServico,
                                      IProcessoAppServico processoAppServico,
                                      IControladorCategoriasAppServico controladorCategoriasServico,
                                      IFilaEnvioServico filaEnvioServico,
                                      IUsuarioClienteSiteAppServico usuarioClienteAppServico,
                                      IPendenciaAppServico pendenciaAppServico,
                                      IArquivoLicencaAnexoAppServico arquivoLicencaAnexoAppServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico, pendenciaAppServico)
        {

            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _cargoProcessoAppServico = cargoProcessoAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _filaEnvioServico = filaEnvioServico;
            _usuarioClienteAppServico = usuarioClienteAppServico;
            _arquivoLicencaAnexoAppServico = arquivoLicencaAnexoAppServico;
        }

        // GET: Instrumentos
        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var idSiteCorrente = Util.ObterSiteSelecionado();
            var idUsuario = Util.ObterCodigoUsuarioLogado();
            var idCliente = Util.ObterClienteSelecionado();

            var model = _logAppServico.GetAll().Where(x => x.IdUsuario != null && x.Modulo != null);

            
            return View(model);
        }
    }
};