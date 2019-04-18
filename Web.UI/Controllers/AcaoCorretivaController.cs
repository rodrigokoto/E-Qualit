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

namespace Web.UI.Controllers
{
    //[ProcessoSelecionado]
    [VerificaIntegridadeLogin]
    public class AcaoCorretivaController : BaseController
    {
        private readonly IRegistroConformidadesAppServico _registroConformidadesAppServico;
        private readonly IRegistroConformidadesServico _registroConformidadesServico;
        private readonly IClienteAppServico _clienteServico;
        private readonly INotificacaoAppServico _notificacaoAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private string _tipoRegistro = "ac";
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public AcaoCorretivaController(
            IRegistroConformidadesAppServico registroConformidadesAppServico,
            IRegistroConformidadesServico registroConformidadesServico,
            INotificacaoAppServico notificacaoAppServico,
            ILogAppServico logAppServico,
            IProcessoAppServico processoAppServico,
            IUsuarioAppServico usuarioAppServico,
            IClienteAppServico clienteServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _registroConformidadesAppServico = registroConformidadesAppServico;
            _registroConformidadesServico = registroConformidadesServico;
            _notificacaoAppServico = notificacaoAppServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _clienteServico = clienteServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        // GET: AcaoCorretiva
        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.IdUsuarioLogado = Util.ObterCodigoUsuarioLogado();
            var numeroUltimoRegistro = 0;
            
            var listaAC = _registroConformidadesAppServico.ObtemListaRegistroConformidadePorSite(ViewBag.IdSite, _tipoRegistro, ref numeroUltimoRegistro);
            ViewBag.UltimoRegistro = numeroUltimoRegistro;

            return View(listaAC);
        }



        private List<string> EnviarNotificacao(RegistroConformidade naoConformidade, List<string> erros)
        {
            try
            {
                var listaAcaoImediataUpdate = naoConformidade.AcoesImediatas.Where(x => x.Estado == EstadoObjetoEF.Modified);
                var acoesImediataSaoAtualizacao = listaAcaoImediataUpdate.FirstOrDefault() != null;
                var notificacao = new Notificacao($"Ação Corretiva #{naoConformidade.NuRegistro}",
                                    null, DateTime.Now, (int)Funcionalidades.AcaoCorretiva,
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
                    notificacao = new Notificacao($"Ação Corretiva #{naoConformidade.NuRegistro}",
                                    null, DateTime.Now, (int)Funcionalidades.AcaoCorretiva,
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
                    });

                }
                else if (naoConformidade.OStatusEReverificacao())
                {
                    naoConformidade.ResponsavelReverificador = _usuarioAppServico.GetById(naoConformidade.IdResponsavelReverificador.Value);
                    notificacao.IdUsuario = naoConformidade.IdResponsavelReverificador.Value;
                    notificacao.FlEtapa = naoConformidade.StatusEtapa.ToString();
                    _notificacaoAppServico.Add(notificacao);
                }

                EnviaEmail(naoConformidade);

            }
            catch
            {
                erros.Add(Traducao.ResourceNotificacaoMensagem.ErroAoNotificarResponsavelPelaProximaEtapa);

            }

            return erros;

        }

        private void EnviaEmail(RegistroConformidade nc)
        {
            var idCliente = Util.ObterClienteSelecionado();
            Cliente cliente = _clienteServico.GetById(idCliente);

            var usuarioNotificacao = _usuarioAppServico.GetById(nc.IdResponsavelEtapa.Value);

            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\AteracaoStatusAcaoCorretiva-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
            string template = System.IO.File.ReadAllText(path);
            string conteudo = template;

            conteudo = conteudo.Replace("#NomeCliente#", cliente.NmFantasia);
            conteudo = conteudo.Replace("#NuAcaoCorretiva#", nc.NuRegistro.ToString());

            Email _email = new Email();

            _email.Assunto = Traducao.ResourceNotificacaoMensagem.msgNotificacaoAcaoCorretiva;
            _email.De = ConfigurationManager.AppSettings["EmailDE"];
            _email.Para = usuarioNotificacao.CdIdentificacao;
            _email.Conteudo = conteudo;
            _email.Servidor = ConfigurationManager.AppSettings["SMTPServer"];
            _email.Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            _email.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
            _email.Enviar();
        }


