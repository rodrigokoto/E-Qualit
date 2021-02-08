using Dominio.Entidade;
using Dominio.Enumerado;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.UI.Helpers;
using ApplicationService.Interface;
using Dominio.Interface.Servico;
using ApplicationService.Enum;
using Rotativa.Options;
using Web.UI.Models;
using System.Web.Routing;
using System.Threading;
using System.Threading.Tasks;
using DAL.Context;
using System.Activities.Debugger;
using System.Data.Entity;
using System.IO;
using Microsoft.Office.Interop.Word;

namespace Web.UI.Controllers
{
    public class BackupController : BaseController
    {

        private int _funcaoImprimir = 8;
        private int _funcaoRevisar = 6;
        //private int _processo;
        private int _site = Util.ObterSiteSelecionado();

        private readonly IDocDocumentoAppServico _documentoAppServico;
        private readonly IDocDocumentoServico _documentoServico;
        private readonly IProcessoServico _processoServico;

        private readonly IRegistroConformidadesAppServico _registroConformidadeAppServico;
        private readonly IRegistroConformidadesServico _registroConformidadeServico;
        private readonly IDocUsuarioVerificaAprovaServico _docUsuarioVerificaAprovaServico;

        private readonly ICargoAppServico _cargoAppServico;

        private readonly IDocTemplateAppServico _docTemplateAppServico;

        private readonly IDocUsuarioVerificaAprovaAppServico _docUsuarioVerificaAprovaAppServico;

        public IUsuarioAppServico _usuarioAppServico;

        private readonly IControleImpressaoAppServico _controleImpressaoAppServico;
        private readonly IControleImpressaoServico _controleImpressaoServico;

        private readonly ILogAppServico _logAppServico;

        private readonly IDocCargoAppServico _docCargoAppServico;

        private readonly IDocumentoAssuntoAppServico _documentoAssuntoAppServico;

        private readonly IDocumentoComentarioAppServico _documentoComentarioAppServico;
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IAnexoAppServico _AnexoAppServico;
        

        public BackupController(IDocDocumentoAppServico docDocumentoAppServico,
                                    IDocDocumentoServico documentoServico,
                                    IRegistroConformidadesAppServico registroConformidadeAppServico,
                                    ICargoAppServico cargoAppServico,
                                    IDocTemplateAppServico docTemplate,
                                    IDocUsuarioVerificaAprovaAppServico docUsuarioVerificaAprovaAppServico,
                                    IUsuarioAppServico usuarioAppServico,
                                    IControleImpressaoAppServico controleImpressaoAppServico,
                                    ILogAppServico logAppServico,
                                    IDocCargoAppServico docCargoAppServico,
                                    IDocumentoAssuntoAppServico documentoAssuntoAppServico,
                                    IDocumentoComentarioAppServico documentoComentarioAppServico,
                                    IControleImpressaoServico controleImpressaoServico,
                                    IProcessoServico processoServico,
                                    IRegistroConformidadesServico registroConformidadeServico,
                                    IUsuarioClienteSiteAppServico usuarioClienteAppServico,
                                    IProcessoAppServico processoAppServico,
                                    IDocUsuarioVerificaAprovaServico docUsuarioVerificaAprovaServico,
                                    IControladorCategoriasAppServico controladorCategoriasServico,
                                    IAnexoAppServico anexoAppServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _AnexoAppServico = anexoAppServico;
            _documentoAppServico = docDocumentoAppServico;
            _registroConformidadeAppServico = registroConformidadeAppServico;
            _cargoAppServico = cargoAppServico;
            _docTemplateAppServico = docTemplate;
            _docUsuarioVerificaAprovaAppServico = docUsuarioVerificaAprovaAppServico;
            _usuarioAppServico = usuarioAppServico;
            _controleImpressaoAppServico = controleImpressaoAppServico;
            _logAppServico = logAppServico;
            _docCargoAppServico = docCargoAppServico;
            _documentoAssuntoAppServico = documentoAssuntoAppServico;
            _documentoComentarioAppServico = documentoComentarioAppServico;
            _controleImpressaoServico = controleImpressaoServico;
            _documentoServico = documentoServico;
            _processoServico = processoServico;
            _registroConformidadeServico = registroConformidadeServico;
            _usuarioClienteAppServico = usuarioClienteAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _docUsuarioVerificaAprovaServico = docUsuarioVerificaAprovaServico;
        }

        // GET: Backup
        public ActionResult Index()
        {
            List<DocDocumento> listaResult = _documentoAppServico.ListaDocumentosAprovadosMaiorRevisao(1015, null).ToList();
            int nrDoc = 1;
            foreach (var docDocumento in listaResult)
            {
                Application app = new Microsoft.Office.Interop.Word.Application();
                Document doc = new Microsoft.Office.Interop.Word.Document();

                try
                {
                    
                    string TituloDoc = nrDoc+"-"+docDocumento.NumeroDocumento;
                    string PathDoc = "~/savedoc/" + TituloDoc + ".doc";
                    string templatePath = Server.MapPath("~/Templates/DocTemplate.docx");

                    string savePath = Server.MapPath(PathDoc);
                   
                    doc = app.Documents.Open(templatePath);
                    doc.Activate();

                    doc.Bookmarks["Revisao"].Range.Text = docDocumento.NuRevisao.ToString();
                    doc.Bookmarks["Titulo"].Range.Text = docDocumento.Titulo.ToString();
                    doc.Bookmarks["Sigla"].Range.Text = docDocumento.Sigla.Descricao.ToString();
                    doc.Bookmarks["NrDocumento"].Range.Text = docDocumento.NumeroDocumento.ToString();
                    doc.Bookmarks["Processo"].Range.Text = docDocumento.Processo.Nome.ToString();
                    doc.Bookmarks["Categoria"].Range.Text = docDocumento.Categoria.Descricao.ToString();
                    doc.Bookmarks["Elaborador"].Range.Text = docDocumento.Elaborador.NmCompleto.ToString();

                    if (docDocumento.TextoDoc != null)
                    {
                        var decodedCkeditor = System.Net.WebUtility.HtmlDecode(docDocumento.TextoDoc.ToString());

                        doc.Bookmarks["Texto"].Range.Text = decodedCkeditor;
                    }

                    doc.SaveAs2(savePath);
                    app.Application.Quit();
                    nrDoc = nrDoc+1;


                }
                catch (Exception ex)
                {
                    app.Application.Quit();
                }

            }

            return View(listaResult);
        }
    }
}