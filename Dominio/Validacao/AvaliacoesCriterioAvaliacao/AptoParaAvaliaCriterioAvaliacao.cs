using Dominio.Entidade;
using FluentValidation;
using System;

namespace Dominio.Validacao.AvaliacoesCriterioAvaliacao
{
    public class AptoParaAvaliaCriterioAvaliacao: AbstractValidator<AvaliaCriterioAvaliacao>
    {
        public AptoParaAvaliaCriterioAvaliacao()
        {
            RuleFor(x => x.IdCriterioAvaliacao)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgErroCriterioAvaliacao);

            RuleFor(x => x.DtProximaAvaliacao)                
                .NotEqual(default(DateTime))
                .WithMessage(Traducao.Resource.MsgDtProximaAvaliacaoIsRequired)
                .NotNull()
                .WithMessage(Traducao.Resource.MsgDtProximaAvaliacaoIsRequired);
                        
            RuleFor(x => x.NotaAvaliacao.Value)
                .LessThanOrEqualTo(100)
                .WithMessage(Traducao.Resource.MsgErroNotaMaxima)
                .GreaterThanOrEqualTo(0)
                .WithMessage(Traducao.Resource.MsgErroNotaMinima)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgErroCampoNota);
        }
    }
}
