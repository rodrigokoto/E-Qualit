using System.Collections.Generic;

namespace Dominio.Entidade
{
    public partial class NotificacaoSmtp
    {
        public int IdSmptNotificacao { get; set; }
        public string DsSmtp { get; set; }
        public int NuPorta { get; set; }
        public string CdUsuario { get; set; }
        public string CdSenha { get; set; }
        public string NmNome { get; set; }
        public bool FlAtivo { get; set; }

        public virtual ICollection<NotificacaoMensagem> NotificacaoMensagens { get; set; }
    }
}
