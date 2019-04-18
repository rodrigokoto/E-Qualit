using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IProcessoAppServico : IBaseServico<Processo>
    {
        List<Processo> ListaProcessosPorSite(int site);
        List<Processo> ListaProcessosPorUsuario(int idUsuario);
        List<Processo> ListaProcessosPorSiteComTracking(int idSite);

    }
}
