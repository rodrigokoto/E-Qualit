using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.Cargos
{
    public class AptoParaCadastroValidation : AbstractValidator<Cargo>
    {
        public AptoParaCadastroValidation()
        {
            RuleFor(x => x.IdSite)
                .NotNull().WithMessage(Traducao.Resource.MsgErroIdSite)
                .GreaterThan(0).WithMessage(Traducao.Resource.MsgErroIdSiteMaior);

            RuleFor(x => x.CargoProcessos)
                .NotEmpty().WithMessage(Traducao.Resource.MsgErroFuncaoSelecionada);

            RuleFor(x => x.NmNome)
                .NotEmpty().WithMessage(Traducao.Resource.Produto_msg_erro_required_Nome);


        }
    }
}
