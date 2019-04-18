using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.AnaliseCriticas
{
    public class AptoParaCadastroValidation : AbstractValidator<AnaliseCritica>
    {
        public AptoParaCadastroValidation()
        {
            RuleFor(x => x.Ata)
                   .NotEmpty().WithMessage(Traducao.Resource.MsgErroAtaObrigatoria)
                   .NotNull().WithMessage(Traducao.Resource.MsgErroAtaObrigatoria)
                   .Length(0, 100).WithMessage(Traducao.Resource.MsgAtaMax100);

            RuleFor(x => x.DataCriacao)
                 .NotEmpty().WithMessage(Traducao.Resource.MsgErroDataCriacao)
                 .NotNull().WithMessage(Traducao.Resource.MsgErroDataCriacao);

            RuleFor(x => x.DataProximaAnalise)
                 .NotEmpty().WithMessage(Traducao.Resource.MsgDataAnalise)
                 .NotNull().WithMessage(Traducao.Resource.MsgDataAnalise);
        }
    }
}
