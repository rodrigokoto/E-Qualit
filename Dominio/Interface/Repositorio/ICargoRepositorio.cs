using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface ICargoRepositorio : IBaseRepositorio<Cargo>
    {
        IEnumerable<Cargo> ListarCargosPorSite(int idSite);

        bool Excluir(int id);
    }
}
