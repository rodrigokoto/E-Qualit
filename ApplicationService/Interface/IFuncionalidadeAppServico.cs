using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IFuncionalidadeAppServico : IBaseServico<Funcionalidade>
    {
        List<Funcionalidade> CriarFuncionalidadesPorSiteModulos(List<SiteFuncionalidade> sites);
    }
}
