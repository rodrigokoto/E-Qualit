using Dominio.Entidade;
using Dominio.Enumerado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.UI.Helpers;
using ApplicationService.Interface;
using Dominio.Servico;
using Dominio.Interface.Servico;
using Rotativa;
using Rotativa.Options;
using ApplicationService.Entidade;
using System.Configuration;
using Web.UI.Models;

namespace Web.UI.Controllers
{
    //[ProcessoSelecionado]
    [VerificaIntegridadeLogin]
    
    public class AcaoImediataController : BaseController
    {
        private readonly IRegistroConformidadesAppServico _registroConformidadesAppServico;
        private readonly IRegistroConformidadesServico _registroConformidadesServico;
        private readonly IRegistroAcaoImediataServico _registroRegistroAcaoImediataServico;
        private readonly IClienteAppServico _clienteServico;
        private readonly INotificacaoAppServico _notificacaoAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteAppServico;
        private readonly IFilaEnvioServico _filaEnvioServico;
        private readonly IPendenciaAppServico _pendenciaAppServico;

        public AcaoImediataController(
            IRegistroConformidadesAppServico registroConformidadesAppServico,
            IRegistroConformidadesServico registroConformidadesServico,
            INotificacaoAppServico notificacaoAppServico,
            ILogAppServico logAppServico,
            IProcessoAppServico processoAppServico,
            IUsuarioAppServico usuarioAppServico,
            IClienteAppServico clienteServico,
            IControladorCategoriasAppServico controladorCategoriasServico,
            IUsuarioClienteSiteAppServico usuarioClienteAppServico,
            IFilaEnvioServico filaEnvioServico,
            IPendenciaAppServico pendenciaAppServico,
        IRegistroAcaoImediataServico registroRegistroAcaoImediataServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico, pendenciaAppServico)
        {
            _registroConformidadesAppServico = registroConformidadesAppServico;
            _registroConformidadesServico = registroConformidadesServico;
            _notificacaoAppServico = notificacaoAppServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _clienteServico = clienteServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _filaEnvioServico = filaEnvioServico;
            _registroRegistroAcaoImediataServico = registroRegistroAcaoImediataServico;
            _usuarioClienteAppServico = usuarioClienteAppServico;
            _pendenciaAppServico = pendenciaAppServico;
        }

        public JsonResult ExcluirAcaoImediata(int IdAcaoImediata)
        {

            var erros = new List<string>();

            try
            {
                var registroAcaoImediata = _registroRegistroAcaoImediataServico.GetById(IdAcaoImediata);
                _registroRegistroAcaoImediataServico.Remove(registroAcaoImediata);
                return Json(new { StatusCode = (int)HttpStatusCode.OK });
            }
            catch (Exception)
            {
                erros.Add("Falha ao Excluir ação imediata");
                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros });
            }
        }
    }
}