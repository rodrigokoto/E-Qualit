using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.CriteriosQualificacao
{
    public class AptoParaCadastroCriterioQualificacao: AbstractValidator<CriterioQualificacao>
    {
        public AptoParaCadastroCriterioQualificacao()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage(Traducao.Resource.CriterioQualificacao_msg_requerid_Titulo);

            RuleFor(x => x.IdProduto)
                .NotEmpty().WithMessage(Traducao.Resource.CriterioQualificacao_msg_requerid_IdSite);
                      
            RuleFor(x => x.DtCriacao)
                .NotNull().WithMessage(Traducao.Resource.CriterioQualificacao_msg_requerid_DtVencimento)
                //.Empty().WithMessage(Traducao.Resource.CriterioQualificacao_msg_requerid_DtVencimento)
                .When(x => x.TemControleVencimento == true);

        }
    }
}
