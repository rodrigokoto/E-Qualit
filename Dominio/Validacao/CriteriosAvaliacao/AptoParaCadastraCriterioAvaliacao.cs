using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.CriteriosAvaliacao
{
    public class AptoParaCadastraCriterioAvaliacao : AbstractValidator<CriterioAvaliacao>
    {
        public AptoParaCadastraCriterioAvaliacao()
        {
            RuleFor(x => x.Titulo)
               .NotEmpty().WithMessage(Traducao.Resource.AvaliacaoDeCriticidade_msg_erro_required_Titulo);

            RuleFor(x => x.IdProduto)
                .NotEmpty().WithMessage(Traducao.Resource.AvaliacaoDeCriticidade_msg_erro_required_IdProduto);

        }
    }
}
