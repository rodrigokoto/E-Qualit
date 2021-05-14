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
using Rotativa.Options;
using Rotativa;
using System.Configuration;
using ApplicationService.Entidade;
using Web.UI.Models;

namespace Web.UI.Controllers
{
    //[ProcessoSelecionado]
    [VerificaIntegridadeLogin]
    [SitePossuiModulo(11)]
    public class GestaoDeRiscoController : BaseController
    {
        private readonly IAnaliseCriticaTemaAppServico _analiseCriticaTemaAppServico;
        private readonly IRegistroConformidadesAppServico _registroConformidadesAppServico;
        private readonly IRegistroConformidadesServico _registroConformidadesServico;
        private readonly IProcessoServico _processoServico;
        private readonly IClienteAppServico _clienteServico;
        private readonly INotificacaoAppServico _notificacaoAppServico;
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IRegistroAcaoImediataServico _registroRegistroAcaoImediataServico;


        private readonly IUsuarioAppServico _usuarioAppServico;

        private string _tipoRegistro = "gr";
        private readonly ISiteAppServico _siteService;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IFilaEnvioServico _filaEnvioServico;

        public GestaoDeRiscoController(
                        IAnaliseCriticaTemaAppServico analiseCriticaTemaAppServico,
                        IRegistroConformidadesAppServico registroConformidadesAppServico,
                        IRegistroConformidadesServico registroConformidadesServico,
                        INotificacaoAppServico notificacaoAppServico,
                        IClienteAppServico clienteServico,
                        ISiteAppServico siteService,
                        IUsuarioAppServico usuarioAppServico,
                        ILogAppServico logAppServico,
                        IUsuarioClienteSiteAppServico usuarioClienteAppServico,
                        IProcessoServico processoServico,
                        IProcessoAppServico processoAppServico,
                        IFilaEnvioServico filaEnvioServico,
                        IPendenciaAppServico pendenciaAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico,
            IRegistroAcaoImediataServico registroRegistroAcaoImediataServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico, pendenciaAppServico)
        {
            _analiseCriticaTemaAppServico = analiseCriticaTemaAppServico;
            _registroConformidadesAppServico = registroConformidadesAppServico;
            _registroConformidadesServico = registroConformidadesServico;
            _notificacaoAppServico = notificacaoAppServico;
            _siteService = siteService;
            _usuarioAppServico = usuarioAppServico;
            _clienteServico = clienteServico;
            _logAppServico = logAppServico;
            _processoServico = processoServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _usuarioClienteAppServico = usuarioClienteAppServico;
            _filaEnvioServico = filaEnvioServico;
            _registroRegistroAcaoImediataServico = registroRegistroAcaoImediataServico;

        }

        // GET: GestaoDeRisco
        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.IdUsuarioLogado = Util.ObterCodigoUsuarioLogado();
            var numeroUltimoRegistro = 0;
            ViewBag.IdSite = Util.ObterSiteSelecionado();

            var listaGR = _registroConformidadesAppServico.ObtemListaRegistroConformidadePorSite(ViewBag.IdSite, _tipoRegistro, ref numeroUltimoRegistro);

            var model = GerarListaViewModel(listaGR);

            ViewBag.UltimoRegistro = numeroUltimoRegistro;

            return View(model);
        }

        private IList<GestaoRiscoViewModel> GerarListaViewModel(IList<RegistroConformidade> listaGR)
        {
            var retorno = new List<GestaoRiscoViewModel>();

            foreach (var item in listaGR)
            {
                retorno.Add(
                    new GestaoRiscoViewModel
                    {
                        IdRegistro = item.IdRegistroConformidade,
                        NuRegistro = item.NuRegistro,
                        DtEmissao = item.DtEmissao,
                        DtEncerramento = item.DtEnceramento,
                        NomeEmissor = item.Emissor.NmCompleto,
                        Responsavel = IdentificarResponsaveis(item),
                        StatusEtapa = item.StatusEtapa,
                        Tags = item.Tags
                    }
                    );
            }

            return retorno;
        }

