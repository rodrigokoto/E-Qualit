using ApplicationService.Enum;
using ApplicationService.Interface;
using DAL.Context;
using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Servico;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using Web.UI.Helpers;

namespace Web.UI.Controllers
{
    //[ProcessoSelecionado]
    [VerificaIntegridadeLogin]
    [SitePossuiModulo(7)]
    [ValidaUsuario]
    public class AnaliseCriticaController : BaseController
    {
        private readonly IAnaliseCriticaAppServico _analiseCriticaAppServico;
        private readonly IAnaliseCriticaServico _analiseCriticaServico;
        private readonly IRegistroConformidadesServico _registroConformidadesServico;

        private readonly IRegistroConformidadesAppServico _registroConformidadeAppServico;
        public readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IAnaliseCriticaTemaAppServico _analiseCriticaTemaAppServico;
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteSiteAppServico;
        private readonly IAnaliseCriticaFuncionarioAppServico _analiseCriticaFuncionarioAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IFilaEnvioServico _filaEnvioServico;

        public AnaliseCriticaController(IAnaliseCriticaAppServico analiseCriticaAppServico,
                                        IAnaliseCriticaServico analiseCriticaServico,
                                        IRegistroConformidadesAppServico registroConformidadeAppServico,
                                        IUsuarioAppServico usuarioAppServico,
                                        IAnaliseCriticaTemaAppServico analiseCriticaTemaAppServico,
                                        IAnaliseCriticaFuncionarioAppServico analiseCriticaFuncionarioAppServico,
                                        IUsuarioClienteSiteAppServico usuarioClienteSiteAppServico,
                                        ILogAppServico logAppServico,
                                        IRegistroConformidadesServico registroConformidadesServico,
                                        IProcessoAppServico processoAppServico,
                                        IControladorCategoriasAppServico controladorCategoriasServico,
                                        IFilaEnvioServico filaEnvioServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _analiseCriticaAppServico = analiseCriticaAppServico;
            _analiseCriticaServico = analiseCriticaServico;
            _registroConformidadeAppServico = registroConformidadeAppServico;
            _usuarioAppServico = usuarioAppServico;
            _analiseCriticaTemaAppServico = analiseCriticaTemaAppServico;
            _analiseCriticaFuncionarioAppServico = analiseCriticaFuncionarioAppServico;
            _usuarioClienteSiteAppServico = usuarioClienteSiteAppServico;
            _logAppServico = logAppServico;
            _registroConformidadesServico = registroConformidadesServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _filaEnvioServico = filaEnvioServico;
        }

        [AutorizacaoUsuario((int)FuncoesAnaliseCritica.RegistroDaAta, (int)Funcionalidades.AnaliseCritica)]
        public ActionResult Criar()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.Tema = "tema";
            //ViewBag.IdProcesso = Util.ObterProcessoSelecionado();
            //ViewBag.ProcessoSelecionado = Util.ObterProcessoSelecionado();
            ViewBag.NumeroRisco = _registroConformidadesServico.GeraProximoNumeroRegistro("gr", Util.ObterSiteSelecionado());
            TempData["NumeroRisco"] = null;
            TempData["NumeroRisco"] = ViewBag.NumeroRisco;

            var analiseCritica = new AnaliseCritica();
            return View(analiseCritica);
        }

        public ActionResult Visualizar(int? id)
        {
            var analiseCritica = new AnaliseCritica();

            if (id != null)
            {
                analiseCritica = _analiseCriticaAppServico.GetById(Convert.ToInt32(id));
            }

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.Tema = "tema";
            //ViewBag.IdProcesso = Util.ObterProcessoSelecionado();

            analiseCritica.Temas.ForEach(tema =>
            {
                if (tema.PossuiGestaoRisco)
                {
                    tema.AnaliseCritica = new AnaliseCritica();
                    tema.GestaoDeRisco.Emissor = new Usuario();
                    tema.GestaoDeRisco.Processo = new Processo();
                    tema.GestaoDeRisco.Site = new Site();

                }
            }
            );


            return View("Visualizar", analiseCritica);
        }

        public ActionResult Editar(int? id)
        {
            var analiseCritica = new AnaliseCritica();

            if (id != null)
            {
                analiseCritica = _analiseCriticaAppServico.GetById(Convert.ToInt32(id));
            }
            var idSite = Util.ObterSiteSelecionado();
            ViewBag.IdSite = idSite;
            ViewBag.Tema = "tema";

            List<Processo> lista = _processoAppServico.ListaProcessosPorSite(analiseCritica.IdSite);

            var usuarios = new List<Usuario>();

            using (var db = new BaseContext())
            {

                var user = (from u in db.Usuario
                            join uc in db.UsuarioClienteSite on u.IdUsuario equals uc.IdUsuario
                            where uc.IdSite == idSite && u.FlAtivo == true
                            select u).ToList();

                usuarios.AddRange(user);
            }



            var usuariosLista = usuarios.Select(x => new { x.IdUsuario, x.NmCompleto }).ToList();

            ViewBag.Processo = lista;
            ViewBag.UsuarioFuncao = usuariosLista;
            //ViewBag.IdProcesso = Util.ObterProcessoSelecionado();

            //   if (((SelectList)ViewBag.Processo) == null)
            //   {
            //       ViewBag.Processo = new SelectList(new[] {
            //new SelectListItem  { Value="0", Text="Valores não encontrados" }
            //       }, "Value", "Text");
            //   }


            analiseCritica.Temas.ForEach(tema =>
            {
                if (tema.PossuiGestaoRisco || tema.PossuiInformarGestaoRisco)
                {
                    tema.AnaliseCritica = new AnaliseCritica();
                    tema.GestaoDeRisco.Emissor = new Usuario();
                    tema.GestaoDeRisco.Processo = new Processo();
                    tema.GestaoDeRisco.Site = new Site();

                }
            }
            );


            return View("Criar", analiseCritica);
        }

