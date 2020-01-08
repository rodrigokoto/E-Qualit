using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Collections.Generic;

namespace ApplicationService.Servico
{
    public class AvaliaCriterioAvaliacaoAppServico : BaseServico<AvaliaCriterioAvaliacao>, IAvaliaCriterioAvaliacaoAppServico
    {

        private readonly IAvaliaCriterioAvaliacaoRepositorio _avaliaCriterioAvaliacaoRepositorio;
        private readonly IHistoricoCriterioAvaliacaoRepositorio _historicoCriterioAvaliacaoRepositorio;

        public AvaliaCriterioAvaliacaoAppServico(IAvaliaCriterioAvaliacaoRepositorio avaliaCriterioAvaliacaoRepositorio,
            IHistoricoCriterioAvaliacaoRepositorio historicoCriterioAvaliacaoRepositorio) : base(avaliaCriterioAvaliacaoRepositorio)
        {
            _avaliaCriterioAvaliacaoRepositorio = avaliaCriterioAvaliacaoRepositorio;
            _historicoCriterioAvaliacaoRepositorio = historicoCriterioAvaliacaoRepositorio;
        }

        public void Salvar(List<AvaliaCriterioAvaliacao> listavaliacoesCriterioAvaliacao)
        {
            listavaliacoesCriterioAvaliacao.ForEach(avaliaCriterioAvaliacao =>
            {

                _avaliaCriterioAvaliacaoRepositorio.Add(avaliaCriterioAvaliacao);

                if (avaliaCriterioAvaliacao.IdCriterioAvaliacao != null)
                {
                    var historico = new HistoricoCriterioAvaliacao
                    {
                        IdCriterioAvaliacao = (int)avaliaCriterioAvaliacao.IdCriterioAvaliacao,
                        Nota = avaliaCriterioAvaliacao.NotaAvaliacao.Value
                    };

                    _historicoCriterioAvaliacaoRepositorio.Add(historico);
                }

            });
        }

    }
}
