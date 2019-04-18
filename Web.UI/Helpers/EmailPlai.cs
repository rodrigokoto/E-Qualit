using Dominio.Entidade;
using Dominio.Interface.Servico;
using System;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Http.Routing;

namespace Web.UI.Helpers
{
    public class EmailPlai : IEmailPlaiServico
    {
        string _urlBase;
        public EmailPlai()
        {

            var request = HttpContext.Current.Request;
            UrlHelper helper = new UrlHelper();
            var site = helper.Content("~");

            site = site.Substring(0, site.Length - 1);
            _urlBase = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, site);
        }

        public void EnviarNotificacao(Plai plai)
        {
            var site = 5;
    
            if (plai.Pai.Usuario == null)
            {
                plai.Pai.Usuario = new Usuario();
            }

            var template = TemplateNotificarGestor(plai.Pai.Usuario.NmCompleto);

            EnviarEmail(plai.Pai.Usuario.CdIdentificacao, 
                        template,
                        plai.Pai.Usuario.NmCompleto + ", um PLAI foi trocado de mês.");
        }

        private static string TemplateNotificarGestor(string nome)
        {
            return "Teste";
            //return Resources.EmailResource.NotificarResponsavel
            //    .Replace("@NOME", nome)
            //    .Replace("@FANTASIA", nomeCliente)
            //    .Replace("@EMAIL", emailCliente)
            //    .Replace("@RAZAOSOCIAL", razaoSocialCliente)
            //    .Replace("@URL", url);
        }

        private static void EnviarEmail(string destinatario, string corpo, string subject)
        {
            var mm = new MailMessage { Body = corpo, Subject = subject, From = new MailAddress("contato@adagency.com.br", "Ad.Agency") };
            mm.To.Add(destinatario);
            mm.IsBodyHtml = true;

            var smtp = new SmtpClient
            {
                Host = ObterHost(),
                Port = ObterPorta(),
                EnableSsl = ConfigurationManager.AppSettings["mailEnableSsl"] != null && ConfigurationManager.AppSettings["mailEnableSsl"].ToString(CultureInfo.InvariantCulture).Equals("true") ? true : false,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mailLogin"].ToString(CultureInfo.InvariantCulture), ConfigurationManager.AppSettings["mailPassword"].ToString(CultureInfo.InvariantCulture))
            };

            new Thread(t => smtp.Send(mm)).Start();
        }

        private static string ObterHost()
        {
            return ConfigurationManager.AppSettings["mailHost"].ToString(CultureInfo.InvariantCulture);
        }

        private static int ObterPorta()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["mailPort"].ToString(CultureInfo.InvariantCulture));
        }
    }
}