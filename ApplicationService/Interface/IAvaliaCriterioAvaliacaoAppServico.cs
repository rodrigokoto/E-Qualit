using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IAvaliaCriterioAvaliacaoAppServico: IBaseServico<AvaliaCriterioAvaliacao>
    {
        void Salvar(List<AvaliaCriterioAvaliacao> listaCriterioAvaliacao);
    }
}
