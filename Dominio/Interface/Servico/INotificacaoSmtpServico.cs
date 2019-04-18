using Dominio.Entidade;

namespace Dominio.Interface.Servico
{
    public interface INotificacaoSmtpServico 
    {
        NotificacaoSmtp ObterSmtpAtivo();
    }
}
