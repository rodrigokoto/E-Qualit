using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface ISiteAppServico : IBaseServico<Site>
    {
        IEnumerable<Site> ObterSitesPorCliente(int idCliente);
        bool AtivarInativar(int id);
    }
}
