using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Web.UI.Helpers;

namespace Web.UI.Controllers
{
    [VerificaIntegridadeLogin]
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadFile()
        {
            var _nomeArquivo = Request.Files[0].FileName;

            byte[] imageArray = System.IO.File.ReadAllBytes(Server.MapPath("/Content/cliente/avatar.png"));

            string urlArquivo = "/Content/cliente/avatar.png";
            //window.opener.CKEDITOR.tools.callFunction(1, url);
            var a = "<script type='text/javascript'>$('.cke_dialog_image_url input[type=text]').val('" + urlArquivo + "')</script></body></html>";

            return new FileContentResult(imageArray, "image/jpeg");
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (file.ContentLength <= 0)
                return null;

            // here logic to upload image
            // and get file path of the image

            const string uploadFolder = "Assets/img/";

            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath(string.Format("~/{0}", uploadFolder)), fileName);
            file.SaveAs(path);

            var url = string.Format("{0}{1}/{2}/{3}", Request.Url.GetLeftPart(UriPartial.Authority),
                Request.ApplicationPath == "/" ? string.Empty : Request.ApplicationPath,
                uploadFolder, fileName);

            // passing message success/failure
            string message = Traducao.Resource.MsgImagemAddSucesso;

            // since it is an ajax request it requires this string
            var output = string.Format(
                "<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
                CKEditorFuncNum, url, message);

            return Content(output);
        }
    }
}