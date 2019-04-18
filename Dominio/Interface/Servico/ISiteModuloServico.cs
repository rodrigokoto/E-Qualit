using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface ISiteModuloServico
    {
        List<SiteFuncionalidade> RetirarReferenciaCircularDaLista(IEnumerable<SiteFuncionalidade> sitesFuncionalidades);
    }
}
