using ApplicationService.Interface;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.UI.Controllers
{
    public class AnexoController : BaseController
    {
        private readonly IRegistroConformidadesAppServico _registroConformidadesAppServico;
        private readonly IRegistroConformidadesServico _registroConformidadesServico;
        private readonly IRegistroAcaoImediataServico _registroRegistroAcaoImediataServico;
        private readonly IPaiAppServico _paiAppServico;
        private readonly IPlaiAppServico _plaiAppServico;
        private readonly IPlaiProcessoNormaAppServico _plaiProcessoNormaAppServico;
        private readonly INormaAppServico _NormaAppServico;
        private readonly IAnexoAppServico _AnexoAppServico;
        private readonly IDocDocumentoAppServico _documentoAppServico;
        private readonly ILicencaAppServico _licencaAppServico;
        private readonly ILicencaServico _licencaServico;
        private readonly IInstrumentoAppServico _instrumentoAppServico;
        private readonly IInstrumentoServico _instrumentoServico;
        private readonly ICalibracaoAppServico _calibracaoAppServico;
        private readonly ICriterioAceitacaoAppServico _criterioAceitacaoAppServico;
        private readonly IArquivoLicencaAnexoAppServico _arquivoLicencaAnexoAppServico;
        public int idSite;

        public AnexoController(ILogAppServico logServico,
        IUsuarioAppServico usuarioAppServico,
        IProcessoAppServico processoAppServico,
        IRegistroConformidadesAppServico registroConformidadesAppServico,
        IRegistroConformidadesServico registroConformidadesServico,
        IRegistroAcaoImediataServico registroRegistroAcaoImediataServico,
        IPaiAppServico paiAppServico,
        IPlaiAppServico plaiAppServico,
        IPlaiProcessoNormaAppServico plaiProcessoNormaAppServico,
        INormaAppServico NormaAppServico,
        IAnexoAppServico AnexoAppServico,
        IDocDocumentoAppServico documentoAppServico,
        ILicencaAppServico licencaAppServico,
        ILicencaServico licencaServico,
        IInstrumentoAppServico instrumentoAppServico,
        IInstrumentoServico instrumentoServico,
        ICalibracaoAppServico calibracaoAppServico,
        ICriterioAceitacaoAppServico criterioAceitacaoAppServico,
        IArquivoLicencaAnexoAppServico arquivoLicencaAnexoAppServico,
        IControladorCategoriasAppServico controladorCategoriasServico) : base(logServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {

            _registroConformidadesAppServico = registroConformidadesAppServico;
            _registroConformidadesServico = registroConformidadesServico;
            _registroRegistroAcaoImediataServico = registroRegistroAcaoImediataServico;
            _paiAppServico = paiAppServico;
            _plaiAppServico = plaiAppServico;
            _plaiProcessoNormaAppServico = plaiProcessoNormaAppServico;
            _NormaAppServico = NormaAppServico;
            _AnexoAppServico = AnexoAppServico;
            _documentoAppServico = documentoAppServico;
            _licencaAppServico = licencaAppServico;
            _licencaServico = licencaServico;
            _instrumentoAppServico = instrumentoAppServico;
            _instrumentoServico = instrumentoServico;
            _calibracaoAppServico = calibracaoAppServico;
            _criterioAceitacaoAppServico = criterioAceitacaoAppServico;
            _arquivoLicencaAnexoAppServico = arquivoLicencaAnexoAppServico;

        }

        // GET: Anexo
        public ActionResult BackupAnexo(int idCliente, int Modulo)
        {

            
           
           
            return View();
        }

        public void BackupRegistros(string TpRegistro)
        {
            switch (TpRegistro)
            {
                case "AC":
                    break;
                case "NC":
                    break;
                case "GR":
                    break;
                case "GM":
                    break;
                default:
                    break;
            }
        }
        public void BackupDocdocumento()
        {
            var Documentos = _documentoAppServico.ListaTodosDocumentosProcessoSite(idSite, null).ToList();

            foreach (var doc in Documentos)
            {
                
            }
            
        }
        public void BackuAuditoria()
        {

        }
        public void BackupLicencas()
        {

        }
        public void BackupInstrumento()
        {

        }
    }
}