        private string IdentificarResponsaveis(RegistroConformidade item)
        {
            string retorno = string.Empty;

            //            -Status Ação = Exibir o nome da responsável por definir as ações
            //- Status Implementação = Exibir os nomes de todos que estão listados nas ações como responsável separados por vírgula(se for apenas uma pessoa, exibir apenas uma vez o nome).
            //- Status Reverificação = Exibir o nome do responsável por reverificar a gestão de risco após concluídas as etapas de implementação. 
            //-Status Encerrada = Exibir o nome da responsável por definir as ações.

            if (item.StatusEtapa == (byte)EtapasRegistroConformidade.AcaoImediata)
            {
                retorno = item.ResponsavelInicarAcaoImediata?.NmCompleto;
            }
            else if (item.StatusEtapa == (byte)EtapasRegistroConformidade.Implementacao)
            {
                foreach (var acao in item.AcoesImediatas)
                {
                    if (!retorno.Contains(acao.ResponsavelImplementar.NmCompleto))
                    {
                        retorno += acao.ResponsavelImplementar.NmCompleto + ", ";
                    }
                }

                retorno = retorno.Substring(0, retorno.Length - 2);
            }
            else if (item.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao)
            {
                var totalAcoesEfetivadas = item.AcoesImediatas.Count(x => x.DtEfetivaImplementacao != null);
                if (totalAcoesEfetivadas == item.AcoesImediatas.Count())
                {
                    retorno = item.ResponsavelReverificador?.NmCompleto;
                }
            }
            else if (item.StatusEtapa == (byte)EtapasRegistroConformidade.Encerrada)
            {
                retorno = item.ResponsavelInicarAcaoImediata?.NmCompleto;
            }

            return retorno;
        }

        private List<string> EnviarNotificacao(RegistroConformidade gestaoDeRisco, List<string> erros)
        {

            try
            {
                var listaAcaoImediataUpdate = gestaoDeRisco.AcoesImediatas.Where(x => x.Estado == EstadoObjetoEF.Modified);
                var acoesImediataSaoAtualizacao = listaAcaoImediataUpdate.FirstOrDefault() != null;
                var notificacao = new Notificacao($"Gestão de Risco#{gestaoDeRisco.NuRegistro}",
                                    null, DateTime.Now, (int)Funcionalidades.GestaoDeRiscos,
                                    gestaoDeRisco.IdProcesso, gestaoDeRisco.IdRegistroConformidade,
                                    gestaoDeRisco.IdSite, 0,
                                    ((char)TipoNotificacao.NotificacaoPorEmail).ToString(),
                                    0);

                if (gestaoDeRisco.OStatusEAcaoImediata())
                {
                    gestaoDeRisco.ResponsavelInicarAcaoImediata = _usuarioAppServico.GetById(gestaoDeRisco.IdResponsavelInicarAcaoImediata.Value);
                    notificacao.IdUsuario = gestaoDeRisco.IdResponsavelInicarAcaoImediata.Value;
                    notificacao.FlEtapa = gestaoDeRisco.StatusEtapa.ToString();
                    _notificacaoAppServico.Add(notificacao);

                    EnviarEmailRiscoDefinirPlanoAcao(gestaoDeRisco);
                }
                else if (gestaoDeRisco.OStatusEImplementacao() && acoesImediataSaoAtualizacao == false)
                {
                    notificacao = new Notificacao($"Gestão de Risco #{gestaoDeRisco.NuRegistro}",
                                    null, DateTime.Now, (int)Funcionalidades.GestaoDeRiscos,
                                    gestaoDeRisco.IdProcesso, gestaoDeRisco.IdRegistroConformidade,
                                    gestaoDeRisco.IdSite, 0,
                                    ((char)TipoNotificacao.NotificacaoPorEmail).ToString(),
                                    0);

                    gestaoDeRisco.AcoesImediatas.ToList().ForEach(acaoImediata =>
                    {
                        acaoImediata.ResponsavelImplementar = _usuarioAppServico.GetById(acaoImediata.IdResponsavelImplementar.Value);
                        notificacao.IdUsuario = acaoImediata.IdResponsavelImplementar.Value;
                        notificacao.FlEtapa = gestaoDeRisco.StatusEtapa.ToString();
                        _notificacaoAppServico.Add(notificacao);
                    });

                }
                else if (gestaoDeRisco.OStatusEReverificacao())
                {
                    gestaoDeRisco.ResponsavelReverificador = _usuarioAppServico.GetById(gestaoDeRisco.IdResponsavelReverificador.Value);
                    notificacao.IdUsuario = gestaoDeRisco.IdResponsavelReverificador.Value;
                    notificacao.FlEtapa = gestaoDeRisco.StatusEtapa.ToString();
                    _notificacaoAppServico.Add(notificacao);
                }

            }
            catch
            {
                erros.Add(Traducao.ResourceNotificacaoMensagem.ErroAoNotificarResponsavelPelaProximaEtapa);

            }

            return erros;


        }

        private string MontarUrlAcessoGestaoRisco(int idRegistro)
        {
            var dominio = "http://" + ConfigurationManager.AppSettings["Dominio"];

            return dominio + "GestaoDeRisco/Editar/" + idRegistro.ToString();
        }

