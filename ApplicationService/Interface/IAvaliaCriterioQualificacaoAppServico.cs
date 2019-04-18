using Dominio.Entidade;

namespace ApplicationService.Interface
{
    public interface IAvaliaCriterioQualificacaoAppServico: IBaseServico<AvaliaCriterioQualificacao>
    {
        void SalvarQualificacao(AvaliaCriterioQualificacao AvaliaCriterioQualificacao);
    }
}
