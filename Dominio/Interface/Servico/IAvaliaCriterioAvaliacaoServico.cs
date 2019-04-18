using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IAvaliaCriterioAvaliacaoServico
    {
        void Salvar(List<AvaliaCriterioAvaliacao> listaCriterioAvaliacao);
        void ValidaAvaliaCriterioAvaliacao(AvaliaCriterioAvaliacao avaliaCriterioAvaliacao, ref List<string> erros);
    }
}
