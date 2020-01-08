using Dominio.Entidade;
using FluentValidation;
using System;

namespace Dominio.Validacao.AvaliacoesCriterioAvaliacao
{
    public class AptoParaAvaliaCriterioAvaliacao: AbstractValidator<AvaliaCriterioAvaliacao>
    {
        
        public AptoParaAvaliaCriterioAvaliacao()
        {

            RuleFor(x => x.DtProximaAvaliacao)                
                .NotEqual(default(DateTime))
                .WithMessage(Traducao.Resource.MsgDtProximaAvaliacaoIsRequired)
                .NotNull()
                .WithMessage(Traducao.Resource.MsgDtProximaAvaliacaoIsRequired);
        }
    }
}
