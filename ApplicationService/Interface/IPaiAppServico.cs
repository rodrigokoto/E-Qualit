using Dominio.Entidade;

namespace ApplicationService.Interface
{
    public interface IPaiAppServico : IBaseServico<Pai>
    {
        Pai ObterPorAno(int? ano);
    }
}
