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

namespace Web.UI.Controllers
{
    //[SitePossuiModulo((int)Funcionalidades.ControlDoc)]
    //[ProcessoSelecionado]
    //[VerificaIntegridadeLogin]
    public class ControlDocController : BaseController
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

        public ControlDocController(IDocDocumentoAppServico docDocumentoAppServico,
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
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
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
        }

        public ActionResult Index(string Mensagem = "")
        {

            ViewBag.IdSite = Util.ObterSiteSelecionado();

            var idPerfil = Util.ObterPerfilUsuarioLogado();

            var comPermissao = ECoordenadorOuAdministrador(idPerfil);

            ViewBag.UsuarioPodeAlterar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(Util.ObterCodigoUsuarioLogado(), (int)Funcionalidades.ControlDoc, 100);
            ViewBag.UsuarioPodeDeletar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(Util.ObterCodigoUsuarioLogado(), (int)Funcionalidades.ControlDoc, 102);

            List<DocDocumento> listaResult = _documentoAppServico.ListaTodosDocumentosProcessoSite(Util.ObterSiteSelecionado()).ToList();

            var comPermissaoDeRevisar = _usuarioAppServico.ObterUsuariosPorFuncao(Util.ObterSiteSelecionado(), _funcaoRevisar).Where(s => s.IdUsuario == Util.ObterCodigoUsuarioLogado()).FirstOrDefault();
            ViewBag.PodeRevisar = comPermissaoDeRevisar != null ? true : false;
            ViewBag.PodeVisualizarObsoletos = true;

            ViewBag.Mensagem = Mensagem;
            CarregarDropDownUsuarios();
            return View("ListDocumentos", listaResult);
        }

        public void CarregarDropDownUsuarios()
        {
            var site = Util.ObterSiteSelecionado();
            var usuarioClienteApp = _usuarioClienteAppServico.Get(s => s.IdSite == site);

            var lstItem = new List<SelectListItem>();

            foreach (var uca in usuarioClienteApp)
            {
                var usuario = uca.Usuario;
                lstItem.Add(new SelectListItem() { Text = usuario.NmCompleto, Value = usuario.IdUsuario.ToString() });
            }
            ViewBag.usuarios = lstItem;
        }

        [HttpPost]
        public ActionResult PDF([System.Web.Http.FromBody]int id, bool controlada, string usuarioDest, string fluxoBase64)
        {
            ApplicationService.Entidade.UsuarioApp UsuarioLogado = (ApplicationService.Entidade.UsuarioApp)ViewBag.UsuarioLogado;

            if (!string.IsNullOrEmpty(usuarioDest))
            {
                var controleImpressao = new ControleImpressao()
                {
                    DataImpressao = DateTime.Now,
                    IdFuncionalidade = 2, //control-doc
                    CodigoReferencia = string.Empty,
                    CopiaControlada = controlada,
                    DataInclusao = DateTime.Now,
                    IdUsuarioDestino = string.IsNullOrEmpty(usuarioDest) ? 0 : Convert.ToInt32(usuarioDest),
                    IdUsuarioIncluiu = Convert.ToInt32(UsuarioLogado.IdUsuario)
                };

                _controleImpressaoAppServico.Add(controleImpressao);
            }

            var documento = _documentoAppServico.Get(s => s.IdDocumento == id).FirstOrDefault();
            documento.FluxoBase64 = fluxoBase64;

            var usuarioClienteApp = _usuarioClienteAppServico.Get(s => s.IdSite == documento.IdSite);

            var clienteLogoAux = usuarioClienteApp.FirstOrDefault().Cliente.ClienteLogo.FirstOrDefault().Anexo;

            LayoutImpressaoViewModel model = new LayoutImpressaoViewModel()
            {
                LogoCliente = Convert.ToBase64String(clienteLogoAux.Arquivo),
                Documento = documento,
                IsImpressaoControlada = controlada
            };

            var pdf = new ViewAsPdf
            {
                ViewName = "PDF",
                Model = model,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "Documento.pdf"
            };

            ViewBag.CopiaControlada = controlada;

            return pdf;

            //return View("PDF", model);
        }


        public ActionResult PDFTeste(int id, bool controlada, string usuarioDest, string fluxoBase64)
        {
            ApplicationService.Entidade.UsuarioApp UsuarioLogado = (ApplicationService.Entidade.UsuarioApp)ViewBag.UsuarioLogado;

            if (!string.IsNullOrEmpty(usuarioDest))
            {
                var controleImpressao = new ControleImpressao()
                {
                    DataImpressao = DateTime.Now,
                    IdFuncionalidade = 2, //control-doc
                    CodigoReferencia = string.Empty,
                    CopiaControlada = controlada,
                    DataInclusao = DateTime.Now,
                    IdUsuarioDestino = string.IsNullOrEmpty(usuarioDest) ? 0 : Convert.ToInt32(usuarioDest),
                    IdUsuarioIncluiu = Convert.ToInt32(UsuarioLogado.IdUsuario)
                };

                _controleImpressaoAppServico.Add(controleImpressao);
            }

            var documento = _documentoAppServico.Get(s => s.IdDocumento == id).FirstOrDefault();
            documento.FluxoBase64 = fluxoBase64;

            var usuarioClienteApp = _usuarioClienteAppServico.Get(s => s.IdSite == documento.IdSite);

            var clienteLogoAux = usuarioClienteApp.FirstOrDefault().Cliente.ClienteLogo.FirstOrDefault().Anexo;

            LayoutImpressaoViewModel model = new LayoutImpressaoViewModel()
            {
                LogoCliente = Convert.ToBase64String(clienteLogoAux.Arquivo),
                Documento = documento,
                IsImpressaoControlada = controlada
            };

            var pdf = new ViewAsPdf
            {
                ViewName = "PDF",
                Model = model,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "Documento.pdf"
            };

            ViewBag.CopiaControlada = controlada;
                       

            return View("PDF", model);
        }

