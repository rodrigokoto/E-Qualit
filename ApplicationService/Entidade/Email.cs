using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;

namespace ApplicationService.Entidade
{
    public class Email
    {
        public Email()
        {
            Anexos = new List<string>();
        }
        public string Servidor { get; set; }
        public string De { get; set; }
        public string Para { get; set; }
        public string Copia { get; set; }
        public string Assunto { get; set; }
        public string Conteudo { get; set; }
        public int Porta { get; set; }
        public bool EnableSSL { get; set; }
        public List<string> Anexos { get; set; }

        //Esse codigo foi copiado devida a falta de tempo
        //Me descuple humanidade
        public void Enviar()
        {
            //configuracoes do email
            MailMessage mess = new MailMessage();
            mess.From = new MailAddress(De);

            if (Para != null && Para.Length > 0)
            {
                foreach (string address in Para.Split(';'))
                {
                    mess.To.Add(new MailAddress(address));
                }
            }

            if (Copia != null && Copia.Length > 0)
            {
                foreach (string address in Copia.Split(';'))
                {
                    mess.CC.Add(new MailAddress(address));
                }
            }

            mess.Subject = Assunto;

            //formato do email
            mess.IsBodyHtml = true;

            //corpo do email						
            mess.Body = Conteudo;

            //prioridade
            mess.Priority = MailPriority.High;

            //anexos
            if (Anexos.Count > 0)
            {
                foreach (string path in Anexos)
                {
                    mess.Attachments.Add(new Attachment(path));
                }
            }

            //define servidor SMTP
            SmtpClient emailClient;
            if (Porta == 0)
                emailClient = new SmtpClient(Servidor);
            else
                emailClient = new SmtpClient(Servidor, Porta);
            emailClient.UseDefaultCredentials = false;
            emailClient.Credentials = new System.Net.NetworkCredential(this.De, ConfigurationManager.AppSettings["SMTPPassword"]);

            if(EnableSSL)
            {
                emailClient.EnableSsl = EnableSSL;
            }            

            emailClient.Send(mess);

        }
    }
}
