using Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Interface.Repositorio
{
    public interface ISiteAnexoRepositorio : IBaseRepositorio<SiteAnexo>
    {
        IEnumerable<SiteAnexo> ListarSiteAnexoPorSite(int idSite);
    }
}