        public ActionResult ConteudoDocumento(int id, int Obsoleto = 0)
        {
            var documento = new DocDocumento();
            documento.GestaoDeRisco = new RegistroConformidade();
            documento.Licenca = new Licenca();
            documento.Licenca.Anexo = new Anexo();
            var usuarioLogado = Util.ObterCodigoUsuarioLogado();
            documento.DocExterno = new DocExterno();
            documento.DocExterno.Anexo = new Anexo();
            documento.NumeroDocumento = _documentoServico.GeraProximoNumeroRegistro(Util.ObterSiteSelecionado());

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.EColaborador = Util.ObterPerfilUsuarioLogado() == (int)PerfisAcesso.Colaborador ? "true" : "false";
            ViewBag.IdUsuarioLogado = usuarioLogado;
            ViewBag.NmUsuarioLogado = Util.ObterUsuario().Nome;
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();

            ViewBag.NumeroRisco = _registroConformidadeServico.GeraProximoNumeroRegistro("gr", Util.ObterSiteSelecionado());

            documento = _documentoAppServico.GetById(id);

            documento.Aprovadores = documento.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "A").OrderBy(x => x.Usuario.NmCompleto).ToList();
            documento.Verificadores = documento.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "V").OrderBy(x => x.Usuario.NmCompleto).ToList();
            CarregarDropDownUsuarios();
            ViewBag.EhOboleto = Obsoleto;

            if (documento.IdDocExterno > 0)
            {
                documento.DocExterno.Anexo.ArquivoB64 = documento.DocExterno.Anexo.TrataAnexoVindoBanco();

            }

            DocDocumento documentoFilhoAtualizar = new DocDocumento();

            //if (Obsoleto == 1)
            //{
            //    DocDocumento documentoFilho = new DocDocumento();
            //    int IdDocumento = documento.IdDocumento;


            //    while (documentoFilho != null)
            //    {
            //        documentoFilho = _documentoAppServico.Get(x => x.IdDocumentoPai == IdDocumento).First();

            //        if(documentoFilho != null)
            //        {
            //            IdDocumento = documentoFilho.IdDocumento;
            //            documentoFilhoAtualizar = documentoFilho;
            //        }                    
            //    }
            //}

            //if(documentoFilhoAtualizar != null && documentoFilhoAtualizar.IdDocumento != 0)
            //{
            //    documento.Assuntos = documentoFilhoAtualizar.Assuntos;
            //    documento.Aprovadores = documentoFilhoAtualizar.Aprovadores;
            //    documento.Comentarios = documentoFilhoAtualizar.Comentarios;
            //    documento.DocExterno = documentoFilhoAtualizar.DocExterno;
            //    documento.DocTemplate = DocDocumento
            //}           

            return View(documento);
        }


        public ActionResult ConteudoDocumentoObsoleto(int id)
        {
            var documento = new DocDocumento();
            documento.GestaoDeRisco = new RegistroConformidade();
            documento.Licenca = new Licenca();
            documento.Licenca.Anexo = new Anexo();
            var usuarioLogado = Util.ObterCodigoUsuarioLogado();
            documento.DocExterno = new DocExterno();
            documento.DocExterno.Anexo = new Anexo();
            documento.NumeroDocumento = _documentoServico.GeraProximoNumeroRegistro(Util.ObterSiteSelecionado());

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.EColaborador = Util.ObterPerfilUsuarioLogado() == (int)PerfisAcesso.Colaborador ? "true" : "false";
            ViewBag.IdUsuarioLogado = usuarioLogado;
            ViewBag.NmUsuarioLogado = Util.ObterUsuario().Nome;
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();

            ViewBag.NumeroRisco = _registroConformidadeServico.GeraProximoNumeroRegistro("gr", Util.ObterSiteSelecionado());

            documento = _documentoAppServico.GetById(id);

            documento.Aprovadores = documento.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "A").OrderBy(x => x.Usuario.NmCompleto).ToList();
            documento.Verificadores = documento.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "V").OrderBy(x => x.Usuario.NmCompleto).ToList();
            CarregarDropDownUsuarios();
            return View(documento);
        }


        //[AutorizacaoUsuario((int)FuncoesControlDoc.Elaborar, (int)Funcionalidades.ControlDoc)]
        public ActionResult EmissaoDocumento(int? id)
        {
            var documento = new DocDocumento();
            documento.GestaoDeRisco = new RegistroConformidade();
            documento.Licenca = new Licenca();
            documento.Licenca.Anexo = new Anexo();
            var usuarioLogado = Util.ObterCodigoUsuarioLogado();
            documento.DocExterno = new DocExterno();
            documento.DocExterno.Anexo = new Anexo();
            documento.NumeroDocumento = _documentoServico.GeraProximoNumeroRegistro(Util.ObterSiteSelecionado());

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.EColaborador = Util.ObterPerfilUsuarioLogado() == (int)PerfisAcesso.Colaborador ? "true" : "false";
            ViewBag.IdUsuarioLogado = usuarioLogado;
            ViewBag.NmUsuarioLogado = Util.ObterUsuario().Nome;
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();

            ViewBag.NumeroRisco = _registroConformidadeServico.GeraProximoNumeroRegistro("gr", Util.ObterSiteSelecionado());


            if (id != null)
            {
                documento = _documentoAppServico.GetById(id.Value);

                documento.Aprovadores = documento.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "A").OrderBy(x => x.Usuario.NmCompleto).ToList();
                documento.Verificadores = documento.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "V").OrderBy(x => x.Usuario.NmCompleto).ToList();

                if (VerificarSeDocumentoEstaEmUso(documento, usuarioLogado))
                    return RedirectToAction("Index", new RouteValueDictionary(
                                           new { controller = "ControlDoc", action = "Index", Mensagem = String.Format(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_documentoEmUso, documento.Titulo) }));
            }

            CarregarDropDownUsuarios();
            return View(documento);
        }
        private bool VerificarSeDocumentoEstaEmUso(DocDocumento documento, int usuarioLogado)
        {

            if (documento.FlStatus == (byte)StatusDocumento.Aprovado && documento.StatusRegistro != (byte)StatusRegistro.EmUso)
            {
                documento.StatusRegistro = (byte)StatusRegistro.EmUso;
                documento.DtAlteracao = DateTime.Now;
                documento.IdUsuarioAlteracao = usuarioLogado;
                _documentoAppServico.Update(documento);
            }
            else if (documento.FlStatus == (byte)StatusDocumento.Aprovado && documento.StatusRegistro == (byte)StatusRegistro.EmUso)
            {
                var tempoAlterando = DateTime.Now.Subtract(documento.DtAlteracao);

                //Liberando o documento caso esteja mais de 7 minutos em uso
                if (tempoAlterando > new TimeSpan(0, 7, 0))
                {
                    documento.StatusRegistro = (byte)StatusRegistro.Destravado;
                    documento.DtAlteracao = DateTime.Now;
                    documento.IdUsuarioAlteracao = usuarioLogado;
                    _documentoAppServico.Update(documento);

                }
                else if (usuarioLogado != documento.IdUsuarioAlteracao)
                    return true;
            }

            return false;
        }

        public ActionResult DocumentosElaboracao()
        {
            ViewBag.FlStatus = (int)StatusDocumento.Elaboracao;
            ViewBag.IdSite = Util.ObterSiteSelecionado();

            var idPerfil = Util.ObterPerfilUsuarioLogado();

            var comPermissao = ECoordenadorOuAdministrador(idPerfil);

            ViewBag.UsuarioPodeAlterar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(Util.ObterCodigoUsuarioLogado(), (int)Funcionalidades.ControlDoc, 100);
            ViewBag.UsuarioPodeDeletar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(Util.ObterCodigoUsuarioLogado(), (int)Funcionalidades.ControlDoc, 102);

            List<DocDocumento> listaResult = _documentoAppServico.ListaDocumentosStatusProcessoSite(Util.ObterSiteSelecionado(), ViewBag.FlStatus);

            if (idPerfil == 4)
            {
                listaResult = listaResult.Where(x => x.IdElaborador == Util.ObterCodigoUsuarioLogado()).ToList();
            }

            ViewBag.EstaEmElaboracao = true;
            ViewBag.PodeRevisar = true;
            ViewBag.PodeVisualizarObsoletos = true;

            CarregarDropDownUsuarios();

            return View("ListDocumentos", listaResult);
        }

        public ActionResult DocumentosVerificacao()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var idPerfil = Util.ObterPerfilUsuarioLogado();

            var perfilAutorizado = ECoordenadorOuAdministrador(idPerfil);

            ViewBag.UsuarioPodeAlterar = perfilAutorizado ? perfilAutorizado : _usuarioAppServico.PossuiAcesso(Util.ObterCodigoUsuarioLogado(), (int)Funcionalidades.ControlDoc, 100);

            List<DocDocumento> listaDocumentos = new List<DocDocumento>();

            if (perfilAutorizado)
            {
                listaDocumentos.AddRange(_documentoAppServico.ListaDocumentosEmVerificacaoPorProcessoESite(Util.ObterSiteSelecionado()));
            }
            else
            {
                listaDocumentos.AddRange(_documentoAppServico.ListaDocumentosVerificacaoPorVerificador(Util.ObterSiteSelecionado(), Util.ObterCodigoUsuarioLogado()));
            }

            CarregarDropDownUsuarios();
            return View("ListDocumentos", listaDocumentos);
        }

        public ActionResult DocumentosAprovacao()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.FlStatus = (int)StatusDocumento.Aprovacao;

            var idPerfil = Util.ObterPerfilUsuarioLogado();

            var perfilAutorizado = ECoordenadorOuAdministrador(idPerfil);

            List<DocDocumento> listaDocumentos = new List<DocDocumento>();

            if (perfilAutorizado)
            {
                listaDocumentos.AddRange(_documentoAppServico.ListaDocumentosEmAprovacaoPorProcessoESite(Util.ObterSiteSelecionado()));
            }
            else
            {
                listaDocumentos.AddRange(_documentoAppServico.ListaDocumentosEmAprovacaoPorAprovador(Util.ObterSiteSelecionado(), Util.ObterCodigoUsuarioLogado()));
            }

            var comPermissao = ECoordenadorOuAdministrador(idPerfil);

            ViewBag.UsuarioPodeAlterar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(Util.ObterCodigoUsuarioLogado(), (int)Funcionalidades.ControlDoc, 100);

            ViewBag.EstaEmElaboracao = false;
            CarregarDropDownUsuarios();
            return View("ListDocumentos", listaDocumentos);
        }

        public ActionResult DocumentosAprovados(int? id = null, int? idProcesso = null)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.FlStatus = (int)StatusDocumento.Aprovado;

            List<DocDocumento> listaResult = _documentoAppServico.ListaDocumentosAprovadosMaiorRevisao(Util.ObterSiteSelecionado(), idProcesso).Where(x => x.IdCategoria == id || id == null).ToList();

            var idPerfil = Util.ObterPerfilUsuarioLogado();

            var comPermissao = ECoordenadorOuAdministrador(idPerfil);

            var comPermissaoDeRevisar = _usuarioAppServico.ObterUsuariosPorFuncao(Util.ObterSiteSelecionado(), _funcaoRevisar).Where(s => s.IdUsuario == Util.ObterCodigoUsuarioLogado()).FirstOrDefault();
            ViewBag.PodeRevisar = comPermissaoDeRevisar != null || idPerfil != 4 ? true : false;
            var comPermissaoDeImprimir = _usuarioAppServico.ObterUsuariosPorFuncao(Util.ObterSiteSelecionado(), _funcaoImprimir).Where(s => s.IdUsuario == Util.ObterCodigoUsuarioLogado()).FirstOrDefault();
            ViewBag.PodeImprimir = comPermissaoDeImprimir != null ? true : false;

            ViewBag.EstaEmElaboracao = false;
            // chumbado
            ViewBag.PodeRevisar = id == null && idProcesso == null && ViewBag.PodeRevisar == true ? true : false;
            ViewBag.UsuarioPodeAlterar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(Util.ObterCodigoUsuarioLogado(), (int)Funcionalidades.ControlDoc, 100);
            if (idProcesso != null)
            {
                ViewBag.Revisao = false;
            }
            else
            {
                ViewBag.Revisao = true;
            }

            ViewBag.PodeImprimir = true;
            ViewBag.PodeImprimir = true;
            ViewBag.EstaEmElaboracao = true;
            CarregarDropDownUsuarios();
            return View("ListDocumentos", listaResult);
        }

        public ActionResult DocumentosRevisao()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.FlStatus = (int)StatusDocumento.Aprovado;

            List<DocDocumento> listaResult = _documentoAppServico.ListaDocumentosAprovadosMaiorRevisao(Util.ObterSiteSelecionado(), null).ToList();

            var idPerfil = Util.ObterPerfilUsuarioLogado();

            var comPermissao = ECoordenadorOuAdministrador(idPerfil);

            var comPermissaoDeRevisar = _usuarioAppServico.ObterUsuariosPorFuncao(Util.ObterSiteSelecionado(), _funcaoRevisar).Where(s => s.IdUsuario == Util.ObterCodigoUsuarioLogado()).FirstOrDefault();
            ViewBag.PodeRevisar = comPermissaoDeRevisar != null || idPerfil != 4 ? true : false;
            var comPermissaoDeImprimir = _usuarioAppServico.ObterUsuariosPorFuncao(Util.ObterSiteSelecionado(), _funcaoImprimir).Where(s => s.IdUsuario == Util.ObterCodigoUsuarioLogado()).FirstOrDefault();
            ViewBag.PodeImprimir = comPermissaoDeImprimir != null ? true : false;

            ViewBag.EstaEmElaboracao = false;
            // chumbado
            ViewBag.PodeRevisar = ViewBag.PodeRevisar == true ? true : false;
            ViewBag.UsuarioPodeAlterar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(Util.ObterCodigoUsuarioLogado(), (int)Funcionalidades.ControlDoc, 100);

            ViewBag.Revisao = true;

            if (idPerfil == 4)
            {
                listaResult = listaResult.Where(x => x.IdElaborador == Util.ObterCodigoUsuarioLogado()).ToList();
            }

            ViewBag.PodeImprimir = true;
            ViewBag.PodeImprimir = true;
            ViewBag.EstaEmElaboracao = true;
            CarregarDropDownUsuarios();
            return View("ListDocumentos", listaResult);
        }

        public ActionResult DocumentosObsoletos(int? idProcesso = null)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.FlStatus = (int)StatusDocumento.Aprovado;

            List<DocDocumento> listaResult = _documentoAppServico.ListaDocumentosObsoletosMaiorRevisao(Util.ObterSiteSelecionado(), idProcesso).ToList();

            var idPerfil = Util.ObterPerfilUsuarioLogado();

            var comPermissao = ECoordenadorOuAdministrador(idPerfil);

            var comPermissaoDeRevisar = _usuarioAppServico.ObterUsuariosPorFuncao(Util.ObterSiteSelecionado(), _funcaoRevisar).Where(s => s.IdUsuario == Util.ObterCodigoUsuarioLogado()).FirstOrDefault();
            ViewBag.PodeRevisar = comPermissaoDeRevisar != null ? true : false;
            var comPermissaoDeImprimir = _usuarioAppServico.ObterUsuariosPorFuncao(Util.ObterSiteSelecionado(), _funcaoImprimir).Where(s => s.IdUsuario == Util.ObterCodigoUsuarioLogado()).FirstOrDefault();
            ViewBag.PodeImprimir = comPermissaoDeImprimir != null ? true : false;
            ViewBag.PodeVisualizarObsoletos = true;
            ViewBag.EstaEmElaboracao = false;
            ViewBag.Obsoletos = true;
            // chumbado
            ViewBag.PodeRevisar = true;
            ViewBag.UsuarioPodeAlterar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(Util.ObterCodigoUsuarioLogado(), (int)Funcionalidades.ControlDoc, 100);
            if (idProcesso != null)
            {
                ViewBag.Revisao = false;
            }
            else
            {
                ViewBag.Revisao = true;
            }

            ViewBag.PodeImprimir = true;
            ViewBag.PodeImprimir = true;
            ViewBag.EstaEmElaboracao = true;
            CarregarDropDownUsuarios();
            return View("ListDocumentos", listaResult);
        }

        public ActionResult Recursos()
        {
            try
            {
                setViewBagsListDocumentosAprovados();

                var template = DocTemplate.RecursosTemplate;
                var documentos = _documentoAppServico.ListaDocumentosPorTemplateSiteEProcesso(_site, template);

                return View("ListDocumentos", documentos);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Riscos()
        {
            try
            {
                setViewBagsListDocumentosAprovados();

                var template = DocTemplate.GestaoDeRiscoTemplate;

                var documentos = _documentoAppServico.ListaDocumentosPorTemplateSiteEProcesso(_site, template);

                return View("ListDocumentos", documentos);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Rotina()
        {
            try
            {
                setViewBagsListDocumentosAprovados();

                var template = DocTemplate.RotinaTemplate;
                var documentos = _documentoAppServico.ListaDocumentosPorTemplateSiteEProcesso(_site, template);

                return View("ListDocumentos", documentos);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Fluxo()
        {
            try
            {
                setViewBagsListDocumentosAprovados();

                var template = DocTemplate.FluxoTemplate;
                var documentos = _documentoAppServico.ListaDocumentosPorTemplateSiteEProcesso(_site, template);

                return View("ListDocumentos", documentos);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Registro()
        {
            try
            {
                setViewBagsListDocumentosAprovados();

                var template = DocTemplate.RegistrosTemplate;
                var documentos = _documentoAppServico.ListaDocumentosPorTemplateSiteEProcesso(_site, template);

                return View("ListDocumentos", documentos);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = ex }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Revisar(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var erros = new List<string>();
            DocDocumento docRevisao = new DocDocumento();
            try
            {
                var doc = _documentoAppServico.GetById(id);

                //_documentoServico.ValidoParaRevisao(doc, ref erros);

                if (erros.Count > 0)
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);

                docRevisao = _documentoServico.CriarRevisaoDocumento(id, Util.ObterCodigoUsuarioLogado());

                EnviaNotificacaoPorEmail(doc);
            }
            catch (Exception ex)
            {
                GravaLog(ex);

                return Json(new
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Erro = ex.ToString()
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, IdRevisao = docRevisao.IdDocumento }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Excluir(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            try
            {
                var documento = _documentoAppServico.GetById(id);

                _documentoAppServico.Remove(documento);

                return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = (int)HttpStatusCode.InternalServerError }, JsonRequestBehavior.AllowGet);
            }
        }

        public bool verificaSeEhInteiro(decimal valor)
        {

            // O sinal % retorna o resto da divisão. Caso o resto seja 0, você pode considerar que o numero seja inteiro.
            decimal resultado = valor % 2;
            if (resultado.Equals(1) || resultado.Equals(0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public JsonResult RetornaNumeroPorSigla(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            try
            {
                decimal numeroDocumento = _documentoServico.GeraProximoNumeroRegistro(Util.ObterSiteSelecionado(), null, id);

                var numeroEhInteiro = verificaSeEhInteiro(numeroDocumento);

                return Json(new { StatusCode = (int)HttpStatusCode.OK, Retorno = (numeroEhInteiro ? numeroDocumento.ToString("000") : numeroDocumento.ToString().Replace(".", ",")) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = (int)HttpStatusCode.InternalServerError }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CargosPorDocumento(int id)
        {
            int site;
            site = Util.ObterSiteSelecionado();


            var cargos = _cargoAppServico.ObtemCargosPorSite(site).Select(x => new { x.NmNome, x.IdCargo, IdDocCargo = 0 });

            var documento = _documentoAppServico.GetById(id);

            if (documento != null)
            {
                if (documento.DocCargo != null)
                {
                    var docCargo = documento.DocCargo;

                    var listaJson = new List<object>();

                    foreach (var cargo in cargos)
                    {
                        var novoObjetoCargo = new
                        {
                            NmNome = cargo.NmNome,
                            IdCargo = cargo.IdCargo,
                            IdDocCargo = docCargo.FirstOrDefault(x => x.IdCargo == cargo.IdCargo) != null ? docCargo.FirstOrDefault(x => x.IdCargo == cargo.IdCargo).Id : 0
                        };

                        listaJson.Add(novoObjetoCargo);
                    }

                    return Json(new { Dados = listaJson, StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Dados = cargos, StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Salvar(DocDocumento doc, StatusDocumento status)
        {
            var erros = new List<string>();

            ViewBag.StatusDocumento = status;


            switch (status)
            {
                case StatusDocumento.Elaboracao:
                    {
                        if (doc.IdDocumento != 0)
                            return Editar(doc);
                        else
                            return Criar(doc);
                    }
                case StatusDocumento.Verificacao:
                    {
                        if (doc.IdDocumento != 0)
                            return Editar(doc);
                        else
                            return Criar(doc);
                    }
                case StatusDocumento.Aprovacao:
                    {
                        if (doc.IdDocumento != 0)
                            return Editar(doc);
                        else
                            return Criar(doc);
                    }
                case StatusDocumento.Aprovado:
                    {
                        foreach (var item in doc.Comentarios)
                        {
                            try
                            {
                                _documentoComentarioAppServico.Remove(item);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        doc.FlStatus = (int)StatusDocumento.Aprovado;

                        if (doc.IdDocumento != 0)
                            return Editar(doc);
                        else
                            return Criar(doc);
                    }
            }



            List<DocDocumento> listaResult = _documentoAppServico.ListaDocumentosStatusProcessoSite(Util.ObterSiteSelecionado(), (int)StatusDocumento.Elaboracao).ToList();
            return View("ListDocumentos", listaResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Criar(DocDocumento doc)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            try
            {
                var erros = new List<string>();

                doc.Assuntos.Add(new DocumentoAssunto { DataAssunto = DateTime.Now, Descricao = Traducao.Resource.DescricaoRevisaoEmissaoInicial, Revisao = "0" });

                TrataCriacaoDoc(doc, ref erros);

                _documentoServico.Valido(doc, ref erros);

                if (doc.GestaoDeRisco != null && !string.IsNullOrEmpty(doc.GestaoDeRisco.DescricaoRegistro))
                {
                    //doc.GestaoDeRisco.EProcedente = doc.PossuiGestaoRisco;
                    doc.GestaoDeRisco.EProcedente = true;
                    doc.GestaoDeRisco.StatusEtapa = 1;
                    doc.GestaoDeRisco.TipoRegistro = "gr";
                    doc.GestaoDeRisco.IdSite = Util.ObterSiteSelecionado();
                    doc.GestaoDeRisco.IdEmissor = Util.ObterCodigoUsuarioLogado();
                    _registroConformidadeServico.ValidarCampos(doc.GestaoDeRisco, ref erros);
                }

                if (erros.Count > 0)
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);

                _documentoAppServico.SalvarDocumento(doc);

                EnviaNotificacaoPorEmail(doc);
            }
            catch (Exception ex)
            {
                GravaLog(ex);

                return Json(new
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Erro = ex.ToString()
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_Success, IdDocumento = doc.IdDocumento }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Editar(int id)
        {
            var documento = _documentoAppServico.GetById(id);
            var usuarioLogado = Util.ObterCodigoUsuarioLogado();

            if (VerificarSeDocumentoEstaEmUso(documento, usuarioLogado))
                return RedirectToAction("Index", new RouteValueDictionary(
                                      new { controller = "ControlDoc", action = "Index", Mensagem = String.Format(Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_documentoEmUso, documento.Titulo) }));

            if (documento.IdLicenca > 0)
            {
                documento.Licenca.Anexo.ArquivoB64 = documento.Licenca.Anexo.TrataAnexoVindoBanco();

            }

            if (documento.IdDocExterno > 0)
            {
                documento.DocExterno.Anexo.ArquivoB64 = documento.DocExterno.Anexo.TrataAnexoVindoBanco();

            }

            if (documento.FlWorkFlow)
            {
                //documento.Aprovadores = documento.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "A").OrderBy(x => x.Usuario.NmCompleto).ToList();
                //documento.Verificadores = documento.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "V").OrderBy(x => x.Usuario.NmCompleto).ToList();
                documento.Aprovadores = documento.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "A").ToList();
                documento.Verificadores = documento.DocUsuarioVerificaAprova.Where(x => x.TpEtapa == "V").ToList();
            }
            if (documento.IdGestaoDeRisco > 0)
            {
                documento.GestaoDeRisco = _registroConformidadeAppServico.GetById(documento.IdGestaoDeRisco.Value);
            }

            ViewBag.NmUsuarioLogado = Util.ObterUsuario().Nome;
            ViewBag.IdUsuarioLogado = usuarioLogado;


            ViewBag.IdSite = Util.ObterSiteSelecionado();
            setViewBagsPorEtapaDocumento(documento, Util.ObterCodigoUsuarioLogado());

            List<DocumentoComentario> listaComent = _documentoComentarioAppServico.GetAll().Where(x => x.IdDocumento == documento.IdDocumento).Distinct().ToList();
            documento.Comentarios = new List<DocumentoComentario>();

            foreach (var item in listaComent.Distinct().ToList())
            {
                if (documento.Comentarios.Contains(item))
                {
                    documento.Comentarios.Add(item);
                }


            }
            ViewBag.NumeroRisco = documento.GestaoDeRisco != null ? documento.GestaoDeRisco.NuRegistro : 0;
            ViewBag.IsEdicao = true;
            CarregarDropDownUsuarios();
            documento.Rotinas = documento.Rotinas.OrderBy(x => x.Item).ToList();
            return View("EmissaoDocumento", documento);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Editar(DocDocumento documentoEditado)
        {
            if (documentoEditado.GestaoDeRisco != null)
                documentoEditado.IdGestaoDeRisco = documentoEditado.GestaoDeRisco.IdRegistroConformidade;

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var erros = new List<string>();

            try
            {
                //if (documentoEditado.FlStatus == (byte)StatusDocumento.Aprovado)
                _documentoServico.AssuntoObrigatorioEditarRevisao(documentoEditado, ref erros);

                TrataEdicaoDoc(documentoEditado, ref erros);

                if (documentoEditado.GestaoDeRisco != null && !string.IsNullOrEmpty(documentoEditado.GestaoDeRisco.DescricaoRegistro))
                {
                    //documentoEditado.GestaoDeRisco.EProcedente = documentoEditado.PossuiGestaoRisco;
                    documentoEditado.GestaoDeRisco.EProcedente = true;
                    documentoEditado.GestaoDeRisco.StatusEtapa = 1;
                    documentoEditado.GestaoDeRisco.TipoRegistro = "gr";
                    documentoEditado.GestaoDeRisco.IdSite = Util.ObterSiteSelecionado();
                    documentoEditado.GestaoDeRisco.IdEmissor = Util.ObterCodigoUsuarioLogado();
                    _registroConformidadeServico.ValidarCampos(documentoEditado.GestaoDeRisco, ref erros);
                }

                _documentoServico.Valido(documentoEditado, ref erros);

                if (erros.Count > 0)
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);

                AtualizarUsuarioCargosETemplatesDoDocumento(documentoEditado);

                AtualizarAssuntos(documentoEditado);
                AdicionaComentario(documentoEditado);

                documentoEditado.DtAlteracao = DateTime.Now;
                documentoEditado.StatusRegistro = (byte)StatusRegistro.Destravado;
                documentoEditado.IdUsuarioAlteracao = Util.ObterCodigoUsuarioLogado();

                DocDocumento baseDocumento = _documentoAppServico.GetById(documentoEditado.IdDocumento);

                MapearDocumentoBase(baseDocumento, documentoEditado);

                if (baseDocumento.FlWorkFlow)
                {
                    _documentoAppServico.Update(baseDocumento);
                }
                else
                {
                    _documentoAppServico.AprovarDocumentoPorUsuario(baseDocumento, Util.ObterCodigoUsuarioLogado());
                }

            }
            catch (Exception ex)
            {
                GravaLog(ex);

                return Json(new { StatusCode = (int)HttpStatusCode.InternalServerError, Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_Success, StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        private void MapearDocumentoBase(DocDocumento dest, DocDocumento source)
        {
            dest.IdCategoria = source.IdCategoria;
            dest.Ativo = source.Ativo;
            dest.ConteudoDocumento = source.ConteudoDocumento;
            dest.CorRisco = source.CorRisco;
            dest.DtAlteracao = DateTime.Now;
            dest.DtAprovacao = source.DtAprovacao;
            dest.DtEmissao = source.DtEmissao;
            dest.DtInclusao = source.DtInclusao;
            dest.DtNotificacao = source.DtNotificacao;
            dest.DtPedidoAprovacao = source.DtPedidoAprovacao;
            dest.DtPedidoVerificacao = source.DtPedidoVerificacao;
            dest.DtVencimento = source.DtVencimento;
            dest.DtVerificacao = source.DtVerificacao;
            dest.EntradaTextoDoc = source.EntradaTextoDoc;
            dest.FlRevisaoPeriodica = source.FlRevisaoPeriodica;
            dest.FlStatus = source.FlStatus;
            dest.FluxoDoc = source.FluxoDoc;
            dest.FlWorkFlow = source.FlWorkFlow;
            //dest.IdDocExterno = source.IdDocExterno;
            dest.IdDocIdentificador = source.IdDocIdentificador;
            dest.IdElaborador = source.IdElaborador;
            dest.IdGestaoDeRisco = source.IdGestaoDeRisco > 0 ? source.IdGestaoDeRisco : null;
            dest.IdLicenca = source.IdLicenca;
            dest.IdProcesso = source.IdProcesso;
            dest.IdSigla = source.IdSigla;
            dest.IdSite = source.IdSite;
            dest.IdUsuarioAlteracao = Util.ObterCodigoUsuarioLogado();
            dest.NumeroDocumento = source.NumeroDocumento;
            dest.NuRevisao = source.NuRevisao;
            dest.PossuiGestaoRisco = source.PossuiGestaoRisco;
            dest.RecursoDoc = source.RecursoDoc;
            dest.SaidaTextoDoc = source.SaidaTextoDoc;
            dest.StatusRegistro = source.StatusRegistro;
            dest.TextoDoc = source.TextoDoc;
            dest.Titulo = source.Titulo;
            dest.XmlMetadata = source.XmlMetadata;
            //dest.DocExterno = source.DocExterno;

            //dest.DocUsuarioVerificaAprova = source.DocUsuarioVerificaAprova;
            //dest.Aprovadores = source.Aprovadores;
            //dest.Verificadores = source.Verificadores;

            if (dest.GestaoDeRisco != null)
            {
                if (source.GestaoDeRisco != null)
                {
                    dest.GestaoDeRisco.Causa = source.GestaoDeRisco.Causa;
                    dest.GestaoDeRisco.DsJustificativa = source.GestaoDeRisco.DsJustificativa;
                    dest.GestaoDeRisco.CriticidadeGestaoDeRisco = source.GestaoDeRisco.CriticidadeGestaoDeRisco;
                    dest.GestaoDeRisco.ResponsavelImplementar = source.GestaoDeRisco.ResponsavelImplementar;
                    dest.GestaoDeRisco.DescricaoRegistro = source.GestaoDeRisco.DescricaoRegistro;
                }
            }
            else
            {
                var numeroUltimoRegistro = 0;
                var listaGR = _registroConformidadeAppServico.ObtemListaRegistroConformidadePorSite(ViewBag.IdSite, "gr", ref numeroUltimoRegistro);
                numeroUltimoRegistro = numeroUltimoRegistro + 1;
                dest.GestaoDeRisco = source.GestaoDeRisco;
                if (dest.GestaoDeRisco != null)
                {
                    dest.GestaoDeRisco.NuRegistro = numeroUltimoRegistro;
                }
            }

            //Rotinas
            dest.Rotinas.AddRange(source.Rotinas.Where(s => s.IdDocRotina == 0));
            List<DocRotina> rotinas = dest.Rotinas.Where(s => !source.Rotinas.Any(a => s.IdDocRotina == a.IdDocRotina)).ToList();
            rotinas.ForEach(f => _documentoAppServico.RemoverGenerico(f));

            dest.Rotinas.ForEach(x =>
            {
                var itemAtualizar = source.Rotinas.Where(y => y.IdDocRotina == x.IdDocRotina).FirstOrDefault();
                x.Como = itemAtualizar.Como;
                x.OQue = itemAtualizar.OQue;
                x.Quem = itemAtualizar.Quem;
                x.Registro = itemAtualizar.Registro;
                x.Item = itemAtualizar.Item;
            });

            //Registros
            dest.Registros.AddRange(source.Registros.Where(s => s.IdDocRegistro == 0));
            List<DocRegistro> registros = dest.Registros.Where(s => !source.Registros.Any(a => s.IdDocRegistro == a.IdDocRegistro)).ToList();
            registros.ForEach(f => _documentoAppServico.RemoverGenerico(f));

            dest.Registros.ForEach(x =>
            {
                var itemAtualizar = source.Registros.Where(y => y.IdDocRegistro == x.IdDocRegistro).FirstOrDefault();
                x.Identificar = itemAtualizar.Identificar;
                x.Armazenar = itemAtualizar.Armazenar;
                x.Proteger = itemAtualizar.Proteger;
                x.Recuperar = itemAtualizar.Recuperar;
                x.Disposicao = itemAtualizar.Disposicao;
                x.Retencao = itemAtualizar.Retencao;
            });

            if (source.DocExterno != null && !string.IsNullOrEmpty(source.DocExterno.Anexo.ArquivoB64))
            {
                if (dest.DocExterno == null || dest.DocExterno.IdDocExterno == 0)
                {
                    dest.DocExterno = new DocExterno()
                    {
                        Anexo = new Anexo()
                    };
                    dest.DocExterno.LinkDocumentoExterno = "";
                    dest.DocExterno.Anexo.DtCriacao = DateTime.Now;
                }
                //dest.DocExterno.IdAnexo = source.DocExterno.IdAnexo;
                //dest.DocExterno.IdDocExterno = source.DocExterno.IdDocExterno;
                //dest.DocExterno.Anexo.IdAnexo = source.DocExterno.IdAnexo;
                dest.DocExterno.Anexo.Nome = source.DocExterno.Anexo.Extensao;
                dest.DocExterno.Anexo.Extensao = System.IO.Path.GetExtension(source.DocExterno.Anexo.Extensao);
                dest.DocExterno.Anexo.ArquivoB64 = source.DocExterno.Anexo.ArquivoB64;
                dest.DocExterno.Anexo.Arquivo = TransformaString64EmBase64(source.DocExterno.Anexo.ArquivoB64);
                dest.DocExterno.Anexo.DtAlteracao = DateTime.Now;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EnviarDocumentoParaVerificacao(DocDocumento documento)
        {
            try
            {
                List<string> erros = new List<string>();

                documento.DtAlteracao = DateTime.Now;

                _documentoServico.AssuntoObrigatorioEditarRevisao(documento, ref erros);


                if (erros.Count == 0)
                {
                    if (documento.FlWorkFlow)
                    {
                        documento.DocUsuarioVerificaAprova.AddRange(documento.Aprovadores);
                        documento.DocUsuarioVerificaAprova.AddRange(documento.Verificadores);
                    }

                    documento.XmlMetadata = Util.EscreveXML(documento.ConteudoDocumento);

                    AtualizarUsuarioCargosETemplatesDoDocumento(documento);
                    AtualizarAssuntos(documento);
                    AdicionaComentario(documento);

                    documento.FlStatus = (int)StatusDocumento.Verificacao;

                    Editar(documento);

                    _documentoAppServico.NotificacaoVerificadoresEmail(documento.NumeroDocumento, documento.IdSite, documento.Verificadores);

                }
                else
                {
                    return Json(new { StatusCode = (int)HttpStatusCode.InternalServerError, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = (int)HttpStatusCode.InternalServerError }, JsonRequestBehavior.AllowGet);
            }


            return Json(new { Success = Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_Success_Verificacao, StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EnviarParaElaboracao(DocDocumento documento)
        {
            try
            {
                documento.DocUsuarioVerificaAprova.AddRange(documento.Aprovadores);
                documento.DocUsuarioVerificaAprova.AddRange(documento.Verificadores);

                documento.XmlMetadata = Util.EscreveXML(documento.ConteudoDocumento);

                AtualizarAssuntos(documento);
                AdicionaComentario(documento);

                _docUsuarioVerificaAprovaAppServico.AtualizarParaEstadoInicialDoDocumento(documento.DocUsuarioVerificaAprova);

                _documentoAppServico.EnviarDocumentoParaElaboracao(documento);

                EnviaNotificacaoPorEmail(documento);

            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_Success_Eleboracao, StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EnviarParaAprovacao(DocDocumento documento)
        {
            try
            {
                documento.DocUsuarioVerificaAprova.AddRange(documento.Aprovadores);
                documento.DocUsuarioVerificaAprova.AddRange(documento.Verificadores);
                documento.XmlMetadata = Util.EscreveXML(documento.ConteudoDocumento);
                _documentoAppServico.VerificarDocumentoPorUsuario(documento, Util.ObterCodigoUsuarioLogado());

                AtualizarUsuarioCargosETemplatesDoDocumento(documento);
                AtualizarAssuntos(documento);
                AdicionaComentario(documento);


                if (_documentoAppServico.VerificadoPorTodos(documento))
                {
                    _documentoAppServico.EnviarDocumentoParaAprovacao(documento);
                    _documentoAppServico.NotificacaoAprovadoresEmail(documento.NumeroDocumento, documento.IdSite, documento.Aprovadores);
                }

            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = (int)HttpStatusCode.InternalServerError }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_Success_Aprovacao, StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EnviarParaAprovado(DocDocumento documento)
        {
            List<string> erros = new List<string>();

            _documentoServico.ValidoParaEtapaDeVerificacao(documento, ref erros);

            if (erros.Count == 0)
            {
                try
                {
                    Editar(documento);

                    documento = _documentoAppServico.GetById(documento.IdDocumento);

                    _documentoAppServico.AprovarDocumentoPorUsuario(documento, Util.ObterCodigoUsuarioLogado());
                    AdicionaComentario(documento);



                    if (_documentoAppServico.AprovadoPorTodos(documento))
                    {
                        _documentoAppServico.AprovarDocumento(documento);

                        foreach (var cargo in documento.DocCargo)
                        {
                            var usuarios = _usuarioAppServico.ObterUsuariosPorCargo(cargo.IdCargo);

                            _documentoAppServico.NotificacaoColaboradores(documento.NumeroDocumento, usuarios, documento.IdSite);
                        }
                    }

                }
                catch (Exception ex)
                {
                    GravaLog(ex);
                    return Json(new { StatusCode = (int)HttpStatusCode.InternalServerError }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { StatusCode = (int)HttpStatusCode.InternalServerError, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = Traducao.ControlDoc.ResourceControlDoc.ControlDoc_msg_Success_Aprovado, StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalvaPDF(int id)
        {
            return View();
        }

        //public ActionResult PDF(int id, int? idUsuarioDestino)
        //{
        //    var documento = _documentoAppServico.GetById(id);

        //    if (!String.IsNullOrWhiteSpace(documento.XmlMetadata))
        //        documento.ConteudoDocumento = (DocDocumentoXML)Util.LeXML(documento.XmlMetadata, typeof(DocDocumentoXML));

        //    List<string> erros = new List<string>();

        //    var controle = new ControleImpressao();
        //    controle.IdUsuarioDestino = idUsuarioDestino;
        //    controle.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
        //    controle.IdFuncionalidade = 2;
        //    controle.CodigoReferencia = id.ToString();
        //    controle.DataImpressao = DateTime.Now;
        //    controle.DataInclusao = DateTime.Now;
        //    controle.CopiaControlada = false;

        //    var viewpdf = new ViewAsPdf();

        //    viewpdf.ViewName = "PDF";
        //    viewpdf.Model = documento;

        //    if (idUsuarioDestino != null)
        //        controle.CopiaControlada = true;

        //    _controleImpressaoServico.Valido(controle, ref erros);

        //    if (erros.Count == 0)
        //        _controleImpressaoAppServico.Add(controle);

        //    ViewBag.CopiaControlada = controle.CopiaControlada;

        //    return viewpdf;
        //}

        private void EnviaNotificacaoPorEmail(DocDocumento doc)
        {
            if (doc.FlRevisaoPeriodica)
                _documentoAppServico.NotificacaoElaboradorEmail(doc.NumeroDocumento, Util.ObterSiteSelecionado(),
                                doc.IdElaborador,
                                doc.DtNotificacao.Value);
        }

        private void TrataEdicaoDoc(DocDocumento doc, ref List<string> erros)
        {
            doc.DtAlteracao = DateTime.Now;
            doc.XmlMetadata = Util.EscreveXML(doc.ConteudoDocumento);
            doc.DocUsuarioVerificaAprova.AddRange(doc.Aprovadores);
            doc.DocUsuarioVerificaAprova.AddRange(doc.Verificadores);


            if (doc.DocTemplate.Any(x => x.IdDocTemplate == 0))
                TrataObjetoApartirDoTipoTemplate(doc, Util.ObterCodigoUsuarioLogado(), ref erros);

            DefineStatus(doc);

        }

        private void TrataCriacaoDoc(DocDocumento doc, ref List<string> erros)
        {
            var idUsuarioLogado = Util.ObterCodigoUsuarioLogado();


            doc.NuRevisao = 0;
            doc.DtInclusao = DateTime.Now;
            doc.DtAlteracao = DateTime.Now;
            doc.IdUsuarioIncluiu = idUsuarioLogado;
            doc.XmlMetadata = Util.EscreveXML(doc.ConteudoDocumento);

            TrataObjetoApartirDoTipoTemplate(doc, idUsuarioLogado, ref erros);

            DefineStatus(doc);
        }

        private void TrataObjetoApartirDoTipoTemplate(DocDocumento doc, int idUsuarioLogado, ref List<string> erros)
        {
            TrataAnexos(doc);
            TrataCriacaoGr(doc, idUsuarioLogado, ref erros);
        }

        private void DefineStatus(DocDocumento doc)
        {
            if (!doc.FlWorkFlow)
            {
                doc.FlStatus = (int)StatusDocumento.Aprovado;

            }
            else
            {
                //[aqui] somente estas duas linhas
                if (doc.DocUsuarioVerificaAprova.Count == 0)
                {
                    //doc.DocUsuarioVerificaAprova.AddRange(doc.Aprovadores);
                    //doc.DocUsuarioVerificaAprova.AddRange(doc.Verificadores);]

                    doc.DocUsuarioVerificaAprova.AddRange(doc.Verificadores);
                    doc.DocUsuarioVerificaAprova.AddRange(doc.Aprovadores);
                    
                }
                    



                //if (doc.FlStatus == 0)
                //    doc.FlStatus = (int)StatusDocumento.Elaboracao;
            }
        }

        private void TrataAnexos(DocDocumento doc)
        {
            if (doc.DocTemplate.Any(x => x.TpTemplate == "L"))
            {
                doc.Licenca.Anexo.Tratar();
            }
            if (doc.DocTemplate.Any(x => x.TpTemplate == "DE"))
            {
                doc.DocExterno.Anexo.Tratar();
                doc.DocExterno.LinkDocumentoExterno = "";
            }
            if (doc.IdLicenca == 0)
            {
                doc.IdLicenca = null;
            }
            if (doc.IdDocExterno == 0)
            {
                doc.IdDocExterno = null;
            }
            if (doc.IdGestaoDeRisco == 0)
            {
                doc.IdGestaoDeRisco = null;
            }
        }

        private void TrataCriacaoGr(DocDocumento doc, int idUsuarioLogado, ref List<string> erros)
        {
            if (doc.DocTemplate.Any(x => x.TpTemplate == "RI"))
            {
                if (doc.GestaoDeRisco == null)
                {
                    doc.GestaoDeRisco = new RegistroConformidade();
                }

                doc.GestaoDeRisco.TipoRegistro = "gr";
                doc.GestaoDeRisco.IdEmissor = idUsuarioLogado;
                doc.GestaoDeRisco.IdUsuarioIncluiu = idUsuarioLogado;
                doc.GestaoDeRisco.IdUsuarioAlterou = idUsuarioLogado;
                doc.GestaoDeRisco.IdSite = Util.ObterSiteSelecionado();
                doc.GestaoDeRisco.EProcedente = false;
                doc.GestaoDeRisco.StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata;
                doc.GestaoDeRisco.FlDesbloqueado = doc.GestaoDeRisco.FlDesbloqueado > 0 ? (byte)0 : (byte)0;
                doc.GestaoDeRisco.IdResponsavelEtapa = doc.GestaoDeRisco.IdResponsavelInicarAcaoImediata;
                _registroConformidadeAppServico.GestaoDeRiscoValida(doc.GestaoDeRisco, ref erros);
            }
        }

        private bool ECoordenadorOuAdministrador(int perfil)
        {
            return perfil == (int)PerfisAcesso.Coordenador || perfil == (int)PerfisAcesso.Administrador;
        }

        private void AtualizarUsuarioCargosETemplatesDoDocumento(DocDocumento documento)
        {
            _docUsuarioVerificaAprovaAppServico
                        .AlterarUsuariosDoDocumento(documento.IdDocumento, documento.DocUsuarioVerificaAprova);

            _docCargoAppServico
                        .AlterarCargosDoDocumento(documento.IdDocumento, documento.DocCargo);

            _docTemplateAppServico
                        .AlterarTemplatesDocumento(documento.IdDocumento, documento.DocTemplate);

            _documentoAssuntoAppServico.AlterarAssuntosDocumento(documento.IdDocumento, documento.Assuntos);
        }

        private void AtualizarAssuntos(DocDocumento documento)
        {
            _documentoAssuntoAppServico.AlterarAssuntosDocumento(documento.IdDocumento, documento.Assuntos);
        }

        private void AdicionaComentario(DocDocumento documento)
        {
            if (documento.Comentarios.Any(comentario => comentario.Id == 0))
            {
                var novosComentarios = documento.Comentarios
                    .Where(comentario => comentario.Id == 0 && comentario.Descricao.Length > 0)
                    .ToList();

                novosComentarios.ForEach(comentario =>
                {
                    comentario.DataComentario = DateTime.Now;
                    comentario.IdDocumento = documento.IdDocumento;
                    comentario.IdUsuario = Util.ObterCodigoUsuarioLogado();

                    _documentoComentarioAppServico.Add(comentario);
                });
            }
        }

        private void setViewBagsListDocumentosAprovados()
        {
            ViewBag.FlStatus = (int)StatusDocumento.Aprovado;

            int perfil = Util.ObterPerfilUsuarioLogado();

            ViewBag.PodeRevisar = ECoordenadorOuAdministrador(perfil) ? true : _usuarioAppServico.ObterUsuariosPorFuncao(Util.ObterSiteSelecionado(), _funcaoRevisar).Where(s => s.IdUsuario == Util.ObterCodigoUsuarioLogado()).FirstOrDefault() != null;
            ViewBag.PodeImprimir = ECoordenadorOuAdministrador(perfil) ? true : _usuarioAppServico.ObterUsuariosPorFuncao(Util.ObterSiteSelecionado(), _funcaoImprimir).Where(s => s.IdUsuario == Util.ObterCodigoUsuarioLogado()).FirstOrDefault() != null;

            ViewBag.EstaEmElaboracao = false;
        }

        private void setViewBagsPorEtapaDocumento(DocDocumento documento, int usuario)
        {
            var podeVerificar = documento.DocUsuarioVerificaAprova.Where(x => x.IdUsuario == usuario && x.TpEtapa == "V").FirstOrDefault() != null;
            var podeAprovar = documento.DocUsuarioVerificaAprova.Where(x => x.IdUsuario == usuario && x.TpEtapa == "A").FirstOrDefault() != null;

            switch (documento.FlStatus)
            {
                case (int)StatusDocumento.Verificacao:
                    ViewBag.PodeVerificarDocumento = podeVerificar;
                    break;
                case (int)StatusDocumento.Aprovacao:
                    ViewBag.PodeAprovarDocumento = podeAprovar;
                    break;
                default:
                    break;
            }
        }
    }
}