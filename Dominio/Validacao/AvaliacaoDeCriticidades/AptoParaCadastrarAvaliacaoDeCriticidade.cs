using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.AvaliacaoDeCriticidades
{
    public class AptoParaCadastrarAvaliacaoDeCriticidade : AbstractValidator<AvaliacaoCriticidade>
    {
        public AptoParaCadastrarAvaliacaoDeCriticidade()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage(Traducao.Resource.AvaliacaoDeCriticidade_msg_erro_required_Titulo);

            RuleFor(x => x.IdProduto)
                .NotEmpty().WithMessage(Traducao.Resource.AvaliacaoDeCriticidade_msg_erro_required_IdProduto);
        }
    }
}
