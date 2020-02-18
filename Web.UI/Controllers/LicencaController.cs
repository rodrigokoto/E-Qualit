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
using DAL.Context;
using System.Data.Entity;

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
        private readonly IArquivoLicencaAnexoAppServico _arquivoLicencaAnexoAppServico;

        public LicencaController(ILicencaAppServico licencaAppServico,
                                      ILicencaServico licencaServico,
                                      ILogAppServico logAppServico,
                                      IUsuarioAppServico usuarioAppServico,
                                      ICargoProcessoAppServico cargoProcessoAppServico,
                                      IProcessoAppServico processoAppServico,
                                      IControladorCategoriasAppServico controladorCategoriasServico,
                                      IFilaEnvioServico filaEnvioServico,
                                      IUsuarioClienteSiteAppServico usuarioClienteAppServico,
                                      IArquivoLicencaAnexoAppServico arquivoLicencaAnexoAppServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
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
            _arquivoLicencaAnexoAppServico = arquivoLicencaAnexoAppServico;
        }

        // GET: Instrumentos
        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var idSiteCorrente = Util.ObterSiteSelecionado();
            var idUsuario = Util.ObterCodigoUsuarioLogado();
            var idCliente = Util.ObterClienteSelecionado();

            var idPerfil = Util.ObterPerfilUsuarioLogado();

            var comPermissao = EAdministradorOuCoordenador(idPerfil);

            var model = _licencaAppServico.GetAll().Where(x => x.Idcliente == idCliente).ToList();

            ViewBag.UsuarioPodeAlterar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(idUsuario, 9, 58);
            ViewBag.UsuarioPodeDeletar = comPermissao ? comPermissao : _usuarioAppServico.PossuiAcesso(idUsuario, 9, 59);
            ViewBag.IdPerfil = idPerfil;
            
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
        //[AutorizacaoUsuario((int)FuncoesLicenca.Incluir, (int)Funcionalidades.Licencas)]
        public JsonResult Criar(Licenca licenca)
        {
            var erros = new List<string>();
            licenca.DataCriacao = DateTime.Now;
            try
            {
                licenca.Idcliente = Util.ObterClienteSelecionado();

                _licencaServico.Valido(licenca, ref erros);

                if (erros.Count > 0)
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _licencaAppServico.Add(licenca);
                    _licencaAppServico.SalvarArquivoLicenca(licenca);
                }
                return Json(new { StatusCode = 200, IdLicenca = licenca.IdLicenca, Success = "Licença incluida com sucesso" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }

       
        private void TrataDadoCricaoInstrumento(Instrumento instrumento)
        {
            instrumento.DataCriacao = DateTime.Now;
            instrumento.IdUsuarioIncluiu = Util.ObterCodigoUsuarioLogado();
            instrumento.valorAceitacao = instrumento.valorAceitacao.Replace('.', ',');
            instrumento.DataAlteracao = DateTime.Now;
            instrumento.Status = (byte)EquipamentoStatus.NaoCalibrado;
        }
        public ActionResult Exibir(int id) {
            ViewBag.IdSite = Util.ObterSiteSelecionado();


            var licenca = _licencaAppServico.GetById(id);
            licenca.ArquivosLicencaAux.AddRange(licenca.ArquivoLicenca.Select(x => x.Anexo));
            licenca.ArquivosLicencaAnexos = _arquivoLicencaAnexoAppServico.Get(r => r.IdLicenca == licenca.IdLicenca);



            ViewBag.Responsavel = _usuarioAppServico.GetById(licenca.IdResponsavel).NmCompleto;

            return View("Exibir", licenca);
        }


        public ActionResult Editar(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();


            var licenca = _licencaAppServico.GetById(id);
            licenca.ArquivosLicencaAux.AddRange(licenca.ArquivoLicenca.Select(x => x.Anexo));
            licenca.ArquivosLicencaAnexos = _arquivoLicencaAnexoAppServico.Get(r => r.IdLicenca == licenca.IdLicenca);

     

            ViewBag.Responsavel = _usuarioAppServico.GetById(licenca.IdResponsavel).NmCompleto;

            return View("Criar", licenca);
        }


        [HttpPost]
        public JsonResult Editar(Licenca licenca)
        {

            var erros = new List<string>();

            try
            {
                licenca.Idcliente = Util.ObterClienteSelecionado();

                _licencaServico.Valido(licenca, ref erros);
                if (erros.Count > 0)
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _licencaAppServico.Update(licenca);
                    foreach (var item in licenca.ArquivoLicenca)
                    {
                        _arquivoLicencaAnexoAppServico.Remove(item);
                    }
                    _licencaAppServico.SalvarArquivoLicenca(licenca);
                }
                return Json(new { StatusCode = 200, IdLicenca = licenca.IdLicenca, Success = "Licença alterada com sucesso" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
                throw;
            }

        }

        public JsonResult Excluir(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();

            

            try
            {

                RemoverFilhos(id);

              

                var ctx = new BaseContext();

                var licenca = ctx.Licenca.Find(id);
                ctx.Entry(licenca).State = EntityState.Deleted;
                ctx.SaveChanges();

                return Json(new { StatusCode = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = ex }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }


        public void RemoverFilhos(int id)
        {
            var licencaParaDeletar = _licencaAppServico.GetById(id);

            foreach (var item in licencaParaDeletar.ArquivoLicenca)
            {
                _arquivoLicencaAnexoAppServico.Remove(_arquivoLicencaAnexoAppServico.GetById(item.IdArquivoLicencaAnexo));

            }

        }
        public void RemoverLicenca(Licenca licenca)
        {
            _licencaAppServico.Remove(licenca);
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