using ApplicationService.Interface;
using ApplicationService.Interface.RH;
using Dominio.Entidade.RH;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.UI.Helpers;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class FuncionarioController : BaseController
    {
        private readonly IFuncionarioAppServico _funcionarioAppServico;
        private readonly ISubModuloAppServico _subModuloAservico;
        private readonly IUsuarioAppServico _usuarioAppServico;
        private readonly IProcessoAppServico _processoAppServico;
        private readonly IControladorCategoriasAppServico _controladorCategoriasServico;

        public FuncionarioController(IFuncionarioAppServico funcionarioAppServico,
                                     ISubModuloAppServico subModuloAservico,
                                     ILogAppServico logAppServico,
                                     IUsuarioAppServico usuarioAppServico,
                                     IProcessoAppServico processoAppServico,
            IControladorCategoriasAppServico controladorCategoriasServico) : base(logAppServico, usuarioAppServico, processoAppServico, controladorCategoriasServico)
        {
            _funcionarioAppServico = funcionarioAppServico;
            _subModuloAservico = subModuloAservico;
            _usuarioAppServico = usuarioAppServico;
            _processoAppServico = processoAppServico;
            _controladorCategoriasServico = controladorCategoriasServico;
        }

        public ActionResult Index()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var funcionarios = _funcionarioAppServico.Get(x => x.Ativo == true);

            return View(funcionarios);
        }

        public ActionResult Criar()
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var idSiteSelecionado = Util.ObterSiteSelecionado();
            var subModulos = _subModuloAservico.Get(x => x.Ativo == true && 
                                                    x.CodigoSite == idSiteSelecionado);

            return View(subModulos);
        }

        public JsonResult Salvar(Funcionario funcionario)
        {
            var erros = new List<string>();

            try
            {
                _funcionarioAppServico.Valido(funcionario, ref erros);

                if (erros.Count == 0)
                {
                    _funcionarioAppServico.Add(funcionario);
                }
                else
                {
                    return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = 500, Erro = erros }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Editar(int? id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            var funcionario = new Funcionario();

            if (id != null)
            {
                funcionario = _funcionarioAppServico.GetById(Convert.ToInt32(id));
            }

            return View(funcionario);
        }

        public JsonResult Excluir(int id)
        {
            ViewBag.IdSite = Util.ObterSiteSelecionado();
            try
            {
                var funcionario = _funcionarioAppServico.GetById(id);

                if (funcionario != null)
                {
                    funcionario.Ativo = false;

                    _funcionarioAppServico.Update(funcionario);
                }
            }
            catch (Exception ex)
            {
                GravaLog(ex);
                return Json(new { StatusCode = 500 }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { StatusCode = 200 }, JsonRequestBehavior.AllowGet);
        }
    }
}