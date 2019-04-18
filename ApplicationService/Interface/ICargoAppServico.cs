using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface ICargoAppServico : IBaseServico<Cargo>
    {
        List<Cargo> ObtemCargosPorSiteEProcesso(int idSite, int idProcesso);
        List<Cargo> ObtemCargosPorSite(int idSite);
        bool AtivarInativar(int id);
    }
}
