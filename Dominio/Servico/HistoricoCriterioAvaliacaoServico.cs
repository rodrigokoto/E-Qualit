using Dominio.Interface.Servico;
using Dominio.Interface.Repositorio;

namespace Dominio.Servico
{
    public class HistoricoCriterioAvaliacaoServico : IHistoricoCriterioAvaliacaoServico
    {
        private readonly IHistoricoCriterioAvaliacaoRepositorio _historicoCriterioAvaliacaoRepositorio;

        public HistoricoCriterioAvaliacaoServico(IHistoricoCriterioAvaliacaoRepositorio historicoCriterioAvaliacaoRepositorio)
        {
            _historicoCriterioAvaliacaoRepositorio = historicoCriterioAvaliacaoRepositorio;
        }
    }
}
