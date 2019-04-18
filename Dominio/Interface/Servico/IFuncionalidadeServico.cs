using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IFuncionalidadeServico
    {
        List<Funcionalidade> CriarFuncionalidadesPorSiteModulos(List<SiteFuncionalidade> sites);
    }
}
