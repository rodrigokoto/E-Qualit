using Dominio.Entidade;

namespace Dominio.Interface.Repositorio
{
    public interface IIndicadorRepostorio : IBaseRepositorio<Indicador>
    {
        void Atualizar(Indicador indicador);
    }
}
