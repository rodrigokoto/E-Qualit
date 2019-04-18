using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface ICriterioQualificacaoServico
    {
        void ValidaCampos(CriterioQualificacao criterioQualificacao, ref List<string> erros);

        void ValidaCamposQualificacao(AvaliaCriterioQualificacao criterioQualificacao, ref List<string> erros);
        void ValidaCamposQualificacaoSemControleVencimento(AvaliaCriterioQualificacao criterioQualificacao, ref List<string> erros);

        void SalvarQualificacao(CriterioQualificacao criterioQualificacao);
    }
}
