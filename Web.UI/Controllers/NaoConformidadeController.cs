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
using ApplicationService.Enum;
using Rotativa;
using Rotativa.Options;
using System.Configuration;
using ApplicationService.Entidade;
using Web.UI.Models;
using System.Data;
using System.Diagnostics;

namespace Web.UI.Controllers
{
    //[ProcessoSelecionado]
    [VerificaIntegridadeLogin]
    public class NaoConformidadeController : BaseController
    {
        private readonly IRegistroConformidadesAppServico _registroConformidadesAppServico;
        private readonly IRegistroAcaoImediataServico _registroRegistroAcaoImediataServico;
        private readonly IRegistroConformidadesServico _registroConformidadesServico;
        private readonly IProcessoServico _processoServico;
        private readonly INotificacaoAppServico _notificacaoAppServico;


        private readonly ILogAppServico _logAppServico;

        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IClienteAppServico _clienteServico;

        private string _tipoRegistro = "nc";
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteAppServico;
        private readonly ISiteAppServico _siteService;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;

        private readonly IFilaEnvioServico _filaEnvioServico;

        public NaoConformidadeController(IRegistroConformidadesAppServico registroConformidadesAppServico,
            INotificacaoAppServico notificacaoAppServico,
            ISiteAppServico siteService,
            IUsuarioAppServico usuarioAppServico,
            ILogAppServico logAppServico,
            IRegistroConformidadesServico registroConformidadesServico,
            IProcessoServico processoServico,
            IProcessoAppServico processoAppServico,
            IClienteAppServico clienteServico,
            IUsuarioClienteSiteAppServico usuarioClienteAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico,
            IFilaEnvioServico filaEnvioServico,
           IRegistroAcaoImediataServico registroRegistroAcaoImediataServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _registroConformidadesAppServico = registroConformidadesAppServico;
            _notificacaoAppServico = notificacaoAppServico;
            _logAppServico = logAppServico;
            _siteService = siteService;
            _usuarioAppServico = usuarioAppServico;
            _registroConformidadesServico = registroConformidadesServico;
            _processoServico = processoServico;
            //ViewBag.ProcessoSelecionado = Util.ObterProcessoSelecionado();
            _processoAppServico = processoAppServico;
            _clienteServico = clienteServico;
            _usuarioClienteAppServico = usuarioClienteAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _filaEnvioServico = filaEnvioServico;
            _registroRegistroAcaoImediataServico = registroRegistroAcaoImediataServico;
        }

        // GET: NaoConformidade
        public ActionResult Index()
        {
            ViewBag.IdUsuarioLogado = Util.ObterCodigoUsuarioLogado();
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.TiposNaoConformidade = RetornarTiposNaoConformidade();

            var numeroUltimoRegistro = 0;
            //var idProcesso = Util.ObterProcessoSelecionado();            //var idProcesso = Util.ObterProcessoSelecionado();
            var listaNC = _registroConformidadesAppServico.ObtemListaRegistroConformidadePorSite(ViewBag.IdSite, _tipoRegistro, ref numeroUltimoRegistro);

            ViewBag.UltimoRegistro = numeroUltimoRegistro;
            return View(listaNC);
        }
        [AutorizacaoUsuario((int)FuncoesNaoConformidade.Cadastrar, (int)Funcionalidades.NaoConformidade)]
        public ActionResult Criar()
        {
            var naoConformidade = new RegistroConformidade();
            naoConformidade.Processo = new Processo();
            naoConformidade.Emissor = new Usuario();
            naoConformidade.ResponsavelInicarAcaoImediata = new Usuario();
            naoConformidade.TipoNaoConformidade = new ControladorCategoria();

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.Arquivos = new List<string>();
            ViewBag.IdCliente = Util.ObterClienteSelecionado();
            ViewBag.StatusEtapa = 0;

            ViewBag.TipoRegistro = _tipoRegistro;
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.UsuarioLogado = Util.ObterUsuario();

            ViewBag.IdUsuarioLogado = Util.ObterCodigoUsuarioLogado();
            ViewBag.Site = Util.ObterSiteSelecionado();
            //ViewBag.IdProcesso = Util.ObterProcessoSelecionado();

            ViewBag.NomeUsuario = Util.ObterUsuario().Nome;
            //ViewBag.NomeProcesso = _processoServico.GetProcessoById(Util.ObterProcessoSelecionado()).Nome;

            return View(naoConformidade);
        }

        public ActionResult FiltroGrafico()
        {
            return PartialView();
        }


        public ActionResult Grafico(int tipoGrafico, int tipoNaoConformidade, string dtDe, string dtAte, int estiloGrafico)
        {
            var diaDe = 1;
            var mesDe = Convert.ToInt32(dtDe.Substring(0, 2));
            var anoDe = Convert.ToInt32(dtDe.Substring(2, 4));

            var mesAte = Convert.ToInt32(dtAte.Substring(0, 2));
            var anoAte = Convert.ToInt32(dtAte.Substring(2, 4));
            var diaAte = DateTime.DaysInMonth(anoAte, mesAte);

            var dataDe = new DateTime(anoDe, mesDe, diaDe);
            var dataAte = new DateTime(anoAte, mesAte, diaAte);

            ViewBag.Title = ObterNomeGrafico(tipoGrafico);


            var parametros = new Dictionary<string, string>();
            parametros.Add("Tipo não conformidade", (tipoNaoConformidade > 0 ? _controladorCategoriasServico.GetById(tipoNaoConformidade).Descricao : "Todos"));
            parametros.Add("De", dataDe.ToString("MM/yyyy"));
            parametros.Add("Até", dataAte.ToString("MM/yyyy"));

            return View(new GraficoNaoConformidadeViewModel { EstiloGrafico = estiloGrafico, DtDe = dataDe, DtAte = dataAte, IdTipoGrafico = tipoGrafico, IdTipoNaoConformidade = tipoNaoConformidade, Parametros = parametros });
        }