        public ActionResult PDF(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.UsuarioLogado = Util.ObterUsuario();
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.IdUsuarioLogado = Util.ObterCodigoUsuarioLogado();
            ViewBag.IdCliente = Util.ObterClienteSelecionado();

            var acaoCorretiva = _registroConformidadesAppServico.GetById(id);

            acaoCorretiva.ArquivosDeEvidenciaAux.AddRange(acaoCorretiva.ArquivosDeEvidencia.Select(x => x.Anexo));

            if (acaoCorretiva.AcoesImediatas.Count > 0)
            {
                if (acaoCorretiva.AcoesImediatas.Any(x => x.ArquivoEvidencia.Count > 0))
                {
                    var listaAnexo = acaoCorretiva.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0);

                    listaAnexo.ToList().ForEach(x =>
                    {
                        x.ArquivoEvidenciaAux = x.ArquivoEvidencia.FirstOrDefault().Anexo;
                    });
                }
            }

            if (acaoCorretiva.IdNuRegistroFilho != null)
            {
                ViewBag.AcaoCorretiva = _registroConformidadesAppServico.GetAll()
                    .FirstOrDefault(x => x.IdSite == acaoCorretiva.IdSite && x.TipoRegistro == "ac" && x.NuRegistro == acaoCorretiva.IdNuRegistroFilho);
            }
            ViewBag.IdProcesso = acaoCorretiva.IdProcesso;
            ViewBag.StatusEtapa = acaoCorretiva.StatusEtapa;


            //if ((acaoCorretiva.StatusRegistro == 0) && (acaoCorretiva.IdEmissor == Util.ObterCodigoUsuarioLogado()))
            //{
            //    ViewBag.ScriptCall = "sim";
            //}

            var pdf = new ViewAsPdf
            {
                ViewName = "PDF",
                Model = acaoCorretiva,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "Ação Corretiva " + acaoCorretiva.IdRegistroConformidade + ".pdf"
            };

            return pdf;

