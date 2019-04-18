using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface ISiteModuloRepositorio : IBaseRepositorio<SiteFuncionalidade>
    {
        List<SiteFuncionalidade> ListarSiteModuloPorSite(int idSite);
    }
}