        private void EnviarEmailRiscoDefinirPlanoAcao(RegistroConformidade registro)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);
            var urlAcesso = MontarUrlAcessoGestaoRisco(registro.IdRegistroConformidade);
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\GestaoRiscoDefinirPlanoAcao-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

            try
            {
                string template = System.IO.File.ReadAllText(path);
                string conteudo = template;

                conteudo = conteudo.Replace("#NomeCliente#", cliente.NmFantasia);
                conteudo = conteudo.Replace("#NuRegistro#", registro.NuRegistro.ToString());
                conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);

                Email _email = new Email();

                _email.Assunto = Traducao.ResourceNotificacaoMensagem.MsgNotificacaoGestaoDeRiscos;
                _email.De = ConfigurationManager.AppSettings["EmailDE"];
                _email.Para = registro.ResponsavelInicarAcaoImediata.CdIdentificacao;
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

        [AutorizacaoUsuario((int)FuncoesGestaoDeRisco.CriaNovaGR, (int)Funcionalidades.GestaoDeRiscos)]
        public ActionResult Criar()
        {
            var gestaoDeRisco = new RegistroConformidade();
            gestaoDeRisco.Processo = new Processo();
            //gestaoDeRisco.Processo.IdProcesso = Util.ObterProcessoSelecionado(); 

            gestaoDeRisco.Emissor = new Usuario();
            gestaoDeRisco.ResponsavelInicarAcaoImediata = new Usuario();
            gestaoDeRisco.TipoNaoConformidade = new ControladorCategoria();
            gestaoDeRisco.RegistroGut = new RegistroGut();
            gestaoDeRisco.FlGut = false;

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

            //ViewBag.NomeProcesso = _processoServico.GetProcessoById(Util.ObterProcessoSelecionado()).Nome;
            ViewBag.NomeUsuario = Util.ObterUsuario().Nome;
            ViewBag.NumeroRisco = _registroConformidadesServico.GeraProximoNumeroRegistro(_tipoRegistro, Util.ObterSiteSelecionado());

            return View(gestaoDeRisco);
        }

        [AutorizacaoUsuario((int)FuncoesGestaoDeRisco.CriaNovaGR, (int)Funcionalidades.GestaoDeRiscos)]
        public ActionResult CriarGut()
        {
            var gestaoDeRisco = new RegistroConformidade();
            gestaoDeRisco.Processo = new Processo();
            //gestaoDeRisco.Processo.IdProcesso = Util.ObterProcessoSelecionado(); 

            gestaoDeRisco.Emissor = new Usuario();
            gestaoDeRisco.ResponsavelInicarAcaoImediata = new Usuario();
            gestaoDeRisco.TipoNaoConformidade = new ControladorCategoria();
            gestaoDeRisco.RegistroGut = new RegistroGut();
            gestaoDeRisco.FlGut = true;

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

            //ViewBag.NomeProcesso = _processoServico.GetProcessoById(Util.ObterProcessoSelecionado()).Nome;
            ViewBag.NomeUsuario = Util.ObterUsuario().Nome;
            ViewBag.NumeroRisco = _registroConformidadesServico.GeraProximoNumeroRegistro(_tipoRegistro, Util.ObterSiteSelecionado());

            return View("Criar", gestaoDeRisco);
        }

        public ActionResult PDF(int id)
        {
            var idSite = Util.ObterSiteSelecionado();
            ViewBag.NumeroRisco = null;
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.UsuarioLogado = Util.ObterUsuario();
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.IdCliente = Util.ObterClienteSelecionado();
            ViewBag.NomeUsuario = Util.ObterUsuario().Nome;
            ViewBag.IdGestaoDeRisco = id;

            var gestaoDeRisco = _registroConformidadesAppServico.GetById(id);

            gestaoDeRisco.ArquivosDeEvidenciaAux.AddRange(gestaoDeRisco.ArquivosDeEvidencia.Select(x => x.Anexo));
            _registroConformidadesAppServico.CarregarArquivosNaoConformidadeAnexos2(gestaoDeRisco, true);

            if (gestaoDeRisco.AcoesImediatas.Count > 0)
            {
                if (gestaoDeRisco.AcoesImediatas.Any(x => x.ArquivoEvidencia.Count > 0))
                {
                    var listaAnexo = gestaoDeRisco.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0);

                    listaAnexo.ToList().ForEach(x =>
                    {
                        x.ArquivoEvidenciaAux = x.ArquivoEvidencia.FirstOrDefault().Anexo;
                    });
                }
            }

            if (gestaoDeRisco.IdNuRegistroFilho != null)
            {
                ViewBag.AcaoCorretiva = _registroConformidadesAppServico.GetAll()
                    .FirstOrDefault(x => x.IdSite == gestaoDeRisco.IdSite && x.TipoRegistro == "gr" && x.NuRegistro == gestaoDeRisco.IdNuRegistroFilho);
            }
            ViewBag.IdProcesso = gestaoDeRisco.IdProcesso;
            ViewBag.StatusEtapa = gestaoDeRisco.StatusEtapa;
            //ViewBag.NomeProcesso = _processoServico.GetProcessoById(Util.ObterProcessoSelecionado()).Nome;

            //if ((gestaoDeRisco.StatusRegistro == 0) && (gestaoDeRisco.IdEmissor == Util.ObterCodigoUsuarioLogado()))
            //{
            //    ViewBag.ScriptCall = "sim";
            //}

            var usuarioClienteApp = _usuarioClienteAppServico.Get(s => s.IdSite == idSite);
            var clienteLogoAux = usuarioClienteApp.FirstOrDefault().Cliente.ClienteLogo.FirstOrDefault().Anexo;

            ViewBag.LogoCliente = Convert.ToBase64String(clienteLogoAux.Arquivo);


            var pdf = new ViewAsPdf
            {
                ViewName = "PDF",
                Model = gestaoDeRisco,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "Gestão de Risco " + gestaoDeRisco.IdRegistroConformidade + ".pdf"
            };
            GravarLogImpressao((int)Funcionalidades.GestaoDeRiscos, gestaoDeRisco.IdRegistroConformidade);
            return pdf;

            //return View("Criar", acaoCorretiva);

        }

        public ActionResult Editar(int id)
        {
            ViewBag.NumeroRisco = null;
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.UsuarioLogado = Util.ObterUsuario();
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.IdCliente = Util.ObterClienteSelecionado();
            ViewBag.NomeUsuario = Util.ObterUsuario().Nome;
            ViewBag.IdGestaoDeRisco = id;

            var gestaoDeRisco = _registroConformidadesAppServico.GetById(id);

            gestaoDeRisco.ArquivosDeEvidenciaAux.AddRange(gestaoDeRisco.ArquivosDeEvidencia.Select(x => x.Anexo));
            _registroConformidadesAppServico.CarregarArquivosNaoConformidadeAnexos2(gestaoDeRisco, true);

            if (gestaoDeRisco.AcoesImediatas.Count > 0)
            {
                if (gestaoDeRisco.AcoesImediatas.Any(x => x.ArquivoEvidencia.Count > 0))
                {
                    var listaAnexo = gestaoDeRisco.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0);

                    listaAnexo.ToList().ForEach(x =>
                    {
                        x.ArquivoEvidenciaAux = x.ArquivoEvidencia.FirstOrDefault().Anexo;
                    });
                }
            }

            if (gestaoDeRisco.IdNuRegistroFilho != null)
            {
                ViewBag.AcaoCorretiva = _registroConformidadesAppServico.GetAll()
                    .FirstOrDefault(x => x.IdSite == gestaoDeRisco.IdSite && x.TipoRegistro == "gr" && x.NuRegistro == gestaoDeRisco.IdNuRegistroFilho);
            }
            ViewBag.IdProcesso = gestaoDeRisco.IdProcesso;
            ViewBag.StatusEtapa = gestaoDeRisco.StatusEtapa;
            //ViewBag.NomeProcesso = _processoServico.GetProcessoById(Util.ObterProcessoSelecionado()).Nome;
            //int idPprocesso = _processoServico.GetProcessoById(Util.ObterProcessoSelecionado()).IdProcesso;
            //if ((gestaoDeRisco.StatusRegistro == 0) && (gestaoDeRisco.IdEmissor == Util.ObterCodigoUsuarioLogado()))
            //{
            //    ViewBag.ScriptCall = "sim";
            //}
            //gestaoDeRisco.IdProcesso = idPprocesso;

            return View("Criar", gestaoDeRisco);
        }

        [HttpPost]
        [AutorizacaoUsuario((int)FuncoesGestaoDeRisco.CriaNovaGR, (int)Funcionalidades.GestaoDeRiscos)]
        public JsonResult SalvarPrimeiraEtapa(RegistroConformidade gestaoDeRisco)
        {

            if (gestaoDeRisco.RegistroGut.Gravidade == 0 || gestaoDeRisco.RegistroGut.Tendencia == 0 || gestaoDeRisco.RegistroGut.Urgencia == 0)
            {
                gestaoDeRisco.RegistroGut = null;
                gestaoDeRisco.FlGut = false;
            }

            //gestaoDeRisco.IdProcesso = Util.ObterProcessoSelecionado();
            gestaoDeRisco.IdEmissor = Util.ObterCodigoUsuarioLogado();
            gestaoDeRisco.StatusRegistro = 1;

            var erros = new List<string>();

            try
            {
                TrataDadosParaCriacao(gestaoDeRisco);

                _registroConformidadesServico.ValidarCampos(gestaoDeRisco, ref erros);

                erros = ValidaCampoPrimarios(gestaoDeRisco, erros);

                if (erros.Count != 0)
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    SalvarArquivoEvidencia(gestaoDeRisco);

                    if (gestaoDeRisco.EProcedente.Value == false)
                    {
                        gestaoDeRisco.StatusEtapa = (int)EtapasRegistroConformidade.Encerrada;
                        gestaoDeRisco.DtEnceramento = DateTime.Now;
                        gestaoDeRisco.IdResponsavelEtapa = gestaoDeRisco.IdEmissor;
                    }
                    else
                    {
                        gestaoDeRisco.StatusEtapa = (int)EtapasRegistroConformidade.AcaoImediata;
                    }


                    gestaoDeRisco = _registroConformidadesAppServico.SalvarPrimeiraEtapa(gestaoDeRisco);
                    GravarLogInclusao((int)Funcionalidades.GestaoDeRiscos, gestaoDeRisco.IdRegistroConformidade);

                    if (gestaoDeRisco.IdResponsavelEtapa != null)
                        erros = EnviarNotificacao(gestaoDeRisco, erros);

                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_criar_valid }, JsonRequestBehavior.AllowGet);

        }

        private List<string> ValidaCampoPrimarios(RegistroConformidade gestaoDeRisco, List<string> erros)
        {
            if (gestaoDeRisco.EProcedente.Value)
            {
                if (gestaoDeRisco.IdResponsavelInicarAcaoImediata == null || gestaoDeRisco.IdResponsavelInicarAcaoImediata == 0)
                {
                    erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_required_Responsavel_definir_AC);
                }

                if (string.IsNullOrEmpty(gestaoDeRisco.Causa))
                {
                    erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_required_Analise_causa);
                }
            }
            else if (!gestaoDeRisco.EProcedente.Value && string.IsNullOrEmpty(gestaoDeRisco.DsJustificativa))
            {
                erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_required_Justificativa);
            }

            return erros;

        }

        private List<string> ValidaCampoSegundaEtapa(RegistroConformidade gestaoDeRisco, List<string> erros)
        {
            if (gestaoDeRisco.EProcedente.Value)
            {
                if (gestaoDeRisco.StatusEtapa == 2)
                {
                    if (gestaoDeRisco.DtDescricaoAcao == null)
                    {
                        erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_required_DtDescricao);
                    }

                    if (gestaoDeRisco.AcoesImediatas.Count() == 0)
                    {
                        erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.EObrigatorioPeloMenosUmaAcao);
                    }

                    gestaoDeRisco.AcoesImediatas.ToList().ForEach(x =>
                    {
                        if (string.IsNullOrEmpty(x.Descricao))
                        {
                            erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_required_AI_Descricao);
                        }

                        if (x.DtPrazoImplementacao == null)
                        {
                            erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_required_AI_Prazo_implementacao);
                        }

                        if (x.IdResponsavelImplementar == null || x.IdResponsavelImplementar == 0)
                        {
                            erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_required_AI_Responsavel_implementar);
                        }


                    });

                    if (gestaoDeRisco.IdResponsavelReverificador == null || gestaoDeRisco.IdResponsavelReverificador == 0)
                    {
                        erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_required_Responsavel_reverificacao);
                    }

                }

                if (gestaoDeRisco.StatusEtapa == 3)
                {

                    gestaoDeRisco.AcoesImediatas.ToList().ForEach(x =>
                    {
                        if (string.IsNullOrEmpty(x.Descricao))
                        {
                            erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_required_AI_Descricao);
                        }

                        if (x.DtEfetivaImplementacao == null)
                        {
                            erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_required_AI_Efetiva_implementacao);
                        }

                        if (x.ArquivoEvidenciaAux.ArquivoB64 == null)
                        {
                            erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_required_AI_Arquivo_evidencia);
                        }

                    });


                }

                if (gestaoDeRisco.StatusEtapa == 4)
                {
                    if (gestaoDeRisco.FlEficaz == null)
                    {
                        erros.Add(Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_required_Foi_eficaz);
                    }
                }

            }

            return erros;
        }

        private void SalvarArquivoEvidencia(RegistroConformidade gr)
        {
            if (gr.ArquivosDeEvidenciaAux.Count > 0)
            {
                gr.ArquivosDeEvidenciaAux.ForEach(arquivoEvidencia =>
                {

                    gr.ArquivosDeEvidencia.Add(new ArquivosDeEvidencia
                    {
                        Anexo = arquivoEvidencia,
                        RegistroConformidade = gr,
                        TipoRegistro = gr.TipoRegistro
                    });
                });
            }
        }
        private void TrataDadosParaCriacao(RegistroConformidade gr)
        {
            gr.TipoRegistro = _tipoRegistro;
            gr.StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata;
            gr.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
            gr.FlDesbloqueado = gr.FlDesbloqueado > 0 ? (byte)0 : (byte)0;

            if (gr.ArquivosDeEvidenciaAux.Count > 0)
            {
                gr.ArquivosDeEvidenciaAux.ForEach(arquivoEvidencia => arquivoEvidencia.Tratar());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DestravarEtapa(int idGestaoDeRisco, int etapa)
        {
            var erros = new List<string>();

            var gestaoDeRisco = _registroConformidadesAppServico.GetById(idGestaoDeRisco);

            var idPerfil = Util.ObterPerfilUsuarioLogado();

            _registroConformidadesServico.ValidaDestravamento(idPerfil, ref erros);

            if (erros.Count == 0)
            {
                gestaoDeRisco.FlDesbloqueado = (byte)etapa;
                var result = _registroConformidadesAppServico.DestravarEtapa(gestaoDeRisco);
                return Json(new { StatusCode = (int)HttpStatusCode.OK });
            }
            else
            {
                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros });
            }
        }

        [HttpPost]
        public JsonResult SalvarSegundaEtapa(RegistroConformidade gestaoDeRisco)
        {
            var erros = new List<string>();


            var acoesImediatasNova = gestaoDeRisco.AcoesImediatas.Where(x => x.IdAcaoImediata == 0).ToList();

            if (acoesImediatasNova.Count > 0)
            {
                var registroAcoes = _registroConformidadesAppServico.GetById(gestaoDeRisco.IdRegistroConformidade);

                if (registroAcoes.AcoesImediatas.Count > 0)
                    gestaoDeRisco.StatusEtapa = 1;
            }

            try
            {
                var responsavelAcaoCorrecao = _registroConformidadesAppServico.Get(x => x.IdRegistroPai == gestaoDeRisco.IdRegistroConformidade && x.TipoRegistro == "ac").OrderByDescending(x => x.IdRegistroConformidade).FirstOrDefault();
                var idResponsavelAcaoCorrecao = (responsavelAcaoCorrecao != null ? responsavelAcaoCorrecao.IdResponsavelInicarAcaoImediata : 0);

                gestaoDeRisco.IdUsuarioAlterou = Util.ObterCodigoUsuarioLogado();
                gestaoDeRisco.FlDesbloqueado = gestaoDeRisco.FlDesbloqueado > 0 ? (byte)0 : (byte)0;
                gestaoDeRisco.TipoRegistro = _tipoRegistro;

                foreach (var acao in gestaoDeRisco.AcoesImediatas)
                {

                    if (gestaoDeRisco.StatusEtapa == 2)
                    {
                        if (acao.DtEfetivaImplementacao != null)
                        {
                            gestaoDeRisco.DtEfetivaImplementacao = acao.DtEfetivaImplementacao;
                        }
                    }
                }


                _registroConformidadesServico.ValidaGestaoDeRisco(gestaoDeRisco, Util.ObterCodigoUsuarioLogado(), ref erros);


                erros = ValidaCampoSegundaEtapa(gestaoDeRisco, erros);

                var usuario = Util.ObterUsuario();


                for (int i = 0; i < gestaoDeRisco.AcoesImediatas.Count; i++)
                {

                    if (gestaoDeRisco.StatusEtapa == 3 && gestaoDeRisco.AcoesImediatas[i].Aprovado == false && (string.IsNullOrEmpty(gestaoDeRisco.AcoesImediatas[i].Motivo) || string.IsNullOrEmpty(gestaoDeRisco.AcoesImediatas[i].Orientacao)))
                    {
                        erros.Add("Favor preencher Motivo e Orientação.");
                    }
                    if (gestaoDeRisco.AcoesImediatas[i].Motivo != null || gestaoDeRisco.AcoesImediatas[i].Orientacao != null)
                    {
                        ComentarioAcaoImediata ca = new ComentarioAcaoImediata();
                        ca.Motivo = gestaoDeRisco.AcoesImediatas[i].Motivo;
                        ca.Orientacao = gestaoDeRisco.AcoesImediatas[i].Orientacao;
                        ca.DataComentario = DateTime.Now.ToString();
                        ca.UsuarioComentario = usuario.Nome;

                        gestaoDeRisco.AcoesImediatas[i].ComentariosAcaoImediata.Add(ca);
                    }

                }

                if (erros.Count == 0)
                {

                    var acoesEfetivadas = gestaoDeRisco.AcoesImediatas.Where(x => x.DtEfetivaImplementacao != null).ToList();

                    acoesEfetivadas.ToList().ForEach(acao =>
                    {
                        acao.IdRegistroConformidade = gestaoDeRisco.IdRegistroConformidade;
                    });

                    AtualizarDatasAgendadas(gestaoDeRisco);

                    gestaoDeRisco.StatusRegistro = 1;
                    gestaoDeRisco = _registroConformidadesAppServico.SalvarSegundaEtapa(gestaoDeRisco, Funcionalidades.GestaoDeRiscos);
                    erros = EnviarNotificacao(gestaoDeRisco, erros);

                    RemoverFilaEnvioAcoesEfetivadas(acoesEfetivadas);

                    if (acoesImediatasNova.Count > 0)
                    {
                        EnviarEmailsImplementacao(acoesImediatasNova, gestaoDeRisco);
                        EnfileirarEmailsAcaoImediata(acoesImediatasNova, gestaoDeRisco);
                    }

                    if (acoesEfetivadas.Count == gestaoDeRisco.AcoesImediatas.Count)
                    {
                        EnviarEmailResponsavelReverificacao(gestaoDeRisco);
                    }

                    var acoesIneficazes = gestaoDeRisco.AcoesImediatas.Where(x => x.Aprovado == false).ToList();
                    if (acoesIneficazes.Count > 0)
                    {
                        EnviarEmailAcaoIneficaz(gestaoDeRisco, acoesIneficazes);
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

                if (ex != null)
                {
                    erros.Add(ex.Message);
                }
                else
                {
                    erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                }
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);

            }
            GravarLogAlteracao((int)Funcionalidades.GestaoDeRiscos, gestaoDeRisco.IdRegistroConformidade);
            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.GestaoDeRisco.ResourceGestaoDeRisco.GR_msg_save_valid }, JsonRequestBehavior.AllowGet);
        }

        private void RemoverFilaEnvioAcoesEfetivadas(List<RegistroAcaoImediata> acoesEfetivadas)
        {
            foreach (var acao in acoesEfetivadas)
            {

                if (acao.IdFilaEnvio != null)
                {
                    var filaEnvio = _filaEnvioServico.ObterPorId(acao.IdFilaEnvio.Value);
                    if (!filaEnvio.Enviado)
                    {
                        acao.IdFilaEnvio = null;
                        _registroRegistroAcaoImediataServico.Update(acao);
                        _filaEnvioServico.Apagar(filaEnvio);
                    }
                }
            }
        }


        private void AtualizarDatasAgendadas(RegistroConformidade gestaoRisco)
        {
            var acoes = gestaoRisco.AcoesImediatas.Where(x => x.DtEfetivaImplementacao == null && x.IdAcaoImediata > 0 && x.IdFilaEnvio != null).ToList();
            var acoesEnfileirar = new List<RegistroAcaoImediata>();

            foreach (var acao in acoes)
            {
                var filaEnvio = _filaEnvioServico.ObterPorId(acao.IdFilaEnvio.Value);

                if (filaEnvio != null)
                {
                    if (!filaEnvio.Enviado)
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

            EnfileirarEmailsAcaoImediata(acoesEnfileirar, gestaoRisco);

        }

        private void EnviarEmailResponsavelReverificacao(RegistroConformidade gestaoRisco)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);
            var urlAcesso = MontarUrlAcessoGestaoRisco(gestaoRisco.IdRegistroConformidade);
            var usuario = _usuarioAppServico.GetById(gestaoRisco.IdResponsavelReverificador.Value);

            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\GestaoRiscoReverificador-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
            string template = System.IO.File.ReadAllText(path);
            string conteudo = template;

            conteudo = conteudo.Replace("#NomeCliente#", cliente.NmFantasia);
            conteudo = conteudo.Replace("#NuRegistro#", gestaoRisco.NuRegistro.ToString());
            conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);

            Email _email = new Email();

            _email.Assunto = Traducao.ResourceNotificacaoMensagem.MsgNotificacaoGestaoDeRiscos;
            _email.De = ConfigurationManager.AppSettings["EmailDE"];
            _email.Para = usuario.CdIdentificacao;
            _email.Conteudo = conteudo;
            _email.Servidor = ConfigurationManager.AppSettings["SMTPServer"];
            _email.Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            _email.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
            _email.Enviar();
        }

        private void EnviarEmailAcaoIneficaz(RegistroConformidade gestaoRisco, List<RegistroAcaoImediata> acoesIneficazes)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);
            var urlAcesso = MontarUrlAcessoGestaoRisco(gestaoRisco.IdRegistroConformidade);
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\GestaoRiscoAcaoIneficaz-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

            foreach (var acao in acoesIneficazes)
            {
                try
                {
                    string template = System.IO.File.ReadAllText(path);
                    string conteudo = template;

                    conteudo = conteudo.Replace("#NomeCliente#", cliente.NmFantasia);
                    conteudo = conteudo.Replace("#NuRegistro#", gestaoRisco.NuRegistro.ToString());
                    conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);

                    Email _email = new Email();

                    _email.Assunto = Traducao.ResourceNotificacaoMensagem.MsgNotificacaoGestaoDeRiscos;
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

        private void EnfileirarEmailsAcaoImediata(List<RegistroAcaoImediata> acoesNova, RegistroConformidade gestaoDeRisco)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);
            var urlAcesso = MontarUrlAcessoGestaoRisco(gestaoDeRisco.IdRegistroConformidade);
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\GestaoRiscoAcaoDataImplementacao-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

            foreach (var acao in acoesNova)
            {
                try
                {
                    var destinatario = _usuarioAppServico.GetById(acao.IdResponsavelImplementar.Value).CdIdentificacao;
                    string template = System.IO.File.ReadAllText(path);
                    string conteudo = template;

                    conteudo = conteudo.Replace("#NomeCliente#", cliente.NmFantasia);
                    conteudo = conteudo.Replace("#NuRegistro#", gestaoDeRisco.NuRegistro.ToString());
                    conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);

                    var filaEnvio = new FilaEnvio();
                    filaEnvio.Assunto = Traducao.ResourceNotificacaoMensagem.MsgNotificacaoGestaoDeRiscos;
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


        private void EnviarEmailsImplementacao(List<RegistroAcaoImediata> acoesNova, RegistroConformidade gestaoDeRisco)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);
            var urlAcesso = MontarUrlAcessoGestaoRisco(gestaoDeRisco.IdRegistroConformidade);
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\GestaoRiscoImplementacao-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";

            foreach (var acao in acoesNova)
            {
                try
                {
                    var destinatario = _usuarioAppServico.GetById(acao.IdResponsavelImplementar.Value).CdIdentificacao;
                    string template = System.IO.File.ReadAllText(path);
                    string conteudo = template;

                    conteudo = conteudo.Replace("#NomeCliente#", cliente.NmFantasia);
                    conteudo = conteudo.Replace("#NuRegistro#", gestaoDeRisco.NuRegistro.ToString());
                    conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);

                    Email _email = new Email();

                    _email.Assunto = Traducao.ResourceNotificacaoMensagem.MsgNotificacaoGestaoDeRiscos;
                    _email.De = ConfigurationManager.AppSettings["EmailDE"];
                    _email.Para = destinatario;
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
        ///[ValidateAntiForgeryToken]
        public JsonResult Excluir(int idGestaoDeRisco)
        {
            var conformidade = _registroConformidadesAppServico.GetById(idGestaoDeRisco);

            _analiseCriticaTemaAppServico.Get(x => x.IdGestaoDeRisco == idGestaoDeRisco).ToList().ForEach(x =>
            {
                _analiseCriticaTemaAppServico.Remove(x);
            });

            _registroConformidadesAppServico.Remove(conformidade);
            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Resource.RegistroExlcuido }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtemUltimaDataEmissao(int site)
        {
            var ultimaDataEmissao = _registroConformidadesAppServico.ObtemUltimaDataEmissao(site, _tipoRegistro).ToString("dd/MM/yyyy");

            return Json(new { StatusCode = (int)HttpStatusCode.OK, UltimaDataEmissao = ultimaDataEmissao }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RemoverComAcaoConformidade(int idGestaoDeRisco)
        {
            var erros = new List<string>();
            var gestaoDeRiscoPai = _registroConformidadesAppServico.GetById(idGestaoDeRisco);
            var gestaoDeRiscoFilho = _registroConformidadesAppServico.ObtemAcaoConformidadePorNaoConformidade(gestaoDeRiscoPai);


            if (UtilsServico.EstaPreenchido(gestaoDeRiscoFilho))
            {
                _registroConformidadesServico.ValidoParaExclusaoAcaoCorretiva(gestaoDeRiscoFilho, ref erros);
            }
            _registroConformidadesServico.ValidoParaExclusaoNaoConformidade(gestaoDeRiscoPai, ref erros);

            if (erros.Count == 0)
            {
                _notificacaoAppServico.RemovePorFuncionalidade(Funcionalidades.GestaoDeRiscos, gestaoDeRiscoPai.IdRegistroConformidade);
                _registroConformidadesAppServico.Remove(gestaoDeRiscoPai);

                if (UtilsServico.EstaPreenchido(gestaoDeRiscoFilho))
                {
                    _registroConformidadesAppServico.Remove(gestaoDeRiscoFilho);
                }
            }
            else

            {
                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ObterPartialGestaoDeRisco(int? idRegistro)
        {
            var erros = new List<string>();
            var analiseCriticaTema = new AnaliseCriticaTema();

            try
            {

                return PartialView("GestaoDeRiscoAnaliseCritica", analiseCriticaTema);

            }
            catch (Exception)
            {

                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObterPartialGestaoDeRiscoAnaliseCritica(int? idRegistro)
        {
            var erros = new List<string>();
            var analiseCriticaTema = new AnaliseCriticaTema();

            try
            {

                return PartialView("GestaoDeRisco", analiseCriticaTema);

            }
            catch (Exception)
            {

                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Imprimir(int id)
        {
            var analiseCritica = _registroConformidadesAppServico.GetById(id);
            _registroConformidadesAppServico.CarregarArquivosNaoConformidadeAnexos2(analiseCritica, true);

            var pdf = new ViewAsPdf
            {
                ViewName = "Criar",
                Model = analiseCritica,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "GestaoDeRisco.pdf"
            };

            return pdf;
        }

        [HttpPost]
        public JsonResult DestravarDocumento(string idGestaoDeRisco)
        {
            var erros = new List<string>();

            try
            {
                var gestaoDeRisco = _registroConformidadesAppServico.GetById(int.Parse(idGestaoDeRisco));
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

    }
}