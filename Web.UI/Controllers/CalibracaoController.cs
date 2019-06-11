using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.UI.Helpers;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class CalibracaoController : BaseController
    {

        private readonly ICalibracaoAppServico _calibracaoAppServico;
        private readonly ICalibracaoServico _calibracaoServico;
        private readonly IArquivoCertificadoAnexoAppServico _arquivoCertificadoAnexoAppServico;
        private readonly IAnexoAppServico _anexoAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;

        private readonly ICriterioAceitacaoAppServico _criterioAceitacaoAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IFilaEnvioServico _filaEnvioServico;
        private readonly IInstrumentoAppServico _instrumentoServico;

        public CalibracaoController(ICalibracaoAppServico calibracaoAppServico,
                                    ICalibracaoServico calibracaoServico,
                                    IArquivoCertificadoAnexoAppServico arquivoCertificadoAnexoAppServico,
                                    IAnexoAppServico anexoAppServico,
                                    ICriterioAceitacaoAppServico criterioAceitacaoAppServico,
                                    ILogAppServico logAppServico,
                                    IUsuarioAppServico usuarioAppServico,
                                    IProcessoAppServico processoAppServico,
                                    IControladorCategoriasAppServico controladorCategoriasServico,
                                    IFilaEnvioServico filaEnvioServico,
                                    IInstrumentoAppServico instrumentoServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _calibracaoAppServico = calibracaoAppServico;
            _calibracaoServico = calibracaoServico;
            _arquivoCertificadoAnexoAppServico = arquivoCertificadoAnexoAppServico;
            _anexoAppServico = anexoAppServico;
            _criterioAceitacaoAppServico = criterioAceitacaoAppServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _filaEnvioServico = filaEnvioServico;
            _instrumentoServico = instrumentoServico;
        }

        // GET: Calibracao
        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var todos = _calibracaoAppServico.GetAll();

            return View(todos);
        }

        public ActionResult Criar(int? id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            return View();
        }

        [HttpPost]
        public JsonResult Criar(Calibracao calibracao)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            try
            {
                var erros = new List<string>();

                _calibracaoServico.Valido(calibracao, ref erros);

                if (erros.Count == 0 || calibracao.Aprovado == 3)
                {
                    calibracao.DataCriacao = DateTime.Now;
                    calibracao.DataAlteracao = DateTime.Now;
                    calibracao.DataCalibracao = DateTime.Now;
                    calibracao.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();

                    calibracao.ArquivoCertificadoAux.ForEach(anexo =>
                    {
                        anexo.Tratar();
                        if (anexo.IdAnexo == 0 && calibracao.IdCalibracao == 0)
                        {
                            calibracao.ArquivoCertificado.Add(new ArquivoCertificadoAnexo
                            {
                                Anexo = anexo,
                                Calibracao = calibracao
                            });
                        }
                        else if (anexo.IdAnexo == 0 && calibracao.IdCalibracao > 0)
                        {
                            calibracao.ArquivoCertificado.Add(new ArquivoCertificadoAnexo
                            {
                                Anexo = anexo,
                                IdCalibracao = calibracao.IdCalibracao,
                            });
                        }
                        else if (anexo.IdAnexo > 0 && calibracao.IdCalibracao > 0)
                        {
                            var anexoCtx = _anexoAppServico.GetById(anexo.IdAnexo);
                            anexoCtx.Arquivo = anexo.Arquivo;
                            anexoCtx.Extensao = anexo.Extensao;
                            anexoCtx.Nome = anexo.Nome;
                            anexoCtx.DtAlteracao = anexo.DtAlteracao;
                        }
                    });

                    EnfileirarEmailCalibracao(calibracao);


                    if (calibracao.CriterioAceitacao != null)
                    {

                        calibracao.CriterioAceitacao.ForEach(x =>
                        {
                            x.DtAlteracao = DateTime.Now;
                            x.DtInclusao = DateTime.Now;
                            x.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
                            x.Calibracao = calibracao;
                        });

                        _calibracaoAppServico.SalvarComCriteriosAceitacao(calibracao);
                    }
                    else
                    {
                        _calibracaoAppServico.SalvarCalibracao(calibracao);
                    }
                }
                else
                {
                    return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
                }

                //                calibracao.ArquivoCertificado = calibracao.ArquivoCertificado != null ? string.Format($"/content/cliente/{Util.ObterSiteSelecionado()}/Instrumento/Calibracao/{calibracao.ArquivoCertificado}") : "";

                if (calibracao.CriterioAceitacao != null)
                {
                    calibracao.CriterioAceitacao.ForEach(x => x.Calibracao = null);
                }

                var json = CalibracaoObjSemReferenciaCircular(calibracao);

                return Json(new { StatusCode = 200, calibracao = json, Success = Traducao.Resource.MsgCalibracaoSalva }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GravaLog(ex);

                return Json(new { StatusCode = (int)HttpStatusCode.InternalServerError, Erro = Traducao.Resource.MsgErroGravarDados }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Editar(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var calibracao = _calibracaoAppServico.GetById(id);

            var calibracaoJson = CalibracaoObjSemReferenciaCircular(calibracao);

            return Json(new { StatusCode = 200, Calibracao = calibracaoJson }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Editar(Calibracao calibracao)
        {
            var erros = new List<string>();

            _calibracaoServico.Valido(calibracao, ref erros);

            if (erros.Count == 0)
            {
                calibracao.DataAlteracao = DateTime.Now;
                calibracao.DataCalibracao = DateTime.Now;
                calibracao.DataCriacao = DateTime.Now;

                if (calibracao.IdFilaEnvio != null)
                {
                    var filaEnvio = _filaEnvioServico.ObterPorId(calibracao.IdFilaEnvio.Value);

                    if (filaEnvio.Enviado)
                        EnfileirarEmailCalibracao(calibracao);
                    else
                        filaEnvio.DataAgendado = calibracao.DataNotificacao;
                    _filaEnvioServico.Atualizar(filaEnvio);
                }
                else
                {
                    EnfileirarEmailCalibracao(calibracao);
                }


                if (calibracao.CriterioAceitacao != null)
                {
                    calibracao.CriterioAceitacao.ForEach(x => x.DtAlteracao = DateTime.Now);
                    calibracao.CriterioAceitacao.ForEach(x => x.DtInclusao = DateTime.Now);

                    foreach (var criterioaceitacao in calibracao.CriterioAceitacao)
                    {
                        if (criterioaceitacao.IdCriterioAceitacao == 0)
                            _criterioAceitacaoAppServico.Add(criterioaceitacao);
                        else
                        {
                            try
                            {
                                _criterioAceitacaoAppServico.Update(criterioaceitacao);
                            }
                            catch
                            {
                            }
                        }

                    }
                }

                _calibracaoAppServico.AtualizarCalibracao(calibracao);
            }
            else
            {
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detalhe(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var calibracaoDetalhe = _calibracaoAppServico.GetById(id);

            return View(calibracaoDetalhe);
        }

        public JsonResult Excluir(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var calibracaoRemover = _calibracaoAppServico.GetById(id);

            RemoverCalibracao(calibracaoRemover);

            return Json(new { StatusCode = 200 }, JsonRequestBehavior.AllowGet);
        }

        private void RemoverCalibracao(Calibracao objetoParaRemover)
        {
            try
            {
                if (objetoParaRemover != null)
                {

                    long idFilaEnvio = 0;

                    if (objetoParaRemover.IdFilaEnvio != null)
                        idFilaEnvio = objetoParaRemover.IdFilaEnvio.Value;

                    List<int> idsCriterio = new List<int>();

                    foreach (var criterio in objetoParaRemover.CriterioAceitacao)
                        idsCriterio.Add(criterio.IdCriterioAceitacao);

                    _calibracaoAppServico.RemoverComRelacionamentos(objetoParaRemover.IdCalibracao);

                    for (int i = 0; i < idsCriterio.Count; i++)
                        _criterioAceitacaoAppServico.Remove(new CriterioAceitacao() { IdCriterioAceitacao = idsCriterio[i] });

                    if (idFilaEnvio > 0)
                    {
                        var filaEnvio = _filaEnvioServico.ObterPorId(objetoParaRemover.IdFilaEnvio.Value);
                        _filaEnvioServico.Apagar(filaEnvio);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UploadArquivo(int id, int idSite)
        {
            var _nomeArquivo = Request.Files[0].FileName;

            string fullPath = "";

            fullPath = Server.MapPath(string.Format($"/content/cliente/{idSite}/Instrumento/Calibracao/"));
            Util.VerificaDiretorio(fullPath);

            Request.Files[0].SaveAs(string.Format("{0}{1}", fullPath, _nomeArquivo));

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(string.Format($"/content/cliente/{idSite}/Instrumento/Calibracao/{_nomeArquivo}"), "text/html");
        }


        [HttpPost]
        public ActionResult RemoverArquivo(string arquivo, int id, string tipo, string campo)
        {
            string fullPath = "";

            fullPath = Server.MapPath(string.Format($"/content/cliente/{id}/Instrumento/Calibracao/{arquivo}"));
            Util.DeletaArquivo(fullPath);

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content(arquivo, "text/html");
        }

        private Object CalibracaoObjSemReferenciaCircular(Calibracao calibracao)
        {
            if (calibracao.CriterioAceitacao != null)
            {
                calibracao.CriterioAceitacao.ForEach(x => x.Calibracao = null);
            }

            var calibracaoJson = new
            {
                IdCalibracao = calibracao.IdCalibracao,
                IdInstrumento = calibracao.IdInstrumento,
                IdFilaEnvio = calibracao.IdFilaEnvio,
                Certificado = calibracao.Certificado,
                OrgaoCalibrador = calibracao.OrgaoCalibrador,
                Aprovado = calibracao.Aprovado,
                Aprovador = new { IdUsuario = calibracao.Aprovador, Nome = calibracao.UsuarioAprovador != null ? calibracao.UsuarioAprovador.NmCompleto : calibracao.NomeUsuarioAprovador },
                Observacoes = calibracao.Observacoes,
                ArquivoCertificado = RetornaArquivosCertificado(calibracao),
                DataCalibracao = calibracao.DataCalibracao != null ? calibracao.DataCalibracao.Value.ToString(Traducao.Resource.formato_data) : "",
                DataProximaCalibracao = calibracao.DataProximaCalibracao != null ? calibracao.DataProximaCalibracao.Value.ToString("dd/MM/yyyy") : "",
                DataCriacao = calibracao.DataCriacao.ToString(Traducao.Resource.formato_data),
                DataAlteracao = calibracao.DataAlteracao.ToString(Traducao.Resource.formato_data),
                DataNotificacao = calibracao.DataNotificacao != null ? calibracao.DataNotificacao.Value.ToString(Traducao.Resource.formato_data) : "",
                DataRegistro = calibracao.DataRegistro != null ? calibracao.DataRegistro.Value.ToString(Traducao.Resource.formato_data) : "",
                IdUsuarioIncluiu = calibracao.IdUsuarioIncluiu,
                CriteriosAceitacao = calibracao.CriterioAceitacao
            };

            return calibracaoJson;
        }

        private List<Anexo> RetornaArquivosCertificado(Calibracao calibracao)
        {

            if (calibracao.ArquivoCertificado.Count > 0)
            {
                calibracao.ArquivoCertificado.Select(x => x.Anexo).ToList().ForEach(x =>
                {
                    x.ArquivoB64 = String.Format("data:application/" + x.Extensao + ";base64," + Convert.ToBase64String(x.Arquivo));
                    x.Arquivo = null;
                    x.ArquivoCertificadoAnexo = null;
                });

                return calibracao.ArquivoCertificado.Select(x => x.Anexo).ToList();
            }
            else
            {
                return new List<Anexo>();
            }

        }


        private string MontarUrlAcessoInstrumento(int idInstrumento)
        {
            var dominio = "http://" + ConfigurationManager.AppSettings["Dominio"];

            return dominio + "Instrumento/Editar/" + idInstrumento.ToString();
        }

        private void EnfileirarEmailCalibracao(Calibracao calibracao)
        {
            try
            {
                var instrumento = _instrumentoServico.GetById(calibracao.IdInstrumento);
                var urlAcesso = MontarUrlAcessoInstrumento(instrumento.IdInstrumento);
                string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + $@"Templates\InstrumentoCalibracao-" + System.Threading.Thread.CurrentThread.CurrentCulture.Name + ".html";
                string destinatario = _usuarioAppServico.GetById(instrumento.IdResponsavel.Value).CdIdentificacao;

                string template = System.IO.File.ReadAllText(path);
                string conteudo = template;

                conteudo = conteudo.Replace("#NuRegistro#", instrumento.Numero);
                conteudo = conteudo.Replace("#urlAcesso#", urlAcesso);

                var filaEnvio = new FilaEnvio();
                filaEnvio.Assunto = Traducao.ResourceNotificacaoMensagem.MsgNotificacaoGestaoDeRiscos;
                filaEnvio.DataAgendado = calibracao.DataNotificacao;
                filaEnvio.DataInclusao = DateTime.Now;
                filaEnvio.Destinatario = destinatario;
                filaEnvio.Enviado = false;
                filaEnvio.Mensagem = conteudo;

                _filaEnvioServico.Enfileirar(filaEnvio);

                calibracao.IdFilaEnvio = filaEnvio.Id;
            }
            catch (Exception ex)
            {
                GravaLog(ex);
            }
        }

    }
}