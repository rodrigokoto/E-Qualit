using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.UI.Helpers;
using ApplicationService.Interface;
using Dominio.Entidade;
using System.Net;
using System.Linq;
using Dominio.Interface.Servico;
using Dominio.Enumerado;
using System.Web;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    [ValidaUsuario]
    public class CargoController : BaseController
    {
        private readonly ILogAppServico _logAppServico;
        private readonly ICargoAppServico _cargoAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly ISiteModuloAppServico _siteModuloAppServico;
        private readonly IFuncaoAppServico _funcaoAppServico;
        private readonly ICargoProcessoAppServico _cargoProcessoAppServico;

        private readonly ICargoServico _cargoServico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public CargoController(ILogAppServico logAppServico,
            IProcessoAppServico processoAppServico,
            ISiteModuloAppServico siteModuloAppServico,
            ICargoAppServico siteAppServico,
            IFuncaoAppServico funcaoAppServico,
            ICargoProcessoAppServico cargoProcessoAppServico,
            ICargoServico cargoServico,
            IUsuarioAppServico usuarioAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _logAppServico = logAppServico;
            _processoAppServico = processoAppServico;
            _siteModuloAppServico = siteModuloAppServico;
            _cargoAppServico = siteAppServico;
            _funcaoAppServico = funcaoAppServico;
            _cargoProcessoAppServico = cargoProcessoAppServico;
            _cargoServico = cargoServico;
            _usuarioAppServico = usuarioAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        public ActionResult Index(int id)
        {

            ViewBag.IdSite = Util.ObterSiteSelecionado();
            SetCookieSiteSelecionado(id);

            var cargos = _cargoAppServico.ObtemCargosPorSite(id);
            ViewBag.IdSite = id;
            return View(cargos);
        }

        private void SetCookieSiteSelecionado(int idSite)
        {
            var site = _siteModuloAppServico.Get(x=>x.IdSite == idSite).FirstOrDefault().Site;

            HttpCookie cookie = Request.Cookies["siteSelecionado"];

            cookie.Value = site.IdSite.ToString();

            cookie.Expires = DateTime.Now.AddDays(1);

            Response.Cookies.Set(cookie);

            Session.Add("siteFrase", site.DsFrase);

            Session["siteImg"] = String.Format("data:application/" + site.SiteAnexo.FirstOrDefault().Anexo.Extensao + ";base64," + Convert.ToBase64String(site.SiteAnexo.FirstOrDefault().Anexo.Arquivo));
        }

        public ActionResult AtivaInativa(int idCargo)
        {

            var erros = new List<string>();

            var resposta = _cargoAppServico.AtivarInativar(idCargo);

            if (!resposta)
            {
                return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Site.ResourceSite.Site_msg_icone_ativo_valid }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Criar(int idSite)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var cargo = new Cargo();
            cargo.IdSite = idSite;
            ViewBag.Processos = _processoAppServico.ListaProcessosPorSite(cargo.IdSite).Take(1).ToList();
            var modulos = _siteModuloAppServico.ObterPorSite(idSite);

            List<Funcionalidade> funcionalidades = new List<Funcionalidade>();

            modulos.ForEach(x => {
                x.Funcionalidade.Funcoes = x.Funcionalidade.Funcoes.OrderBy(s => s.NuOrdem).ToList();
            });

            modulos.ForEach(x => {
                if (!funcionalidades.Select(y => y.IdFuncionalidade).Contains(x.IdFuncionalidade))
                {
                    funcionalidades.Add(x.Funcionalidade);
                }
            });

            

            ViewBag.Modulos = modulos;
            ViewBag.Funcionalidades = funcionalidades.Where(x=> x.Ativo == true).ToList();

            return View(cargo);
        }

        public ActionResult Editar(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var cargo = _cargoAppServico.GetById(id);
            ViewBag.Processos = _processoAppServico.ListaProcessosPorSite(cargo.IdSite).Take(1).ToList();
            var mudulos = _siteModuloAppServico.ObterPorSite(cargo.IdSite);

            List<Funcionalidade> funcionalidades = new List<Funcionalidade>();

            mudulos.ForEach(x => {
                if (!funcionalidades.Select(y => y.IdFuncionalidade).Contains(x.IdFuncionalidade))
                {
                    funcionalidades.Add(x.Funcionalidade);
                }
            });



            ViewBag.Modulos = mudulos;
            ViewBag.Funcionalidades = funcionalidades.Where(x => x.Ativo).ToList();

            return View("Criar", cargo);
        }

        [HttpPost]
        public JsonResult Salvar(Cargo cargo)
        {
            var erros = new List<string>();
            try
            {
                var idPerfil = Util.ObterPerfilUsuarioLogado();

                cargo.DtInclusao = DateTime.Now;

                if (idPerfil == (int)PerfisAcesso.Colaborador)
                {
                    erros.Add(Traducao.Resource.MsgUsuarioLogadoNaoCoordenador);
                    return Json(new { StatusCode = (int)HttpStatusCode.BadRequest, Erro = erros }, JsonRequestBehavior.AllowGet);
                }

                _cargoServico.Valida(cargo, ref erros);

                if (erros.Count != 0)
                {
                    return Json(new { StatusCode = 505, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (cargo.IdCargo > 0)
                    {
                        AtualizaCargosProcesso(cargo.CargoProcessos.ToList(), cargo.IdCargo);
                        var cargoCtx = _cargoAppServico.GetById(cargo.IdCargo);
                        cargoCtx.NmNome = cargo.NmNome;
                        _cargoAppServico.Update(cargoCtx);
                    }
                    else
                    {
                        cargo.Ativo = true;
                        cargo.CargoProcessos.ToList().ForEach(x => x.Cargo = cargo);
                        _cargoAppServico.Add(cargo);
                    }
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_invalid_backend);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = (int)HttpStatusCode.OK, Success = Traducao.Cargo.ResourceCargo.Cargo_msg_salva_valid }, JsonRequestBehavior.AllowGet);
        }

        private void AtualizaCargosProcesso(List<CargoProcesso> listaCargoProcesso, int idCargo)
        {
            int idSite = Util.ObterSiteSelecionado();

            var listaDosQueSeraoAdicionados = listaCargoProcesso.Where(x => x.IdCargoProcesso == 0).ToList();
            var listaCtx = _cargoProcessoAppServico.Get(x => x.IdCargo == idCargo && x.Processo.IdSite == idSite).ToList();

            List<int> listaQueSeraDeletados = new List<int>();

            listaCtx.ForEach(x =>
            {
                if(!listaCargoProcesso.Select(y=> y.IdFuncao).Contains(x.IdFuncao)) {
                    listaQueSeraDeletados.Add(x.IdCargoProcesso);
                }
            });
            

            if (listaDosQueSeraoAdicionados.Count > 0)
            {
                var cargo = _cargoAppServico.GetById(idCargo);
                var processos = _processoAppServico.ListaProcessosPorSite(cargo.IdSite).ToList();

                foreach (var itemAdd in listaDosQueSeraoAdicionados)
                {
                    foreach (var item in processos)
                    {
                        itemAdd.IdProcesso = item.IdProcesso;
                        _cargoProcessoAppServico.Add(itemAdd);
                    }
                }
            }

            if (listaQueSeraDeletados.Count > 0)
            {
                foreach(var i in listaQueSeraDeletados)
                {

                    var deletado = listaCtx.FirstOrDefault(x => x.IdCargoProcesso == i);

                    var deletados = _cargoProcessoAppServico.Get(x => x.IdCargo == deletado.IdCargo && x.IdFuncao == deletado.IdFuncao).ToList();

                    foreach(var item in deletados)
                    {
                        _cargoProcessoAppServico.Remove(item);
                    }
                }                
            }
        }

        public JsonResult ObterPorSite(int idSite)
        {
            var erros = new List<string>();

            try
            {
                var cargos = new List<Cargo>();
                var cargosCTX = _cargoAppServico.Get(x => x.IdSite == idSite);

                foreach (var cargoCTX in cargosCTX)
                {
                    cargos.Add(new Cargo
                    {
                        IdCargo = cargoCTX.IdCargo,
                        NmNome = cargoCTX.NmNome,
                        IdSite = cargoCTX.IdSite
                    });
                }

                return Json(new { StatusCode = 200, Cargo = cargos }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                erros.Add(Traducao.Resource.MsgErroCargoSite);

                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AcessoAdmin]
        public JsonResult Excluir(int id)
        {
            var erros = new List<string>();

            try
            {
                _cargoServico.Excluir(id);
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                erros.Add(Traducao.Shared.ResourceMensagens.Mensagem_erro_ExcluirCargo);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200, Success = Traducao.Cargo.ResourceCargo.Cargo_msg_exluir }, JsonRequestBehavior.AllowGet);
        }

    }
}