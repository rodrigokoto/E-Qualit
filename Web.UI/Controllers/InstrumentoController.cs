using ApplicationService.Enum;
using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Enumerado;
using Dominio.Interface.Servico;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Rotativa;
using Rotativa.Options;
using Web.UI.Helpers;
using System.Linq;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    [SitePossuiModulo(9)]
    public class InstrumentoController : BaseController
    {
        private readonly IInstrumentoAppServico _instrumentoAppServico;
        private readonly IInstrumentoServico _instrumentoServico;

        private readonly ICalibracaoAppServico _calibracaoAppServico;

        private readonly ICriterioAceitacaoAppServico _criterioAceitacaoAppServico;

        private readonly ILogAppServico _logAppServico;

        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly ICargoProcessoAppServico _cargoProcessoAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IFilaEnvioServico _filaEnvioServico;

        public InstrumentoController(IInstrumentoAppServico instrumentoAppServico,
                                      IInstrumentoServico instrumentoServico,
                                      ICalibracaoAppServico calibracaoAppServico,
                                      ICriterioAceitacaoAppServico criterioAceitacaoAppServico,
                                      ILogAppServico logAppServico,
                                      IUsuarioAppServico usuarioAppServico,
                                      ICargoProcessoAppServico cargoProcessoAppServico,
                                      IProcessoAppServico processoAppServico,
                                      IControladorCategoriasAppServico controladorCategoriasServico,
                                      IPendenciaAppServico pendenciaAppServico,
                                      IFilaEnvioServico filaEnvioServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico,  pendenciaAppServico)
        {
            _instrumentoAppServico = instrumentoAppServico;
            _instrumentoServico = instrumentoServico;
            _calibracaoAppServico = calibracaoAppServico;
            _criterioAceitacaoAppServico = criterioAceitacaoAppServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _cargoProcessoAppServico = cargoProcessoAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _filaEnvioServico = filaEnvioServico;
        }

        // GET: Instrumentos
        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var idSiteCorrente = Util.ObterSiteSelecionado();
            var idUsuario = Util.ObterCodigoUsuarioLogado();


            var instrumentos = _instrumentoAppServico.Get(x => x.IdSite == idSiteCorrente).OrderByDescending(x => x.IdInstrumento).ToList();

            var idPerfil = Util.ObterPerfilUsuarioLogado();

            var comPermissao = EAdministradorOuCoordenador(idPerfil);

            ViewBag.UsuarioPodeAlterar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(idUsuario, 9, 58);
            ViewBag.UsuarioPodeDeletar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(idUsuario, 9, 59);
            ViewBag.IdPerfil = idPerfil;
            ViewBag.FuncionalidadeCriarInstrumento = false;
            if (idPerfil == 4)
            {
                ViewBag.FuncionalidadeCriarInstrumento = _cargoProcessoAppServico.PossuiAcessoAFuncao(idUsuario, 57);

            }

            return View(instrumentos);
        }

        [AutorizacaoUsuario((int)FuncoesInstrumento.CadastroDeInstrumento, (int)Funcionalidades.Instrumentos)]
        public ActionResult Criar()
        {
            var instrumento = new Instrumento();

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            //ViewBag.IdProcesso = Util.ObterProcessoSelecionado();
            ViewBag.IdFuncao = (int)FuncoesInstrumento.CadastroDeInstrumento;
            ViewBag.IdAprovadorCalibracao = Util.ObterCodigoUsuarioLogado();

            return View(instrumento);
        }

        [HttpPost]
        [AutorizacaoUsuario((int)FuncoesInstrumento.CadastroDeInstrumento, (int)Funcionalidades.Instrumentos)]
        public JsonResult Criar(Instrumento instrumento)
        {
            var erros = new List<string>();

            try
            {
                instrumento.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();

                _instrumentoServico.Valido(instrumento, ref erros);
                var instrumentoByCodSigla = _instrumentoAppServico.Get(s => s.Numero == instrumento.Numero && s.IdSigla == instrumento.IdSigla).FirstOrDefault();

                if (instrumentoByCodSigla != null)
                    erros.Add("Não foi possível salvar, pois já existe um Instrumento cadastrado com a mesma Sigla e Número.");

                if (erros.Count > 0)
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _instrumentoAppServico.Add(instrumento);
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, IdInstrumento = instrumento.IdInstrumento, Success = Traducao.Instrumentos.ResourceInstrumentos.IN_msg_create_valid }, JsonRequestBehavior.AllowGet);
        }

        private void TrataDadoCricaoInstrumento(Instrumento instrumento)
        {
            instrumento.DataCriacao = DateTime.Now;
            instrumento.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
            instrumento.valorAceitacao = instrumento.valorAceitacao.Replace('.', ',');
            instrumento.DataAlteracao = DateTime.Now;
            instrumento.Status = (byte)EquipamentoStatus.NaoCalibrado;
        }

        public ActionResult Editar(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            //ViewBag.IdProcesso = Util.ObterProcessoSelecionado();
            ViewBag.IdFuncao = 68;
            ViewBag.IdAprovadorCalibracao = Util.ObterCodigoUsuarioLogado();

            var instrumento = _instrumentoAppServico.GetById(id);

            instrumento.Calibracao = instrumento.Calibracao.OrderBy(x => x.DataProximaCalibracao).ToList();

            ViewBag.Responsavel = _usuarioAppServico.GetById(1).NmCompleto;

            if (instrumento.Calibracao.Count == 0)
            {
                ViewBag.DtUltimaCalibracao = DateTime.Now;
            }
            else
            {
             
                if (instrumento.Calibracao.Last().DataProximaCalibracao != null)
                    ViewBag.DtUltimaCalibracao = (DateTime)instrumento.Calibracao.Last().DataProximaCalibracao;
                else
                    ViewBag.DtUltimaCalibracao = string.Empty;
            }

            return View("Criar", instrumento);
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
                decimal numeroDocumento = _instrumentoServico.GeraProximoNumeroRegistro(Util.ObterSiteSelecionado(), null, id);

                var numeroEhInteiro = verificaSeEhInteiro(numeroDocumento);

                return Json(new { StatusCode = (int)HttpStatusCode.OK, Retorno = (numeroEhInteiro ? numeroDocumento.ToString("000") : numeroDocumento.ToString().Replace(".", ",")) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = (int)HttpStatusCode.InternalServerError }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PDF(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            //ViewBag.IdProcesso = Util.ObterProcessoSelecionado();
            ViewBag.IdFuncao = 68;
            ViewBag.IdAprovadorCalibracao = Util.ObterCodigoUsuarioLogado();

            var instrumento = _instrumentoAppServico.GetById(id);

            var pdf = new ViewAsPdf
            {
                ViewName = "PDF",
                Model = instrumento,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = Traducao.Instrumentos.ResourceInstrumentos.IN_lbl_placeholder_Instrumento + instrumento.IdInstrumento + ".pdf"
            };

            return pdf;

            //return View("PDF", instrumento);
        }

        [HttpGet]
        public ActionResult Destravar(int IdInstrumento)
        {
            var instrumento = _instrumentoAppServico.Get(x => x.IdInstrumento == IdInstrumento).FirstOrDefault();
            instrumento.FlagTravado = false;
            _instrumentoAppServico.Update(instrumento);

            return RedirectToAction("Editar", new { id = IdInstrumento });
        }

        [HttpPost]
        public JsonResult Editar(Instrumento instrumento)
        {

            var erros = new List<string>();

            try
            {
                instrumento.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();

                TrataCalibracoes(instrumento);

                _instrumentoServico.Valido(instrumento, ref erros);

                if (erros.Count == 0)
                {
                    instrumento.DataAlteracao = DateTime.Now;
                    instrumento.IdProcesso = instrumento.IdProcesso == 0 ? null : instrumento.IdProcesso;
                    _instrumentoAppServico.Update(instrumento);

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

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Instrumentos.ResourceInstrumentos.IN_msg_save_valid }, JsonRequestBehavior.AllowGet);



        }

        private void TrataCalibracoes(Instrumento instrumento)
        {
            instrumento.Calibracao.ForEach(calibracao =>
            {
                if (calibracao.IdCalibracao == 0)
                {
                    calibracao.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
                    calibracao.DataCriacao = DateTime.Now;
                }
                else
                    calibracao.DataAlteracao = DateTime.Now;
            });
        }





        public JsonResult Excluir(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var instrumentoParaDeletar = _instrumentoAppServico.GetById(id);

            DeletarInstrumentoERelacionamentos(instrumentoParaDeletar);

            return Json(new { StatusCode = 200 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detalhe(int id)
        {
            var instrumentoDetalhe = _instrumentoAppServico.GetById(id);

            return View(instrumentoDetalhe);
        }


        public ActionResult SalvaPDF(int id)
        {
            GeraArquivoZip(ControllerContext, "PDF", id);

            return View();
        }

        public ActionResult Imprimir(int id)
        {
            var analiseCritica = _instrumentoAppServico.GetById(id);

            var pdf = new ViewAsPdf
            {
                ViewName = "Criar",
                Model = analiseCritica,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "Instrumento.pdf"
            };

            return pdf;
        }

        private void DeletarInstrumentoERelacionamentos(Instrumento objetoParaRemover)
        {
            try
            {
                if (objetoParaRemover != null)
                {
                    List<int> idsCalibracao = new List<int>();
                    List<int> idsCriterio = new List<int>();

                    foreach (var calibracao in objetoParaRemover.Calibracao)
                    {
                        foreach (var criterio in calibracao.CriterioAceitacao)
                            idsCriterio.Add(criterio.IdCriterioAceitacao);

                        idsCalibracao.Add(calibracao.IdCalibracao);
                    }

                    _instrumentoAppServico.DeletarInstrumentoEDependencias(objetoParaRemover.IdInstrumento);

                    for (int i = 0; i < idsCalibracao.Count; i++)
                        _calibracaoAppServico.RemoverComRelacionamentos(idsCalibracao[i]);

                    for (int i = 0; i < idsCriterio.Count; i++)
                        _criterioAceitacaoAppServico.Remove(new CriterioAceitacao() { IdCriterioAceitacao = idsCriterio[i] });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        bool EAdministradorOuCoordenador(int perfil)
        {
            switch (perfil)
            {
                case (int)PerfisAcesso.Administrador:
                    return true;
                case (int)PerfisAcesso.Coordenador:
                    return true;
                default:
                    return false;
            }
        }
    }
};