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
    public class LicencaController : BaseController
    {

        private readonly ILicencaAppServico _licencaAppServico;
        private readonly ILicencaServico _licencaServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly ILogAppServico _logAppServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IFilaEnvioServico _filaEnvioServico;
        private readonly ICargoProcessoAppServico _cargoProcessoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;
        private readonly IUsuarioClienteSiteAppServico _usuarioClienteAppServico;

        public LicencaController(ILicencaAppServico licencaAppServico,
                                      ILicencaServico licencaServico,
                                      ILogAppServico logAppServico,
                                      IUsuarioAppServico usuarioAppServico,
                                      ICargoProcessoAppServico cargoProcessoAppServico,
                                      IProcessoAppServico processoAppServico,
                                      IControladorCategoriasAppServico controladorCategoriasServico,
                                      IFilaEnvioServico filaEnvioServico,
                                      IUsuarioClienteSiteAppServico usuarioClienteAppServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {

            _licencaAppServico = licencaAppServico;
            _licencaServico = licencaServico;
            _logAppServico = logAppServico;
            _usuarioAppServico = usuarioAppServico;
            _cargoProcessoAppServico = cargoProcessoAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
            _filaEnvioServico = filaEnvioServico;
            _usuarioClienteAppServico = usuarioClienteAppServico;
        }

        // GET: Instrumentos
        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var idSiteCorrente = Util.ObterSiteSelecionado();
            var idUsuario = Util.ObterCodigoUsuarioLogado();

            var idPerfil = Util.ObterPerfilUsuarioLogado();

            var comPermissao = EAdministradorOuCoordenador(idPerfil);

            var model = _licencaAppServico.GetAll().ToList();

            ViewBag.UsuarioPodeAlterar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(idUsuario, 9, 58);
            ViewBag.UsuarioPodeDeletar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(idUsuario, 9, 59);
            ViewBag.IdPerfil = idPerfil;
            ViewBag.FuncionalidadeCriarInstrumento = false;


            return View(model);
        }

        //[AutorizacaoUsuario((int)FuncoesInstrumento.CadastroDeInstrumento, (int)Funcionalidades.Instrumentos)]
        public ActionResult Criar()
        {
            var licenca = new Licenca();

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            //ViewBag.IdProcesso = Util.ObterProcessoSelecionado();
            ViewBag.IdFuncao = (int)FuncoesLicenca.Incluir;
            ViewBag.IdAprovaLicenca = Util.ObterCodigoUsuarioLogado();

            return View(licenca);
        }

        [HttpPost]
        [AutorizacaoUsuario((int)FuncoesLicenca.Incluir, (int)Funcionalidades.Licencas)]
        public JsonResult Criar(Licenca licenca)
        {
            var erros = new List<string>();

            try
            {
                

                return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        //[HttpPost]
        //[AutorizacaoUsuario((int)FuncoesInstrumento.CadastroDeInstrumento, (int)Funcionalidades.Instrumentos)]
        //public JsonResult Criar(Instrumento instrumento)
        //{
        //    var erros = new List<string>();

        //    try
        //    {
        //        instrumento.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();

        //        _instrumentoServico.Valido(instrumento, ref erros);
        //        var instrumentoByCodSigla = _instrumentoAppServico.Get(s => s.Numero == instrumento.Numero && s.IdSigla == instrumento.IdSigla).FirstOrDefault();

        //        if (instrumentoByCodSigla != null)
        //            erros.Add("Não foi possível salvar, pois já existe um Instrumento cadastrado com a mesma Sigla e Número.");

        //        if (erros.Count > 0)
        //        {
        //            return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            _instrumentoAppServico.Add(instrumento);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GravaLog(ex);
        //        erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
        //        return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
        //    }

        //    return Json(new { StatusCode = 200, IdInstrumento = instrumento.IdInstrumento, Success = Traducao.Instrumentos.ResourceInstrumentos.IN_msg_create_valid }, JsonRequestBehavior.AllowGet);
        //}

        private void TrataDadoCricaoInstrumento(Instrumento instrumento)
        {
            instrumento.DataCriacao = DateTime.Now;
            instrumento.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
            instrumento.valorAceitacao = instrumento.valorAceitacao.Replace('.', ',');
            instrumento.DataAlteracao = DateTime.Now;
            instrumento.Status = (byte)EquipamentoStatus.NaoCalibrado;
        }

        //public ActionResult Editar(int id)
        //{
        //    ViewBag.IdSite = Util.ObterSiteSelecionado();
        //    //ViewBag.IdProcesso = Util.ObterProcessoSelecionado();
        //    ViewBag.IdFuncao = 68;
        //    ViewBag.IdAprovadorCalibracao = Util.ObterCodigoUsuarioLogado();

        //    var instrumento = _instrumentoAppServico.GetById(id);

        //    ViewBag.Responsavel = _usuarioAppServico.GetById(instrumento.IdResponsavel.Value).NmCompleto;

        //    return View("Criar", instrumento);
        //}


        [HttpPost]
        public JsonResult Editar(Instrumento instrumento)
        {


            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Instrumentos.ResourceInstrumentos.IN_msg_save_valid }, JsonRequestBehavior.AllowGet);



        }

        public JsonResult Excluir(int id)
        {
            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Instrumentos.ResourceInstrumentos.IN_msg_save_valid }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult Detalhe(int id)
        {
            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Instrumentos.ResourceInstrumentos.IN_msg_save_valid }, JsonRequestBehavior.AllowGet);


        }


        public ActionResult SalvaPDF(int id)
        {
            GeraArquivoZip(ControllerContext, "PDF", id);

            return View();
        }

        public ActionResult Imprimir(int id)
        {
            //var analiseCritica = _instrumentoAppServico.GetById(id);

            var pdf = new ViewAsPdf
            {
                ViewName = "Criar",
                //Model = analiseCritica,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "Instrumento.pdf"
            };

            return pdf;
        }

        private void DeletarInstrumentoERelacionamentos(Instrumento objetoParaRemover)
        {
            //try
            //{
            //    if (objetoParaRemover != null)
            //    {
            //        List<int> idsCalibracao = new List<int>();
            //        List<int> idsCriterio = new List<int>();

            //        foreach (var calibracao in objetoParaRemover.Calibracao)
            //        {
            //            foreach (var criterio in calibracao.CriterioAceitacao)
            //                idsCriterio.Add(criterio.IdCriterioAceitacao);

            //            idsCalibracao.Add(calibracao.IdCalibracao);
            //        }

            //        _instrumentoAppServico.DeletarInstrumentoEDependencias(objetoParaRemover.IdInstrumento);

            //        for (int i = 0; i < idsCalibracao.Count; i++)
            //            _calibracaoAppServico.RemoverComRelacionamentos(idsCalibracao[i]);

            //        for (int i = 0; i < idsCriterio.Count; i++)
            //            _criterioAceitacaoAppServico.Remove(new CriterioAceitacao() { IdCriterioAceitacao = idsCriterio[i] });
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
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

    }
};