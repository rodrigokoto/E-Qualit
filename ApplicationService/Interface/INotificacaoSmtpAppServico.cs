using Dominio.Entidade;

namespace ApplicationService.Interface
{
    public interface INotificacaoSmtpAppServico : IBaseServico<NotificacaoSmtp>
    {
        NotificacaoSmtp ObterSmtpAtivo();
    }
}
