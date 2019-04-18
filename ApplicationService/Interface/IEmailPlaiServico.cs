using Dominio.Entidade;

namespace ApplicationService.Interface
{
    public interface IEmailPlaiServico
    {
        void EnviarNotificacao(Plai plai);
    }
}
