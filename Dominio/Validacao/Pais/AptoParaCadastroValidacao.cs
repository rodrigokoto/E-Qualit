using Dominio.Entidade;
using FluentValidation;


namespace Dominio.Validacao.Pais
{
    public class AptoParaCadastroValidacao : AbstractValidator<Pai>
    {
        public AptoParaCadastroValidacao()
        {
            RuleFor(x => x.Ano)
             .NotEmpty().WithMessage(Traducao.Resource.MsgAnoValido)
             .NotNull().WithMessage(Traducao.Resource.MsgAnoValido);

            RuleFor(x => x.IdGestor)
             .NotEmpty().WithMessage(Traducao.Resource.MsgGestor)
             .NotNull().WithMessage(Traducao.Resource.MsgGestor);

            RuleFor(x => x.IdSite)
             .NotEmpty().WithMessage(Traducao.Resource.MsgIdSite)
             .NotNull().WithMessage(Traducao.Resource.MsgIdSite);
        }
     
    }
}
