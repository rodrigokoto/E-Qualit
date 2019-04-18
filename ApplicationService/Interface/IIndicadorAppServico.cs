using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IIndicadorAppServico : IBaseServico<Indicador>
    {
        IEnumerable<Indicador> IndicadoresPorProcessoESite(int idSite, int? idProcesso = null);
        IEnumerable<Indicador> IndicadoresPorProcessoESiteEIndicador(int id, int ? idProcesso = null);
        void Atualizar(Indicador indicador);
    }
}
