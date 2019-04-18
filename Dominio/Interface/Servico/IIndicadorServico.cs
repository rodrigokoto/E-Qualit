using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IIndicadorServico
    {
        IEnumerable<Indicador> IndicadoresPorProcessoESite(int idProcesso, int idSite);
        IEnumerable<Indicador> IndicadoresPorProcessoESiteEIndicador(int idProcesso, int id);
        void Valido(Indicador indicador, ref List<string> erros);
    }
}