        private string ObterNomeGrafico(int tipoGrafico)
        {
            string retorno = "";


            switch (tipoGrafico)
            {
                case 1:
                    retorno = "Total de NCs geradas x Mês";
                    break;
                case 2:
                    retorno = "Total de NCs geradas x NCs abertas x NCs fechadas";
                    break;
                case 3:
                    retorno = "Total de NCs geradas x Tipo de NC";
                    break;
                case 4:
                    retorno = "Total de NCs geradas x Total de NC com AC";
                    break;
                case 5:
                    retorno = "Total de NCs geradas x Departamento";
                    break;
                case 6:
                    retorno = "Total de NCs geradas x Unidade";
                    break;
                case 7:
                    retorno = "Comparativo do total de NCs gerado mês a mês x ano a ano";
                    break;
                default:
                    break;
            }

            return retorno;
        }

        private SelectList RetornarTiposNaoConformidade()
        {
            var idSite = Util.ObterSiteSelecionado();
            var listaAtivos = _controladorCategoriasServico.ListaAtivos("tnc", idSite);

            if (!listaAtivos.Select(x => x.Descricao).Contains("Auditoria"))
            {
                _controladorCategoriasServico.Add(new ControladorCategoria
                {
                    IdSite = idSite,
                    Descricao = "Auditoria",
                    TipoTabela = "tnc",
                    Ativo = true
                });

                listaAtivos = _controladorCategoriasServico.ListaAtivos("tnc", idSite);
            }

            return new SelectList(listaAtivos.OrderBy(x => x.Descricao).ToList(), "IdControladorCategoria", "Descricao");

        }

        public ActionResult PDF(int id)
        {
            var idSite = Util.ObterSiteSelecionado();
            ViewBag.IdSite = idSite;
            ViewBag.UsuarioLogado = Util.ObterUsuario();
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.IdCliente = Util.ObterClienteSelecionado();
            ViewBag.NomeUsuario = Util.ObterUsuario().Nome;
            //ViewBag.NomeProcesso = _processoServico.GetProcessoById(Util.ObterProcessoSelecionado()).Nome;


            var naoConformidade = _registroConformidadesAppServico.GetById(id);

            naoConformidade.ArquivosDeEvidenciaAux.AddRange(naoConformidade.ArquivosDeEvidencia.Select(x => x.Anexo));

            if (naoConformidade.AcoesImediatas.Count > 0)
            {
                if (naoConformidade.AcoesImediatas.Any(x => x.ArquivoEvidencia.Count > 0))
                {
                    var listaAnexo = naoConformidade.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0);

                    listaAnexo.ToList().ForEach(x =>
                    {
                        x.ArquivoEvidenciaAux = x.ArquivoEvidencia.FirstOrDefault().Anexo;
                    });
                }
            }

            if (naoConformidade.IdNuRegistroFilho != null)
            {
                ViewBag.AcaoCorretiva = _registroConformidadesAppServico.GetAll()
                    .FirstOrDefault(x => x.IdSite == naoConformidade.IdSite && x.TipoRegistro == "ac" && x.NuRegistro == naoConformidade.IdNuRegistroFilho);
            }
            ViewBag.IdProcesso = naoConformidade.IdProcesso;
            ViewBag.StatusEtapa = naoConformidade.StatusEtapa;

            //if ((naoConformidade.StatusRegistro == 0) && (naoConformidade.IdEmissor == Util.ObterCodigoUsuarioLogado()))
            //{
            //    ViewBag.ScriptCall = "sim";
            //}

            var usuarioClienteApp = _usuarioClienteAppServico.Get(s => s.IdSite == idSite);
            var clienteLogoAux = usuarioClienteApp.FirstOrDefault().Cliente.ClienteLogo.FirstOrDefault().Anexo;

            ViewBag.LogoCliente = Convert.ToBase64String(clienteLogoAux.Arquivo);

            //LayoutImpressaoViewModel model = new LayoutImpressaoViewModel()
            //{
            //    LogoCliente = Convert.ToBase64String(clienteLogoAux.Arquivo),
            //    Documento = documento,
            //    IsImpressaoControlada = controlada
            //};


            var pdf = new ViewAsPdf
            {
                ViewName = "PDF",
                Model = naoConformidade,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "Não Conformidade " + naoConformidade.IdRegistroConformidade + ".pdf"
            };

            return pdf;

            //return View("PDF", naoConformidade);
        }

        public ActionResult Exibir(int id)
        {
            //var analiseCritica = _registroConformidadesAppServico.GetById(id);

            ////var pdf = new ViewAsPdf
            ////{
            ////    ViewName = "Criar",
            ////    Model = analiseCritica,
            ////    PageOrientation = Orientation.Portrait,
            ////    PageSize = Size.A4,
            ////    PageMargins = new Margins(10, 15, 10, 15),
            ////    FileName = "NaoConformidade.pdf"
            ////};

            //return View("Exibir", analiseCritica);
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.UsuarioLogado = Util.ObterUsuario();
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.IdCliente = Util.ObterClienteSelecionado();
            ViewBag.NomeUsuario = Util.ObterUsuario().Nome;
            //ViewBag.NomeProcesso = _processoServico.GetProcessoById(Util.ObterProcessoSelecionado()).Nome;
            ViewBag.IdDocumento = id;

            var naoConformidade = _registroConformidadesAppServico.GetById(id);

            naoConformidade.ArquivosDeEvidenciaAux.AddRange(naoConformidade.ArquivosDeEvidencia.Select(x => x.Anexo));

            if (naoConformidade.AcoesImediatas.Count > 0)
            {
                if (naoConformidade.AcoesImediatas.Any(x => x.ArquivoEvidencia.Count > 0))
                {
                    var listaAnexo = naoConformidade.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0);

                    listaAnexo.ToList().ForEach(x =>
                    {
                        x.ArquivoEvidenciaAux = x.ArquivoEvidencia.FirstOrDefault().Anexo;
                    });
                }
            }

