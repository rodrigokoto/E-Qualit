using Entidade;
using System;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Threading;
using Traducao;

namespace Servico
{
    public class EmailServico
    {
        private string _urlBase = string.Empty;

        public void EnviarSenha(CtrlUsuario usuario)
        {
            //TODO Como não está usando injeção de dependencia não consigo obter a URL
            //var site = new Util();
            //var url = site.ObterUrlBase("/Content/assets/images/email/esqueciSenha");
            var url = "www.teste.com.br";
            var template = Template(usuario.NmCompleto, usuario.CdIdentificacao, usuario.CdSenha, url);

            EnviarEmail(usuario.CdIdentificacao, template, usuario.NmCompleto + ", confira sua senha de acesso ao Equality.");
        }

        private static string Template(string nome, string login, string senha, string url)
        {
            return Resource.EnviarSenha
                .Replace("@NOME", nome)
                .Replace("@LOGIN", login)
                .Replace("@SENHA", senha)
                .Replace("@URL", url);
        }

        private static void EnviarEmail(string destinatario, string corpo, string subject)
        {
            var mm = new MailMessage { Body = corpo, Subject = subject, From = new MailAddress("contato@g2it.com.br", "G2 IT") };
            mm.To.Add(destinatario);
            mm.IsBodyHtml = true;

            var smtp = new SmtpClient
            {
                Host = ObterHost(),
                Port = ObterPorta(),
                EnableSsl = ConfigurationManager.AppSettings["mailEnableSsl"] != null && ConfigurationManager.AppSettings["mailEnableSsl"].ToString(CultureInfo.InvariantCulture).Equals("true") ? true : false,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTPUser"].ToString(CultureInfo.InvariantCulture), ConfigurationManager.AppSettings["SMTPPassword"].ToString(CultureInfo.InvariantCulture))
            };

            new Thread(t => smtp.Send(mm)).Start();
        }

        private static string ObterHost()
        {
            return ConfigurationManager.AppSettings["SMTPServer"].ToString(CultureInfo.InvariantCulture);
        }

        private static int ObterPorta()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString(CultureInfo.InvariantCulture));
        }
    }
}
