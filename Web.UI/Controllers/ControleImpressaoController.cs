using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Servico;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.UI.Helpers;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class ControleImpressaoController : BaseController
    {
        private readonly IControleImpressaoAppServico _controleImpressaoAppServico;
        private readonly IControleImpressaoServico _controleImpressaoServico;
        private readonly IUsuarioAppServico _usuarioAppServico;

        private readonly ILogAppServico _logAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public ControleImpressaoController(IControleImpressaoAppServico controleImpressao, 
                                           ILogAppServico logAppServico,
                                           IControleImpressaoServico controleImpressaoServico,
                                           IUsuarioAppServico usuarioAppServico,
                                           IProcessoAppServico processoAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _controleImpressaoAppServico = controleImpressao;
            _controleImpressaoServico = controleImpressaoServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        // GET: ControleImpressao
        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            return View();
        }

        public ActionResult PDF(int? idUsuarioDestino)
        {
            List<string> erros = new List<string>();

            var controle = new ControleImpressao();
            controle.IdUsuarioDestino = idUsuarioDestino;
            controle.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
            controle.IdFuncionalidade = 2;
            controle.CodigoReferencia = "1";
            controle.DataImpressao = DateTime.Now;
            controle.DataInclusao = DateTime.Now;
            controle.CopiaControlada = false;

            var pdfView = new ViewAsPdf();

            pdfView.ViewName = "PDF";

            if (idUsuarioDestino != null)
                controle.CopiaControlada = true;

            _controleImpressaoServico.Valido(controle, ref erros);

            if (erros.Count == 0)
            {
                _controleImpressaoAppServico.Add(controle);
            }
            
            ViewBag.CopiaControlada = controle.CopiaControlada;

            return pdfView;
        }
    }
}