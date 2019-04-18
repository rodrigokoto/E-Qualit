using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;

namespace ApplicationService.Servico
{
    public class AvaliaCriterioQualificacaoAppServico : BaseServico<AvaliaCriterioQualificacao>, IAvaliaCriterioQualificacaoAppServico
    {
        private readonly IAvaliaCriterioQualificacaoRepositorio _AvaliaCriterioQualificacaoRepositorio;

        public AvaliaCriterioQualificacaoAppServico(IAvaliaCriterioQualificacaoRepositorio AvaliaCriterioQualificacaoRepositorio) : base(AvaliaCriterioQualificacaoRepositorio)
        {
            _AvaliaCriterioQualificacaoRepositorio = AvaliaCriterioQualificacaoRepositorio;
        }

        public void SalvarQualificacao(AvaliaCriterioQualificacao AvaliaCriterioQualificacao)
        {
            _AvaliaCriterioQualificacaoRepositorio.Update(AvaliaCriterioQualificacao);
        }

    }
}
