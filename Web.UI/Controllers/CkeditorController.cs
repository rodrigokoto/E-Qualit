using ApplicationService.Interface;
using System;
using System.IO;
using System.Web.Mvc;

namespace Web.UI.Controllers
{
	public class CkeditorController : Controller
	{
		private readonly IClienteAppServico _clienteAppServico;

		public CkeditorController(IClienteAppServico clienteAppServico)
		{
			_clienteAppServico = clienteAppServico;
		}

		// GET: Ckeditor
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult UploadImageCkEditor(string CKEditorFuncNum, string CKEditor, string langCode)
		{
			var idCliente = Web.UI.Helpers.Util.ObterClienteSelecionado();

			string path = System.Web.HttpContext.Current.Server.MapPath("~\\Content\\ImagensCkEditor\\Arquivos\\" + idCliente + "\\DocDocumento\\");
			DirectoryInfo di = new DirectoryInfo(path);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

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
			 
			string pastaUpload = "/Content/ImagensCkEditor/Arquivos/" + idCliente + "/DocDocumento";

			Random _rdm = new Random();
			int hash = _rdm.Next(1000, 9999);

			var nomeArquivo = hash.ToString() + "-" + Path.GetFileName(arquivo.FileName);
			path = path.Replace("\\", "/");
			var caminhoImagemServidor = Path.Combine(Server.MapPath(string.Format("~/{0}", pastaUpload)),  nomeArquivo );
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