using System;

namespace Dominio.Entidade
{
    public partial class NotificacaoMensagem
    {
        public NotificacaoMensagem()
        {
        }
        public NotificacaoMensagem(int IdSite, string EmailDestinatario,
            string EmailRemetente, string Mensagem, string Assunto, 
            DateTime DtEnvio, int IdSmtpNotificacao)
        {
            this.IdSite = IdSite;
            this.DtEnvio = DtEnvio;
            this.IdSmtpNotificacao = IdNotificacaoMenssagem;

            DsAssunto = Assunto;
            DsMensagem = Mensagem;
            NmEmailNome = EmailRemetente;
            NmEmailPara = EmailDestinatario;
            DtCadastro = DateTime.Now;
            FlEnviada = false;

        }
        public int IdNotificacaoMenssagem { get; set; }
        public int IdSite { get; set; }
        public string NmEmailPara { get; set; }
        public string NmEmailNome { get; set; }
        public string DsMensagem { get; set; }
        public DateTime DtCadastro { get; set; }
        public DateTime? DtEnvio { get; set; }
        public int? IdSmtpNotificacao { get; set; }
        public bool? FlEnviada { get; set; }
        public string DsAssunto { get; set; }

        public virtual Site Site { get; set; }
        public virtual NotificacaoSmtp NotificacaoSmtp { get; set; }
    }
}
