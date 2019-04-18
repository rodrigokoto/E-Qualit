using ApplicationService.Interface.RH;
using Rotativa;
using Rotativa.Options;
using System.Web.Mvc;
using Web.UI.Helpers;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class EpiController : Controller
    {
        public readonly IEpiAppServico _epiAppServico;

        public EpiController(IEpiAppServico epiAppServico)
        {
            _epiAppServico = epiAppServico;
        }

        public ActionResult PDFDownload(int id)
        {
            var epi = _epiAppServico.GetById(id);

            var pdf = new ViewAsPdf
            {
                ViewName = "Comprovante",
                Model = epi,
                PageOrientation = Orientation.Portrait,
                PageSize = Size.A4,
                PageMargins = new Margins(10, 15, 10, 15),
                FileName = "Comprovante.pdf"
            };

            return pdf;
        }

        public ActionResult Comprovante()
        {
            return View();
        }
    }
}