using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface ICriterioAceitacaoAppServico : IBaseServico<CriterioAceitacao>
    {
        void AlterarEstadoParaDeletado(CriterioAceitacao obj);
        void Remove(List<CriterioAceitacao> criteriosAceitacao);
    }
}
