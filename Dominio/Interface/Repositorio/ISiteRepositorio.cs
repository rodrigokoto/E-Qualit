using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface ISiteRepositorio : IBaseRepositorio<Site>
    {
        IEnumerable<Site> ListarSitesPorCliente(int idCliente);

        bool Excluir(int id);
    }
}