            if (naoConformidade.IdNuRegistroFilho != null)
            {
                ViewBag.AcaoCorretiva = _registroConformidadesAppServico.GetAll()
                    .FirstOrDefault(x => x.IdSite == naoConformidade.IdSite && x.TipoRegistro == "ac" && x.NuRegistro == naoConformidade.IdNuRegistroFilho);
            }
            ViewBag.IdProcesso = naoConformidade.IdProcesso;
            ViewBag.StatusEtapa = naoConformidade.StatusEtapa;

            //if ((naoConformidade.StatusRegistro == 0) && (naoConformidade.IdEmissor == Util.ObterCodigoUsuarioLogado()))
            //{
            //    ViewBag.ScriptCall = "sim";
            //}

            return View("Exibir", naoConformidade);
        }


        public ActionResult Exibir2(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.UsuarioLogado = Util.ObterUsuario();
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.IdCliente = Util.ObterClienteSelecionado();
            ViewBag.NomeUsuario = Util.ObterUsuario().Nome;
            //ViewBag.NomeProcesso = _processoServico.GetProcessoById(Util.ObterProcessoSelecionado()).Nome;


            var naoConformidade = _registroConformidadesAppServico.GetById(id);

            naoConformidade.ArquivosDeEvidenciaAux.AddRange(naoConformidade.ArquivosDeEvidencia.Select(x => x.Anexo));

            if (naoConformidade.AcoesImediatas.Count > 0)
            {
                if (naoConformidade.AcoesImediatas.Any(x => x.ArquivoEvidencia.Count > 0))
                {
                    var listaAnexo = naoConformidade.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0);

                    listaAnexo.ToList().ForEach(x =>
                    {
                        x.ArquivoEvidenciaAux = x.ArquivoEvidencia.FirstOrDefault().Anexo;
                    });
                }
            }

            if (naoConformidade.IdNuRegistroFilho != null)
            {
                ViewBag.AcaoCorretiva = _registroConformidadesAppServico.GetAll()
                    .FirstOrDefault(x => x.IdSite == naoConformidade.IdSite && x.TipoRegistro == "ac" && x.NuRegistro == naoConformidade.IdNuRegistroFilho);
            }
            ViewBag.IdProcesso = naoConformidade.IdProcesso;
            ViewBag.StatusEtapa = naoConformidade.StatusEtapa;

            //if ((naoConformidade.StatusRegistro == 0) && (naoConformidade.IdEmissor == Util.ObterCodigoUsuarioLogado()))
            //{
            //    ViewBag.ScriptCall = "sim";
            //}

            return View("Exibir", naoConformidade);
        }


        public ActionResult Editar(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.UsuarioLogado = Util.ObterUsuario();
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.IdCliente = Util.ObterClienteSelecionado();
            ViewBag.NomeUsuario = Util.ObterUsuario().Nome;
            //ViewBag.NomeProcesso = _processoServico.GetProcessoById(Util.ObterProcessoSelecionado()).Nome;
            ViewBag.IdDocumento = id;

            var naoConformidade = _registroConformidadesAppServico.GetById(id);

            naoConformidade.ArquivosDeEvidenciaAux.AddRange(naoConformidade.ArquivosDeEvidencia.Select(x => x.Anexo));

            if (naoConformidade.AcoesImediatas.Count > 0)
            {
                if (naoConformidade.AcoesImediatas.Any(x => x.ArquivoEvidencia.Count > 0))
                {
                    var listaAnexo = naoConformidade.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0);

                    listaAnexo.ToList().ForEach(x =>
                    {
                        x.ArquivoEvidenciaAux = x.ArquivoEvidencia.FirstOrDefault().Anexo;
                    });
                }
                //// Remove anexos para não confirmados
                //if (naoConformidade.StatusEtapa == 2)
                //    foreach (var item in naoConformidade.AcoesImediatas)
                //    {
                //        if (item.Aprovado == false)
                //        {
                //            naoConformidade.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata).ArquivoEvidenciaAux = null;
                //            naoConformidade.AcoesImediatas.FirstOrDefault(x => x.IdAcaoImediata == item.IdAcaoImediata).ArquivoEvidencia = new List<ArquivoDeEvidenciaAcaoImediata>();
                //        }
                //    }

            }



            if (naoConformidade.IdNuRegistroFilho != null)
            {
                ViewBag.AcaoCorretiva = _registroConformidadesAppServico.GetAll()
                    .FirstOrDefault(x => x.IdSite == naoConformidade.IdSite && x.TipoRegistro == "ac" && x.NuRegistro == naoConformidade.IdNuRegistroFilho);
            }
            ViewBag.IdProcesso = naoConformidade.IdProcesso;
            ViewBag.StatusEtapa = naoConformidade.StatusEtapa;

            //if ((naoConformidade.StatusRegistro == 0) && (naoConformidade.IdEmissor == Util.ObterCodigoUsuarioLogado()))
            //{
            //    ViewBag.ScriptCall = "sim";
            //}

            return View("Criar", naoConformidade);
        }

        [HttpPost]
        [AutorizacaoUsuario((int)FuncoesNaoConformidade.Cadastrar, (int)Funcionalidades.NaoConformidade)]
        public JsonResult SalvarPrimeiraEtapa(RegistroConformidade naoConformidade)
        {

            var erros = new List<string>();
            try
            {
                TrataDadosParaCriacao(naoConformidade);

                _registroConformidadesServico.ValidarCampos(naoConformidade, ref erros);

                if (erros.Count != 0)
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    SalvarArquivoEvidencia(naoConformidade);
                    naoConformidade.StatusRegistro = 1;
                    naoConformidade = _registroConformidadesAppServico.SalvarPrimeiraEtapa(naoConformidade);
                    naoConformidade.Site = _siteService.GetById(naoConformidade.IdSite);

                    try
                    {
                        EnviarEmailResponsavel(naoConformidade);
                    }
                    catch (Exception ex)
                    {
                        GravaLog(ex);
                    }

                    erros = EnviarNotificacao(naoConformidade, erros);
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_criar_valid }, JsonRequestBehavior.AllowGet);

        }

        private void EnviarEmailResponsavel(RegistroConformidade naoConformidade)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);
            var urlAcesso = MontarUrlAcesso(naoConformidade.IdRegistroConformidade);

            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\NaoConformidadeResponsavel-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
            string template = System.IO.File.ReadAllText(path);
            string conteudo = template;

            conteudo = conteudo.Replace("#NomeCliente#", cliente.NmFantasia);
            conteudo = conteudo.Replace("#NuNaoConformidade#", naoConformidade.NuRegistro.ToString());
            conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);
            var responsavelAcaoImediata = _usuarioAppServico.GetById(naoConformidade.IdResponsavelInicarAcaoImediata.Value);
            Email _email = new Email();

            _email.Assunto = Traducao.ResourceNotificacaoMensagem.MsgNotificacaoNaoConformidade;
            _email.De = ConfigurationManager.AppSettings["EmailDE"];
            _email.Para = responsavelAcaoImediata.CdIdentificacao;
            _email.Conteudo = conteudo;
            _email.Servidor = ConfigurationManager.AppSettings["SMTPServer"];
            _email.Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            _email.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
            _email.Enviar();
        }


        private void SalvarArquivoEvidencia(RegistroConformidade nc)
        {
            if (nc.ArquivosDeEvidenciaAux.Count > 0)
            {
                nc.ArquivosDeEvidenciaAux.ForEach(arquivoEvidencia =>
                {

                    nc.ArquivosDeEvidencia.Add(new ArquivosDeEvidencia
                    {
                        Anexo = arquivoEvidencia,
                        RegistroConformidade = nc,
                        TipoRegistro = nc.TipoRegistro
                    });
                });
            }
        }

        private void TrataDadosParaCriacao(RegistroConformidade nc)
        {
            nc.TipoRegistro = _tipoRegistro;
            nc.StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata;
            nc.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
            nc.FlDesbloqueado = nc.FlDesbloqueado > 0 ? (byte)0 : (byte)0;

            if (nc.ArquivosDeEvidenciaAux.Count > 0)
            {
                nc.ArquivosDeEvidenciaAux.ForEach(arquivoEvidencia => arquivoEvidencia.Tratar());
            }
        }

        private void TrataDadosParaCriacao_Edicao(RegistroConformidade nc)
        {
            nc.TipoRegistro = _tipoRegistro;
            nc.StatusEtapa = (byte)EtapasRegistroConformidade.Implementacao;
            nc.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
            nc.FlDesbloqueado = nc.FlDesbloqueado > 0 ? (byte)0 : (byte)0;

            if (nc.ArquivosDeEvidenciaAux.Count > 0)
            {
                nc.ArquivosDeEvidenciaAux.ForEach(arquivoEvidencia => arquivoEvidencia.Tratar());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DestravarEtapa(int idNaoConformidade)
        {
            var erros = new List<string>();

            var naoConformidade = _registroConformidadesAppServico.GetById(idNaoConformidade);
            var idPerfil = Util.ObterPerfilUsuarioLogado();

            _registroConformidadesServico.ValidaDestravamento(idPerfil, ref erros);

            if (erros.Count == 0)
            {
                var result = _registroConformidadesAppServico.DestravarEtapa(naoConformidade);
                return Json(new { StatusCode = (int)HttpStatusCode.OK });
            }
            else
            {
                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros });
            }
        }

        [HttpPost]
        public JsonResult SalvarSegundaEtapa(RegistroConformidade naoConformidade)
        {

            var erros = new List<string>();

            try
            {
                var responsavelAcaoCorrecao = _registroConformidadesAppServico.Get(x => x.IdRegistroPai == naoConformidade.IdRegistroConformidade && x.TipoRegistro == "ac").OrderByDescending(x => x.IdRegistroConformidade).FirstOrDefault();
                var idResponsavelAcaoCorrecao = (responsavelAcaoCorrecao != null ? responsavelAcaoCorrecao.IdResponsavelInicarAcaoImediata : 0);

                naoConformidade.IdUsuarioAlterou = Util.ObterCodigoUsuarioLogado();
                naoConformidade.FlDesbloqueado = naoConformidade.FlDesbloqueado > 0 ? (byte)0 : (byte)0;
                naoConformidade.TipoRegistro = _tipoRegistro;
                naoConformidade.StatusRegistro = 1;

                _registroConformidadesServico.ValidaNaoConformidade(naoConformidade, Util.ObterCodigoUsuarioLogado(), ref erros);
                
                if (naoConformidade.EProcedente == null)
                {
                    erros.Add(Traducao.Resource.MsgCampoProcedente);
                }


                var usuario = Util.ObterUsuario();


                for (int i = 0; i < naoConformidade.AcoesImediatas.Count; i++)
                {
                    if (naoConformidade.EProcedente == true && naoConformidade.StatusEtapa == 3 && naoConformidade.AcoesImediatas[i].Aprovado == false && (string.IsNullOrEmpty(naoConformidade.AcoesImediatas[i].Motivo) || string.IsNullOrEmpty(naoConformidade.AcoesImediatas[i].Orientacao)))
                    {
                        erros.Add("Favor preencher Motivo e Orientação.");
                    }
                    if (naoConformidade.AcoesImediatas[i].Motivo != null || naoConformidade.AcoesImediatas[i].Orientacao != null)
                    {
                        ComentarioAcaoImediata ca = new ComentarioAcaoImediata();
                        ca.Motivo = naoConformidade.AcoesImediatas[i].Motivo;
                        ca.Orientacao = naoConformidade.AcoesImediatas[i].Orientacao;
                        ca.DataComentario = DateTime.Now.ToString();
                        ca.UsuarioComentario = usuario.Nome;



                        naoConformidade.AcoesImediatas[i].ComentariosAcaoImediata.Add(ca);
                    }
                    //if(naoConformidade.AcoesImediatas[i].ArquivoEvidenciaAux.ArquivoB64 == null || naoConformidade.AcoesImediatas[i].ArquivoEvidenciaAux.ArquivoB64 == "undefined")
                    //{
                    //    erros.Add("Não é possível incluir um arquivo vazio como anexo.");
                    //}
                }
                //int count = 0;
                //RegistroAcaoImediata acaoImediata = new RegistroAcaoImediata();
                //foreach (var item in naoConformidade.AcoesImediatas)
                //{
                //    count++;
                //    ComentarioAcaoImediata ca = new ComentarioAcaoImediata();
                //    acaoImediata = item;
                //    ca.Motivo = item.Motivo;
                //    ca.Orientacao = item.Orientacao;
                //    acaoImediata.ComentariosAcaoImediata.Add(ca);
                //}
                //naoConformidade.AcoesImediatas = acaoImediata;

                if (erros.Count == 0)
                {
                    var acoesImediatasNova = naoConformidade.AcoesImediatas.Where(x => x.IdAcaoImediata == 0).ToList();

                    var acoesEfetivadas = naoConformidade.AcoesImediatas.Where(x => x.DtEfetivaImplementacao != null).ToList();
                    acoesEfetivadas.ToList().ForEach(acao =>
                    {
                        acao.IdRegistroConformidade = naoConformidade.IdRegistroConformidade;
                    });

                    RemoverFilaEnvioAcoesEfetivadas(acoesEfetivadas);

                    if (acoesImediatasNova.Count > 0)
                    {
                        EnfileirarEmailsAcaoImediata(acoesImediatasNova, naoConformidade);
                    }

                    if (1 == 0)
                    {
                        TrataDadosParaCriacao_Edicao(naoConformidade);
                        SalvarArquivoEvidencia(naoConformidade);
                    }
                    AtualizarDatasAgendadas(naoConformidade);
                    
                    naoConformidade = _registroConformidadesAppServico.SalvarSegundaEtapa(naoConformidade, Funcionalidades.NaoConformidade);

                    if (naoConformidade.EProcedente == true)
                        erros = EnviarNotificacao(naoConformidade, erros);

                    var acoesIneficazes = naoConformidade.AcoesImediatas.Where(x => x.Aprovado == false).ToList();
                    if (acoesIneficazes.Count > 0)
                    {
                        EnviarEmailAcaoIneficaz(naoConformidade, acoesIneficazes);
                    }


                    if (naoConformidade.NecessitaAcaoCorretiva != null && naoConformidade.NecessitaAcaoCorretiva.Value)
                    {
                        if (idResponsavelAcaoCorrecao != naoConformidade.IdResponsavelPorIniciarTratativaAcaoCorretiva)
                        {
                            var acaoCorretiva = _registroConformidadesAppServico.Get(x => x.IdRegistroPai == naoConformidade.IdRegistroConformidade && x.NuRegistro == naoConformidade.IdNuRegistroFilho.Value).FirstOrDefault();
                            EnviarEmailAcaoCorretivaResponsavel(naoConformidade, acaoCorretiva);
                        }
                    }
                }
                else
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //foreach (var acaoImediata in naoConformidade.AcoesImediatas)
                //{
                //    Debug.WriteLine($"acaoImediata: {acaoImediata.IdAcaoImediata} - idRegistro: {acaoImediata.IdRegistroConformidade}");
                //}

                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_save_valid }, JsonRequestBehavior.AllowGet);

        }

        private void AtualizarDatasAgendadas(RegistroConformidade naoConformidade)
        {
            var acoes = naoConformidade.AcoesImediatas.Where(x => x.DtEfetivaImplementacao == null && x.IdAcaoImediata > 0 && x.IdFilaEnvio != null).ToList();
            var acoesEnfileirar = new List<RegistroAcaoImediata>();

            foreach (var acao in acoes)
            {
                var filaEnvio = _filaEnvioServico.ObterPorId(acao.IdFilaEnvio.Value);

                if(filaEnvio != null)
                {
                    if(!filaEnvio.Enviado)
                    {
                        filaEnvio.DataAgendado = acao.DtPrazoImplementacao.Value.AddDays(1);
                    }
                    else
                    {
                        acoesEnfileirar.Add(acao);
                    }
                }

                _filaEnvioServico.Atualizar(filaEnvio);
            }

            EnfileirarEmailsAcaoImediata(acoesEnfileirar, naoConformidade);

        }

        private void EnviarEmailAcaoCorretivaResponsavel(RegistroConformidade naoConformidade, RegistroConformidade acaoCorretiva)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);
            var urlAcesso = MontarUrlAcessoAcaoCorretiva(acaoCorretiva.IdRegistroConformidade);
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\AcaoCorretivaResponsavel-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

            try
            {
                var usuario = _usuarioAppServico.GetById(naoConformidade.IdResponsavelPorIniciarTratativaAcaoCorretiva);
                string template = System.IO.File.ReadAllText(path);
                string conteudo = template;

                conteudo = conteudo.Replace("#NuRegistro#", naoConformidade.IdNuRegistroFilho.ToString());
                conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);

                Email _email = new Email();

                _email.Assunto = Traducao.ResourceNotificacaoMensagem.msgNotificacaoAcaoCorretiva;
                _email.De = ConfigurationManager.AppSettings["EmailDE"];
                _email.Para = usuario.CdIdentificacao;
                _email.Conteudo = conteudo;
                _email.Servidor = ConfigurationManager.AppSettings["SMTPServer"];
                _email.Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                _email.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
                _email.Enviar();
            }
            catch (Exception ex)
            {
                GravaLog(ex);
            }
        }

        private void RemoverFilaEnvioAcoesEfetivadas(List<RegistroAcaoImediata> acoesEfetivadas)
        {
            foreach (var acao in acoesEfetivadas)
            {

                if (acao.IdFilaEnvio != null)
                {
                    var filaEnvio = _filaEnvioServico.ObterPorId(acao.IdFilaEnvio.Value);
                    if (filaEnvio != null && !filaEnvio.Enviado)
                    {
                        acao.IdFilaEnvio = null;
                        _registroRegistroAcaoImediataServico.Update(acao);
                        _filaEnvioServico.Apagar(filaEnvio);
                    }
                }
            }
        }

        private void EnfileirarEmailsAcaoImediata(List<RegistroAcaoImediata> acoesImediatasNova, RegistroConformidade naoConformidade)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);
            var urlAcesso = MontarUrlAcesso(naoConformidade.IdRegistroConformidade);
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\NaoConformidadeAcaoDataImplementacao-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

            foreach (var acao in acoesImediatasNova)
            {
                try
                {
                    var destinatario = _usuarioAppServico.GetById(acao.IdResponsavelImplementar.Value).CdIdentificacao;
                    string template = System.IO.File.ReadAllText(path);
                    string conteudo = template;

                    conteudo = conteudo.Replace("#NomeCliente#", cliente.NmFantasia);
                    conteudo = conteudo.Replace("#NuNaoConformidade#", naoConformidade.NuRegistro.ToString());
                    conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);

                    var filaEnvio = new FilaEnvio();
                    filaEnvio.Assunto = "Notificação de Não Conformidade";
                    filaEnvio.DataAgendado = acao.DtPrazoImplementacao.Value.AddDays(1);
                    filaEnvio.DataInclusao = DateTime.Now;
                    filaEnvio.Destinatario = destinatario;
                    filaEnvio.Enviado = false;
                    filaEnvio.Mensagem = conteudo;

                    _filaEnvioServico.Enfileirar(filaEnvio);

                    acao.IdFilaEnvio = filaEnvio.Id;
                }
                catch (Exception ex)
                {
                    GravaLog(ex);
                }
            }
        }
        [HttpPost]
        public JsonResult ListarAcaoImediataComentarios(int idAcaoImediata)
        {
            var teste = _registroRegistroAcaoImediataServico.GetById(idAcaoImediata);

            //var ultimaDataEmissao = _registroRegistroAcaoImediataServico.Update//ObtemUltimaDataEmissao(site, _tipoRegistro).ToString(Traducao.Resource.dateFormat);

            List<ComentarioAcaoImediata> lista = new List<ComentarioAcaoImediata>();
            foreach (var item in teste.ComentariosAcaoImediata)
            {
                ComentarioAcaoImediata ca = new ComentarioAcaoImediata();
                ca.Motivo = item.Motivo;
                ca.Orientacao = item.Orientacao;
                ca.DataComentario = item.DataComentario;
                ca.UsuarioComentario = item.UsuarioComentario;

                lista.Add(ca);
            }


            return Json(new { StatusCode = (int)HttpStatusCode.OK, Comentarios = lista }, JsonRequestBehavior.AllowGet);
            //return null;
        }





        [HttpGet]
        public ActionResult ObterNaoConformidade(int id)
        {
            var erros = new List<string>();

            var naoConformidade = _registroConformidadesAppServico.GetById(id);
            ViewBag.UsuarioLogado = Util.ObterCodigoUsuarioLogado();

            return View("Editar", naoConformidade);
        }

        [HttpGet]
        public JsonResult ObtemUltimaDataEmissao(int site)
        {
            var ultimaDataEmissao = _registroConformidadesAppServico.ObtemUltimaDataEmissao(site, _tipoRegistro).ToString(Traducao.Resource.dateFormat);

            return Json(new { StatusCode = (int)HttpStatusCode.OK, UltimaDataEmissao = ultimaDataEmissao }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoverComAcaoConformidade(int idNaoConformidade)
        {

            var erros = new List<string>();

            try
            {
                var naoConformidade = _registroConformidadesAppServico.GetById(idNaoConformidade);
                var acaoCorretiva = _registroConformidadesAppServico.ObtemAcaoConformidadePorNaoConformidade(naoConformidade);

                if (UtilsServico.EstaPreenchido(acaoCorretiva))
                {
                    _registroConformidadesServico.ValidoParaExclusaoAcaoCorretiva(acaoCorretiva, ref erros);
                }
                _registroConformidadesServico.ValidoParaExclusaoNaoConformidade(naoConformidade, ref erros);

                if (erros.Count == 0)
                {
                    _notificacaoAppServico.RemovePorFuncionalidade(Funcionalidades.NaoConformidade, naoConformidade.IdRegistroConformidade);

                    _registroConformidadesAppServico.Remove(naoConformidade);

                    if (UtilsServico.EstaPreenchido(acaoCorretiva))
                    {
                        _registroConformidadesAppServico.Remove(acaoCorretiva);
                    }
                }
                else
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.NaoConformidade.ResourceNaoConformidade.NC_msg_delete_valid }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult UploadArquivo(int id, string tipo, string acaoImediata = null)
        {
            var acaoImediataPath = acaoImediata != null ? acaoImediata + "/" : "";
            tipo = _tipoRegistro;
            try
            {
                var _nomeArquivo = Request.Files[0].FileName;

                string fullPath = Server.MapPath(string.Format($"/content/cliente/{Util.ObterClienteSelecionado()}/acoes/{tipo}/{acaoImediataPath}"));
                Util.VerificaDiretorio(fullPath);

                Request.Files[0].SaveAs(Server.MapPath(string.Format($"/content/cliente/{Util.ObterClienteSelecionado()}/acoes/{tipo}/{acaoImediataPath}{_nomeArquivo}")));

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Content(string.Format($"/content/cliente/{Util.ObterClienteSelecionado()}/acoes/{tipo}/{acaoImediataPath}{_nomeArquivo}"), "text/html");

            }
            catch (Exception ex)
            {
                GravaLog(ex);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Content(Traducao.Resource.MsgErroInclusaoArquivo, "text/html");
        }

        [HttpPost]
        public ActionResult RemoverArquivo(string arquivo, int id, string tipo, string campo)
        {
            tipo = _tipoRegistro;
            try
            {
                //TODO: CÓDIGO DE REMOÇÃO DO ARQUIVO
                //Server.MapPath(string.Format($"/content/cliente/{idSite}/acoes/{tipo}/{_nomeArquivo}"))

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Content(arquivo, "text/html");
            }
            catch (Exception ex)
            {
                GravaLog(ex);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Content(Traducao.Resource.MsgErroInclusaoArquivo, "text/html");
        }

        public ActionResult Imprimir(int id)
        {
            var analiseCritica = _registroConformidadesAppServico.GetById(id);

            var pdf = new ViewAsPdf
            {
                ViewName = "Criar",
                Model = analiseCritica,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "NaoConformidade.pdf"
            };

            return pdf;
        }

        private List<string> EnviarNotificacao(RegistroConformidade naoConformidade, List<string> erros)
        {
            try
            {
                var listaAcaoImediataUpdate = naoConformidade.AcoesImediatas.Where(x => x.Estado == EstadoObjetoEF.Modified);
                var acoesImediataSaoAtualizacao = listaAcaoImediataUpdate.FirstOrDefault() != null;
                var notificacao = new Notificacao($"Não Conformidade #{naoConformidade.NuRegistro}",
                                    null, DateTime.Now, (int)Funcionalidades.NaoConformidade,
                                    naoConformidade.IdProcesso, naoConformidade.IdRegistroConformidade,
                                    naoConformidade.IdSite, 0,
                                    ((char)TipoNotificacao.NotificacaoPorEmail).ToString(),
                                    0);

                if (naoConformidade.OStatusEAcaoImediata())
                {
                    naoConformidade.ResponsavelInicarAcaoImediata = _usuarioAppServico.GetById(naoConformidade.IdResponsavelInicarAcaoImediata.Value);
                    notificacao.IdUsuario = naoConformidade.IdResponsavelInicarAcaoImediata.Value;
                    notificacao.FlEtapa = naoConformidade.StatusEtapa.ToString();
                    _notificacaoAppServico.Add(notificacao);
                }
                else if (naoConformidade.OStatusEImplementacao() && acoesImediataSaoAtualizacao == false)
                {
                    notificacao = new Notificacao($"Não Conformidade #{naoConformidade.NuRegistro}",
                                    null, DateTime.Now, (int)Funcionalidades.NaoConformidade,
                                    naoConformidade.IdProcesso, naoConformidade.IdRegistroConformidade,
                                    naoConformidade.IdSite, 0,
                                    ((char)TipoNotificacao.NotificacaoPorEmail).ToString(),
                                    0);

                    naoConformidade.AcoesImediatas.ToList().ForEach(acaoImediata =>
                    {
                        acaoImediata.ResponsavelImplementar = _usuarioAppServico.GetById(acaoImediata.IdResponsavelImplementar.Value);
                        notificacao.IdUsuario = acaoImediata.IdResponsavelImplementar.Value;
                        notificacao.FlEtapa = naoConformidade.StatusEtapa.ToString();
                        _notificacaoAppServico.Add(notificacao);

                        try
                        {
                            EnviarEmailImplementacao(naoConformidade, acaoImediata.ResponsavelImplementar);
                        }
                        catch (Exception ex)
                        {
                            GravaLog(ex);
                        }

                    });

                }
                else if (naoConformidade.OStatusEReverificacao())
                {
                    naoConformidade.ResponsavelReverificador = _usuarioAppServico.GetById(naoConformidade.IdResponsavelReverificador.Value);
                    notificacao.IdUsuario = naoConformidade.IdResponsavelReverificador.Value;
                    notificacao.FlEtapa = naoConformidade.StatusEtapa.ToString();
                    _notificacaoAppServico.Add(notificacao);

                    try
                    {
                        EnviarEmailVerificacao(naoConformidade, naoConformidade.ResponsavelReverificador);
                    }
                    catch (Exception ex)
                    {
                        GravaLog(ex);
                    }
                }
            }
            catch
            {
                erros.Add(Traducao.ResourceNotificacaoMensagem.ErroAoNotificarResponsavelPelaProximaEtapa);

            }

            return erros;

        }

        private void EnviarEmailAcaoIneficaz(RegistroConformidade naoConformidade, List<RegistroAcaoImediata> acoesIneficazes)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);
            var urlAcesso = MontarUrlAcesso(naoConformidade.IdRegistroConformidade);
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\NaoConformidadeAcaoIneficaz-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

            foreach (var acao in acoesIneficazes)
            {
                try
                {
                    string template = System.IO.File.ReadAllText(path);
                    string conteudo = template;

                    conteudo = conteudo.Replace("#NomeCliente#", cliente.NmFantasia);
                    conteudo = conteudo.Replace("#NuNaoConformidade#", naoConformidade.NuRegistro.ToString());
                    conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);

                    Email _email = new Email();

                    _email.Assunto = Traducao.ResourceNotificacaoMensagem.MsgNotificacaoNaoConformidade;
                    _email.De = ConfigurationManager.AppSettings["EmailDE"];
                    _email.Para = acao.ResponsavelImplementar.CdIdentificacao;
                    _email.Conteudo = conteudo;
                    _email.Servidor = ConfigurationManager.AppSettings["SMTPServer"];
                    _email.Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    _email.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
                    _email.Enviar();
                }
                catch (Exception ex)
                {
                    GravaLog(ex);
                }
            }
        }

        private void EnviarEmailImplementacao(RegistroConformidade naoConformidade, Usuario usuario)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);
            //var usuarioNotificacao = _usuarioAppServico.GetById(naoConformidade.IdResponsavelEtapa.Value);
            var urlAcesso = MontarUrlAcesso(naoConformidade.IdRegistroConformidade);

            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\NaoConformidadeImplementacao-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
            string template = System.IO.File.ReadAllText(path);
            string conteudo = template;

            conteudo = conteudo.Replace("#NomeCliente#", cliente.NmFantasia);
            conteudo = conteudo.Replace("#NuNaoConformidade#", naoConformidade.NuRegistro.ToString());
            conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);

            Email _email = new Email();

            _email.Assunto = Traducao.ResourceNotificacaoMensagem.MsgNotificacaoNaoConformidade;
            _email.De = ConfigurationManager.AppSettings["EmailDE"];
            _email.Para = usuario.CdIdentificacao;
            _email.Conteudo = conteudo;
            _email.Servidor = ConfigurationManager.AppSettings["SMTPServer"];
            _email.Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            _email.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
            _email.Enviar();
        }

        private void EnviarEmailVerificacao(RegistroConformidade naoConformidade, Usuario usuario)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);
            //var usuarioNotificacao = _usuarioAppServico.GetById(naoConformidade.IdResponsavelEtapa.Value);
            var urlAcesso = MontarUrlAcesso(naoConformidade.IdRegistroConformidade);

            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\NaoConformidadeVerificacao-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
            string template = System.IO.File.ReadAllText(path);
            string conteudo = template;

            conteudo = conteudo.Replace("#NomeCliente#", cliente.NmFantasia);
            conteudo = conteudo.Replace("#NuNaoConformidade#", naoConformidade.NuRegistro.ToString());
            conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);

            Email _email = new Email();

            _email.Assunto = Traducao.ResourceNotificacaoMensagem.MsgNotificacaoNaoConformidade;
            _email.De = ConfigurationManager.AppSettings["EmailDE"];
            _email.Para = usuario.CdIdentificacao;
            _email.Conteudo = conteudo;
            _email.Servidor = ConfigurationManager.AppSettings["SMTPServer"];
            _email.Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            _email.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
            _email.Enviar();
        }

        private string MontarUrlAcesso(int idRegistroConformidade)
        {
            var dominio = "http://" + ConfigurationManager.AppSettings["Dominio"];

            return dominio + "NaoConformidade/Editar/" + idRegistroConformidade.ToString();
        }

        private string MontarUrlAcessoAcaoCorretiva(int idRegistro)
        {
            var dominio = "http://" + ConfigurationManager.AppSettings["Dominio"];

            return dominio + "AcaoCorretiva/Editar/" + idRegistro.ToString();
        }


        private void EnviaEmail(RegistroConformidade nc)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);

            var usuarioNotificacao = _usuarioAppServico.GetById(nc.IdResponsavelEtapa.Value);

            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\AteracaoStatusNaoConformidade-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
            string template = System.IO.File.ReadAllText(path);
            string conteudo = template;

            conteudo = conteudo.Replace("#NomeCliente#", cliente.NmFantasia);
            conteudo = conteudo.Replace("#NuNaoConformidade#", nc.NuRegistro.ToString());
            conteudo = conteudo.Replace("#NuRegistroConformidade#", nc.IdRegistroConformidade.ToString());

            Email _email = new Email();

            _email.Assunto = Traducao.ResourceNotificacaoMensagem.MsgNotificacaoNaoConformidade;
            _email.De = ConfigurationManager.AppSettings["EmailDE"];
            _email.Para = usuarioNotificacao.CdIdentificacao;
            _email.Conteudo = conteudo;
            _email.Servidor = ConfigurationManager.AppSettings["SMTPServer"];
            _email.Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            _email.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
            _email.Enviar();
        }


        [HttpPost]
        public JsonResult DestravarDocumento(string idNaoConformidade)
        {
            var erros = new List<string>();

            try
            {
                var gestaoDeRisco = _registroConformidadesAppServico.GetById(int.Parse(idNaoConformidade));
                gestaoDeRisco.StatusRegistro = 0;
                _registroConformidadesAppServico.Update(gestaoDeRisco);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_save_valid }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult ObterDadosGraficosBarra(int tipoGrafico, DateTime dtDe, DateTime dtAte, int? tipoNaoConformidade)
        {
            List<object[]> dataPoints = null;
            var dtDados = new DataTable();
            int? idTipoNaoConformidade = (tipoNaoConformidade == 0 ? null : tipoNaoConformidade);

            dtDados = _registroConformidadesServico.RetornarDadosGrafico(dtDe, dtAte, idTipoNaoConformidade, Util.ObterClienteSelecionado(), Util.ObterSiteSelecionado(), tipoGrafico);

            dataPoints = GerarDataPointsBarra(dtDados);

            return Json(dataPoints);
        }

        [HttpPost]
        public JsonResult ObterDadosGraficosPizza(int tipoGrafico, DateTime dtDe, DateTime dtAte, int? tipoNaoConformidade)
        {
            var dtDados = new DataTable();
            List<object> dataPoints = null;
            int? idTipoNaoConformidade = (tipoNaoConformidade == 0 ? null : tipoNaoConformidade);

            dtDados = _registroConformidadesServico.RetornarDadosGrafico(dtDe, dtAte, idTipoNaoConformidade, Util.ObterClienteSelecionado(), Util.ObterSiteSelecionado(), tipoGrafico);

            dataPoints = GerarDataPointsPizza(dtDados);

            return Json(dataPoints);
        }



        private List<object[]> GerarDataPointsBarra(DataTable dtDados)
        {
            List<object[]> dataPoints = new List<object[]>();


            bool barraUnica = dtDados.Rows.Count == 1;

            foreach (DataRow item in dtDados.Rows)
            {
                var data = new object[] { item["Rotulo"], item["Valor"] };

                dataPoints.Add(data);
            }

            //if (barraUnica)
            //    dataPoints.Add(new object[] { "", 0 });

            return dataPoints;
        }


        private List<object> GerarDataPointsPizza(DataTable dtDados)
        {
            List<object> dataPoints = new List<object>();

            foreach (DataRow item in dtDados.Rows)
            {
                var data = new { label = item["Rotulo"].ToString(), data = Convert.ToDecimal(item["Valor"]) };

                //Ajuste para gráfico 
                //if (item["Rotulo"].ToString() != "Total NCs")
                dataPoints.Add(data);
            }

            return dataPoints;
        }



    }
}