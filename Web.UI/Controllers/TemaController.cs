using ApplicationService.Interface;
using System.Web.Mvc;
using Web.UI.Helpers;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class TemaController : Controller
    {
        private readonly IAnaliseCriticaTemaAppServico _temaServico;

        public TemaController(IAnaliseCriticaTemaAppServico temaServico)
        {
            _temaServico = temaServico;
        }

        // GET: TEma
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Editar(int idTema)
        {
            var tema = _temaServico.GetById(idTema);
            
            return Json(new { StatusCode = 200, Tema = tema }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Desativar(int idTema)
        {
            var tema = _temaServico.GetById(idTema);
            tema.Ativo = 0;

            _temaServico.Update(tema);

            return Json(new { StatusCode = 200 }, JsonRequestBehavior.AllowGet);
        }
    }
}