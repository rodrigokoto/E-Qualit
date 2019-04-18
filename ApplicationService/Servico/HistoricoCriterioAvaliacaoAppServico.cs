using Dominio.Entidade;
using ApplicationService.Interface;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class HistoricoCriterioAvaliacaoAppServico : BaseServico<HistoricoCriterioAvaliacao>, IHistoricoCriterioAvaliacaoAppServico
    {
        private readonly IHistoricoCriterioAvaliacaoRepositorio _historicoCriterioAvaliacaoRepositorio;

        public HistoricoCriterioAvaliacaoAppServico(IHistoricoCriterioAvaliacaoRepositorio historicoCriterioAvaliacaoRepositorio) : base(historicoCriterioAvaliacaoRepositorio)
        {
            _historicoCriterioAvaliacaoRepositorio = historicoCriterioAvaliacaoRepositorio;
        }
    }
}
