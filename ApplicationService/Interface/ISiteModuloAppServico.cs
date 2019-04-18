using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface ISiteModuloAppServico : IBaseServico<SiteFuncionalidade>
    {
        List<SiteFuncionalidade> RetirarReferenciaCircularDaLista(IEnumerable<SiteFuncionalidade> sitesFuncionalidades);
        List<SiteFuncionalidade> ObterPorSite(int idSite);
    }
}
