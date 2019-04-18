using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface ISubModuloRepositorio : IBaseRepositorio<SubModulo>
    {
        IEnumerable<SubModulo> ListarSubModuloPorSite(int idSite);
    }
}
