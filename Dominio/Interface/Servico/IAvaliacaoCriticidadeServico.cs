using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IAvaliacaoCriticidadeServico
    {
        void ValidaCampos(AvaliacaoCriticidade avaliacaoCriticidade, ref List<string> erros);

    }
}
