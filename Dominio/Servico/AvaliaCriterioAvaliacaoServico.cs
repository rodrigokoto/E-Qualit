using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.AvaliacoesCriterioAvaliacao;
using System.Collections.Generic;

namespace Dominio.Servico
{
    public class AvaliaCriterioAvaliacaoServico : IAvaliaCriterioAvaliacaoServico
    {

        private readonly IAvaliaCriterioAvaliacaoRepositorio _avaliaCriterioAvaliacaoRepositorio;
        private readonly IHistoricoCriterioAvaliacaoRepositorio _historicoCriterioAvaliacaoRepositorio;

        public AvaliaCriterioAvaliacaoServico(IAvaliaCriterioAvaliacaoRepositorio avaliaCriterioAvaliacaoRepositorio,
            IHistoricoCriterioAvaliacaoRepositorio historicoCriterioAvaliacaoRepositorio) 
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

        public void ValidaAvaliaCriterioAvaliacao(AvaliaCriterioAvaliacao avaliaCriterioAvaliacao, ref List<string> erros)
        {
            var validacampos = new AptoParaAvaliaCriterioAvaliacao().Validate(avaliaCriterioAvaliacao);

            if (!validacampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacampos.Errors));
            }
        }

    }
}
