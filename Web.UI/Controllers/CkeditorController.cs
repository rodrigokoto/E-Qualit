using System;
using System.IO;
using System.Web.Mvc;

namespace Web.UI.Controllers
{
	public class CkeditorController : Controller
	{


		// GET: Ckeditor
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult UploadImageCkEditor(string CKEditorFuncNum, string CKEditor, string langCode)
		{

			var output = string.Empty;

			var arquivo = Request.Files[0];

			if (arquivo.ContentLength <= 0)
				return null;
			if (arquivo.ContentLength > 10485760)
			{

				output = string.Format(
					"<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
					CKEditorFuncNum, "", "Arquivo deve ter no maximo 10MB");
				return Content(output);
			}
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
			output = string.Format(
				"<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
				CKEditorFuncNum, urlRetorno, mensagemUsuario);

			

			return Content(output);
		}
	}
}