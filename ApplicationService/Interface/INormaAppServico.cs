using Dominio.Entidade;

namespace ApplicationService.Interface
{
    public interface INormaAppServico : IBaseServico<Norma>
    {

        bool AtivarInativar(int id);

    }
}
