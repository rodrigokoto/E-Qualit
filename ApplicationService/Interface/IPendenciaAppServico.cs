using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IPendenciaAppServico
    {
        List<Pendencia> PendeciaPorSite(int idSite , int idCliente);
        List<Pendencia> PendeciaPorUsuario(int idSite, int idCliente , int idUsuario);


    }
}
