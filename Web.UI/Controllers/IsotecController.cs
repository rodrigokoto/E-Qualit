using System.Linq;
using System.Web.Mvc;
using Dominio.Enumerado;
using Web.UI.Helpers;
using ApplicationService.Interface;
using System.Threading;
using Dominio.Interface.Repositorio;
using System.Collections.Generic;
using Dominio.Entidade;
using System;
using Web.UI.Models;
using ApplicationService.Entidade;
using System.Configuration;

namespace Web.UI.Controllers
{
    
    public class IsotecController : Controller
    {
        private readonly ISiteRepositorio _siteRepositorio;
        private readonly IClienteAppServico _clienteAppServico;

        public IsotecController(ISiteRepositorio siteRepositorio, IClienteAppServico clienteAppServico)
        {
            _siteRepositorio = siteRepositorio;
            _clienteAppServico = clienteAppServico;
        }

        public ActionResult Index(string route)
        {

            var sites = _siteRepositorio.GetAll();

            List<string> clientes = new List<string>();

            foreach (var site in sites)
            {
                foreach (var clienteLogo in site.Cliente.ClienteLogo)
                {
                    var imgtype = clienteLogo.Anexo.Extensao.Substring(1, clienteLogo.Anexo.Extensao.Length - 1);
                    var imgSrcUsuario = string.Format("data:image/" + imgtype + ";base64," + Convert.ToBase64String(clienteLogo.Anexo.Arquivo));

                    clientes.Add(imgSrcUsuario);
                }
                
            }

            ViewBag.Site = sites;
            ViewBag.Cliente = clientes;

            if (route != "") {

                var clienteCtx = _clienteAppServico.Get(x => x.NmUrlAcesso == route).FirstOrDefault();

                if (clienteCtx != null)
                {
                    TrataImg(clienteCtx);

                }
                return View("../Login/Index" ,  clienteCtx);                
            }

            return View(); 
        }

        private void TrataImg(Cliente cliente)
        {
            if (cliente.ClienteLogo.Count > 0)
            {
                var anexo = cliente.ClienteLogo.FirstOrDefault().Anexo;
                cliente.ClienteLogoAux = new Dominio.Entidade.Anexo
                {
                    ArquivoB64 = String.Format("data:image/" + anexo.Extensao + ";base64," + Convert.ToBase64String(anexo.Arquivo))
                };
            }

        }

        [HttpPost]
        public ActionResult sentMessage(MailerViewModel mailerViewModel) {

            var sites = _siteRepositorio.GetAll();

            List<string> clientes = new List<string>();

            foreach (var site in sites)
            {
                foreach (var clienteLogo in site.Cliente.ClienteLogo)
                {
                    var imgtype = clienteLogo.Anexo.Extensao.Substring(1, clienteLogo.Anexo.Extensao.Length - 1);
                    var imgSrcUsuario = string.Format("data:image/" + imgtype + ";base64," + Convert.ToBase64String(clienteLogo.Anexo.Arquivo));

                    clientes.Add(imgSrcUsuario);
                }

            }

            ViewBag.Site = sites;
            ViewBag.Cliente = clientes;

            Email _email = new Email();

            _email.Assunto = "Nova mensagem de:" + mailerViewModel.Email + " Site Equalit";
            _email.De = ConfigurationManager.AppSettings["EmailDE"];
            _email.Para = "contato@equalit.com.br";
            _email.Conteudo = "Nova mensagem de:" + mailerViewModel.Nome + " " + mailerViewModel.Mensagem;
            _email.Servidor = ConfigurationManager.AppSettings["SMTPServer"];
            _email.Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            _email.EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSSL"]);
            _email.Enviar();

            return View("Index" , mailerViewModel);
        }
    }
}