using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;

namespace ApplicationService.Servico
{
    public class CriterioQualificacaoAppServico : BaseServico<CriterioQualificacao>, ICriterioQualificacaoAppServico
    {
        private readonly ICriterioQualificacaoRepositorio _criterioQualificacaoRepositorio;

        public CriterioQualificacaoAppServico(ICriterioQualificacaoRepositorio criterioQualificacaoRepositorio) : base(criterioQualificacaoRepositorio)
        {
            _criterioQualificacaoRepositorio = criterioQualificacaoRepositorio;
        }

        public void SalvarQualificacao(CriterioQualificacao criterioQualificacao)
        {
            _criterioQualificacaoRepositorio.Update(criterioQualificacao);
        }

    }
}
