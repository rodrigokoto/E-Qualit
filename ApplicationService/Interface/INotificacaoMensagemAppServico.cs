using Dominio.Entidade;

namespace ApplicationService.Interface
{
    public interface INotificacaoMensagemAppServico : IBaseServico<NotificacaoMensagem>
    {
        void GeraFilaEmail();
    }
}
