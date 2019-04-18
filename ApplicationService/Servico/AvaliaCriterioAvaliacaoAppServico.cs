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
                if (avaliaCriterioAvaliacao.NotaAvaliacao != null)
                {
                    _avaliaCriterioAvaliacaoRepositorio.Add(avaliaCriterioAvaliacao);

                    var historico = new HistoricoCriterioAvaliacao
                    {
                        IdCriterioAvaliacao = avaliaCriterioAvaliacao.IdCriterioAvaliacao,
                        Nota = avaliaCriterioAvaliacao.NotaAvaliacao.Value
                    };

                    _historicoCriterioAvaliacaoRepositorio.Add(historico);
                }

            });
        }

    }
}
