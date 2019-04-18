using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.Normas
{
    public class AptoParaCadastroValidation : AbstractValidator<Norma>
    {
        public AptoParaCadastroValidation()
        {
            RuleFor(x => x.Codigo)
                 .NotEmpty().WithMessage(Traducao.Resource.MsgCodigoObrigatorio)
                 .NotNull().WithMessage(Traducao.Resource.MsgCodigoObrigatorio)
                 .Length(0, 500).WithMessage(Traducao.Resource.MsgMax500Codigo);

            RuleFor(x => x.Titulo)
                 .NotEmpty().WithMessage(Traducao.Resource.MsgTituloObrigatorio)
                 .NotNull().WithMessage(Traducao.Resource.MsgTituloObrigatorio)
                 .Length(0, 500).WithMessage(Traducao.Resource.MsgMax500Titulo); 
        }
    }
}