        public ActionResult PDF(int? id)
        {
            var analiseCritica = new AnaliseCritica();

            if (id != null)
            {
                analiseCritica = _analiseCriticaAppServico.GetById(Convert.ToInt32(id));
            }

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.Tema = "tema";
            //ViewBag.IdProcesso = Util.ObterProcessoSelecionado();

            analiseCritica.Temas.ForEach(tema =>
            {
                if (tema.PossuiGestaoRisco)
                {
                    tema.AnaliseCritica = new AnaliseCritica();
                    tema.GestaoDeRisco.Emissor = new Usuario();
                    tema.GestaoDeRisco.Processo = new Processo();
                    tema.GestaoDeRisco.Site = new Site();

                }
            }
            );


            var pdf = new ViewAsPdf
            {
                ViewName = "PDF",
                Model = analiseCritica,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "Analise Critica" + analiseCritica.IdAnaliseCritica + ".pdf"
            };

            return pdf;
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Salvar(AnaliseCritica analiseCritica)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var erros = new List<string>();

            try
            {
                analiseCritica.IdSite = Util.ObterSiteSelecionado();
                TrataTemasGestaoDeRisco(analiseCritica.Temas, analiseCritica.IdResponsavel, ref erros);

                _analiseCriticaServico.Valido(analiseCritica, ref erros);

                if (erros.Count == 0)
                {
                    if (EAtualizacao(analiseCritica.IdAnaliseCritica))
                    {
                        analiseCritica.Temas.ForEach(x =>
                        {
                            x.IdAnaliseCritica = analiseCritica.IdAnaliseCritica;
                        });
                        analiseCritica.Funcionarios.ForEach(x =>
                        {
                            x.IdAnaliseCritica = analiseCritica.IdAnaliseCritica;
                        });

                        _analiseCriticaAppServico.AtualizaAnaliseCritica(analiseCritica);

                    }
                    else
                    {
                        analiseCritica.DataCadastro = DateTime.Now;
                        _analiseCriticaAppServico.SalvarAnaliseCritica(analiseCritica);
                        EnfileirarEmailsAnaliseCritica(analiseCritica);
                        _analiseCriticaAppServico.AtualizaAnaliseCritica(analiseCritica);
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

            return Json(new { StatusCode = 200, Success = Traducao.AnaliseCritica.ResourceAnaliseCritica.AC_msg_save_valid }, JsonRequestBehavior.AllowGet);
        }

        private void EnfileirarEmailsAnaliseCritica(AnaliseCritica analiseCritica)
        {
            var urlAcesso = MontarUrlAnaliseCritica(analiseCritica.IdAnaliseCritica);
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\AnaliseCriticaDataProximaAta-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";


            try
            {
                var destinatario = _usuarioAppServico.GetById(analiseCritica.IdResponsavel).CdIdentificacao;
                string template = System.IO.File.ReadAllText(path);
                string conteudo = template;

                conteudo = conteudo.Replace("#NomeAnaliseCritica#", analiseCritica.Ata);
                conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);

                var filaEnvio = new FilaEnvio();
                filaEnvio.Assunto = Traducao.ResourceNotificacaoMensagem.MsgNotificacaoGestaoDeRiscos;
                filaEnvio.DataAgendado = analiseCritica.DataProximaAnalise;
                filaEnvio.DataInclusao = DateTime.Now;
                filaEnvio.Destinatario = destinatario;
                filaEnvio.Enviado = false;
                filaEnvio.Mensagem = conteudo;

                _filaEnvioServico.Enfileirar(filaEnvio);

                analiseCritica.IdFilaEnvio = filaEnvio.Id;
            }
            catch (Exception ex)
            {
                GravaLog(ex);
            }

        }

        private string MontarUrlAnaliseCritica(int idRegistro)
        {
            var dominio = "http://" + ConfigurationManager.AppSettings["Dominio"];

            return dominio + "AnaliseCritica/Editar/" + idRegistro.ToString();
        }

        private void TrataTemasGestaoDeRisco(List<AnaliseCriticaTema> temas, int IdResponsavelPorCriacaoDaNova, ref List<string> erros)
        {
            foreach (var tema in temas)
            {

                if (tema.PossuiInformarGestaoRisco == true)
                {
                    tema.GestaoDeRisco.IdEmissor = IdResponsavelPorCriacaoDaNova;
                    //tema.GestaoDeRisco.IdProcesso = Util.ObterProcessoSelecionado();
                    tema.GestaoDeRisco.IdSite = Util.ObterSiteSelecionado();
                    tema.GestaoDeRisco.StatusEtapa = tema.PossuiGestaoRisco == true ? (byte)EtapasRegistroConformidade.AcaoImediata : (byte)EtapasRegistroConformidade.Encerrada;
                    tema.GestaoDeRisco.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
                    tema.GestaoDeRisco.IdUsuarioAlterou = Util.ObterCodigoUsuarioLogado();
                    if (tema.PossuiGestaoRisco == true)
                    {
                        tema.GestaoDeRisco.IdResponsavelEtapa = tema.GestaoDeRisco.IdResponsavelInicarAcaoImediata.Value;
                    }

                    tema.GestaoDeRisco.TipoRegistro = "gr";
                    tema.GestaoDeRisco.FlDesbloqueado = tema.GestaoDeRisco.FlDesbloqueado > 0 ? (byte)0 : (byte)0;
                    tema.GestaoDeRisco.EProcedente = tema.PossuiGestaoRisco;
                    tema.GestaoDeRisco.IdProcesso = tema.IdProcesso;
                    //tema.GestaoDeRisco.IdProcesso = tema.Processo.IdProcesso;


                    _registroConformidadeAppServico.GestaoDeRiscoValida(tema.GestaoDeRisco, ref erros);
                }
                else
                {
                    tema.GestaoDeRisco = null;
                }
            }
        }

        private bool EAtualizacao(int id)
        {
            if (id == 0)
            {
                return false;
            }
            return true;
        }

        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var idSiteCorrente = Util.ObterSiteSelecionado();
            var analiseCriticas = _analiseCriticaAppServico.ObterPorIdSite(idSiteCorrente).Where(x => x.Ativo == true);

            return View(analiseCriticas);
        }

        [HttpGet]
        public JsonResult ObterUsuariosPorAnaliseCritica(int idAnaliseCritica)
        {
            var usuarios = new List<Usuario>();
            try
            {
                usuarios = _analiseCriticaAppServico.ObterUsuariosPorAnaliseCritica(idAnaliseCritica, Util.ObterSiteSelecionado());
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = Traducao.Resource.MsgErroContatoADM }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = HttpStatusCode.OK, Lista = usuarios }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalvaPDF(int id)
        {
            GeraArquivoZip(ControllerContext, "PDF", id);

            return View();
        }

        public ActionResult Imprimir(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();

            var analiseCritica = new AnaliseCritica();

            analiseCritica = _analiseCriticaAppServico.GetById(Convert.ToInt32(id));

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            ViewBag.Tema = "tema";

            analiseCritica.Temas.ForEach(tema =>
            {
                if (tema.PossuiGestaoRisco)
                {
                    tema.AnaliseCritica = new AnaliseCritica();
                    tema.GestaoDeRisco.Emissor = new Usuario();
                    tema.GestaoDeRisco.Processo = new Processo();
                    tema.GestaoDeRisco.Site = new Site();

                }
            }
            );

            var pdf = new ViewAsPdf
            {
                ViewName = "PDF",
                Model = analiseCritica,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "AnaliseCritica.pdf"
            };

            return pdf;
        }

        public JsonResult Excluir(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            try
            {
                var analiseCritica = _analiseCriticaAppServico.GetById(id);

                if (analiseCritica.Temas != null)
                {
                    analiseCritica.Temas.ForEach(x =>
                    {
                        if (x.GestaoDeRisco != null)
                        {
                            x.GestaoDeRisco.JustificativaAnulacao = "Anulado";
                            x.GestaoDeRisco.StatusEtapa = 0;
                        }
                    });
                }

                analiseCritica.Ativo = false;

                _analiseCriticaAppServico.Update(analiseCritica);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = 500 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Resource.SucessoAoExcluirORegistro }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadImageCkEditor(string CKEditorFuncNum, string CKEditor, string langCode)
        {
            var arquivo = Request.Files[0];

            if (arquivo.ContentLength <= 0)
                return null;

            // here logic to upload image
            // and get file path of the image

            const string pastaUpload = "/Content/ImagensCkEditor/";

            var nomeArquivo = Path.GetFileName(arquivo.FileName);
            var caminhoImagemServidor = Path.Combine(Server.MapPath(string.Format("~/{0}", pastaUpload)), nomeArquivo);
            arquivo.SaveAs(caminhoImagemServidor);

            var urlRetorno = string.Format("{0}{1}/{2}/{3}", Request.Url.GetLeftPart(UriPartial.Authority),
                Request.ApplicationPath == "/" ? string.Empty : Request.ApplicationPath,
                pastaUpload, nomeArquivo);

            // passing message success/failure
            string mensagemUsuario = Traducao.Resource.MsgImagemAddSucesso;

            // since it is an ajax request it requires this string
            var output = string.Format(
                "<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
                CKEditorFuncNum, urlRetorno, mensagemUsuario);

            return Content(output);
        }

    }
}
