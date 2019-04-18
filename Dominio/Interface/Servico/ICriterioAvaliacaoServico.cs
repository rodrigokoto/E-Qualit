using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface ICriterioAvaliacaoServico
    {
        void ValidaCamposCadastro(CriterioAvaliacao criterioAvaliacao, ref List<string> erros);

        void ValidaCamposQualifica(CriterioAvaliacao criterioAvaliacao, ref List<string> erros);
    }
}
