using Dominio.Entidade;

namespace Dominio.Interface.Servico
{
    public interface IEmailPlaiServico
    {
        void EnviarNotificacao(Plai plai);

    }
}
