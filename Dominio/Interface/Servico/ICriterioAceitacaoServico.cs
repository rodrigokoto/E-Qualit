using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface ICriterioAceitacaoServico 
    {
        void AlterarEstadoParaDeletado(CriterioAceitacao obj);
        void Remove(List<CriterioAceitacao> criteriosAceitacao);
    }
}