            //return View("Criar", acaoCorretiva);

        }


        public ActionResult Editar(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.UsuarioLogado = Util.ObterUsuario();
            ViewBag.IdPerfil = Util.ObterPerfilUsuarioLogado();
            ViewBag.IdUsuarioLogado = Util.ObterCodigoUsuarioLogado();
            ViewBag.IdCliente = Util.ObterClienteSelecionado();

            var acaoCorretiva = _registroConformidadesAppServico.GetById(id);

            acaoCorretiva.ArquivosDeEvidenciaAux.AddRange(acaoCorretiva.ArquivosDeEvidencia.Select(x => x.Anexo));

            if (acaoCorretiva.AcoesImediatas.Count > 0)
            {
                if (acaoCorretiva.AcoesImediatas.Any(x => x.ArquivoEvidencia.Count > 0))
                {
                    var listaAnexo = acaoCorretiva.AcoesImediatas.Where(x => x.ArquivoEvidencia.Count > 0);

                    listaAnexo.ToList().ForEach(x =>
                    {
                        x.ArquivoEvidenciaAux = x.ArquivoEvidencia.FirstOrDefault().Anexo;
                    });
                }
            }

            if (acaoCorretiva.IdNuRegistroFilho != null)
            {
                ViewBag.AcaoCorretiva = _registroConformidadesAppServico.GetAll()
                    .FirstOrDefault(x => x.IdSite == acaoCorretiva.IdSite && x.TipoRegistro == "ac" && x.NuRegistro == acaoCorretiva.IdNuRegistroFilho);
            }
            ViewBag.IdProcesso = acaoCorretiva.IdProcesso;
            ViewBag.StatusEtapa = acaoCorretiva.StatusEtapa;


            //if ((acaoCorretiva.StatusRegistro == 0) && (acaoCorretiva.IdEmissor == Util.ObterCodigoUsuarioLogado()))
            //{
            //    ViewBag.ScriptCall = "sim";
            //}

            return View("Criar", acaoCorretiva);

        }

        [HttpPost]
        public JsonResult DestravarEtapa(int idAcaoCorretiva, int etapa)
        {
            var erros = new List<string>();

            var acaoCorretiva = _registroConformidadesAppServico.GetById(idAcaoCorretiva);
            var idPerfil = Util.ObterPerfilUsuarioLogado();

            _registroConformidadesServico.ValidaDestravamento(idPerfil, ref erros);

            if (erros.Count == 0)
            {
                acaoCorretiva.FlDesbloqueado = (byte)etapa;
                var result = _registroConformidadesAppServico.DestravarEtapa(acaoCorretiva);
                return Json(new { StatusCode = (int)HttpStatusCode.OK });
            }
            else
            {
                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros });
            }
        }

        [HttpPost]
        public JsonResult SalvarSegundaEtapa(RegistroConformidade acaoCorretiva)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var erros = new List<string>();

            try
            {

                acaoCorretiva.IdUsuarioAlterou = Util.ObterCodigoUsuarioLogado();
                acaoCorretiva.FlDesbloqueado = acaoCorretiva.FlDesbloqueado > 0 ? (byte)0 : (byte)0;
                acaoCorretiva.TipoRegistro = _tipoRegistro;
                acaoCorretiva.StatusRegistro = 1;
                acaoCorretiva.IdEmissor = Util.ObterCodigoUsuarioLogado();



                _registroConformidadesServico.ValidaAcaoCorretiva(acaoCorretiva, Util.ObterCodigoUsuarioLogado(), ref erros);

                if (erros.Count == 0)
                {
                    acaoCorretiva = _registroConformidadesAppServico.SalvarSegundaEtapa(acaoCorretiva, Funcionalidades.AcaoCorretiva);
                    erros = EnviarNotificacao(acaoCorretiva, erros);
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

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.AcaoCorretiva.ResourceAcaoCorretiva.AC_msg_save_valid }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoverAcaoConformidade(int idAcaoCorretiva)
        {
            var erros = new List<string>();
            var acaoCorretiva = _registroConformidadesAppServico.GetById(idAcaoCorretiva);

            _registroConformidadesServico.ValidoParaExclusaoAcaoCorretiva(acaoCorretiva, ref erros);

            if (erros.Count == 0)
            {
                _registroConformidadesAppServico.Remove(acaoCorretiva);
                return Json(new { StatusCode = (int)HttpStatusCode.OK }, JsonRequestBehavior.AllowGet);

            }
            else

            {
                return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erros = erros }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public JsonResult ObtemUltimaDataEmissao(int site)
        {
            var ultimaDataEmissao = _registroConformidadesAppServico.ObtemUltimaDataEmissao(site, _tipoRegistro).ToString(Traducao.Resource.dateFormat);

            return Json(new { StatusCode = (int)HttpStatusCode.OK, UltimaDataEmissao = ultimaDataEmissao }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoverComAcaoConformidade(int idAcaoCorretiva)
        {
            
            var erros = new List<string>();

            try
            {

                var naoConformidade = _registroConformidadesAppServico.GetById(idAcaoCorretiva);
                var acaoCorretiva = _registroConformidadesAppServico.ObtemAcaoConformidadePorNaoConformidade(naoConformidade);

                if (UtilsServico.EstaPreenchido(acaoCorretiva))
                {
                    _registroConformidadesServico.ValidoParaExclusaoAcaoCorretiva(acaoCorretiva, ref erros);
                }
                _registroConformidadesServico.ValidoParaExclusaoNaoConformidade(naoConformidade, ref erros);

                if (erros.Count == 0)
                {

                    _notificacaoAppServico.RemovePorFuncionalidade(Funcionalidades.AcaoCorretiva, naoConformidade.IdRegistroConformidade);
                    _registroConformidadesAppServico.Remove(naoConformidade);
                    
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

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.AcaoCorretiva.ResourceAcaoCorretiva .AC_msg_delete_valid }, JsonRequestBehavior.AllowGet);


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
                FileName = "AcaoCorretiva.pdf"
            };

            return pdf;
        }


        [HttpPost]
        public JsonResult DestravarDocumento(string idAcaoCorretiva)
        {
            var erros = new List<string>();

            try
            {
                var gestaoDeRisco = _registroConformidadesAppServico.GetById(int.Parse(idAcaoCorretiva));
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