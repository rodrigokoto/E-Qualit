using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.CriteriosQualificacao
{
    public class AptoParaQualificarCriterioQualificacaoSemControleVencimento : AbstractValidator<AvaliaCriterioQualificacao>
    {
        public AptoParaQualificarCriterioQualificacaoSemControleVencimento()
        {
            //RuleFor(x => x.ArquivoEvidencia)
            //    .NotEmpty()
            //    .WithMessage("Obrigatório");

            RuleFor(x => x.Aprovado)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgCampoAprovado);

            //RuleFor(x => x.Observacoes)
            //    .NotEmpty()
            //    .WithMessage(Traducao.Resource.MsgObservacoesObrigatorias);
                        
            RuleFor(x => x.IdResponsavelPorQualificar)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgCampoQualificar);

            RuleFor(x => x.DtQualificacaoVencimento)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgCampoControleVencimento);

        }
    }
}
