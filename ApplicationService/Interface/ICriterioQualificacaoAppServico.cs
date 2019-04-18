using Dominio.Entidade;

namespace ApplicationService.Interface
{
    public interface ICriterioQualificacaoAppServico: IBaseServico<CriterioQualificacao>
    {
        void SalvarQualificacao(CriterioQualificacao CriterioQualificacao);
    }
}
