using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.Temas
{
    public class CampoDescricaoObrigatorioValidation : AbstractValidator<AnaliseCriticaTema>
    {
        public CampoDescricaoObrigatorioValidation()
        {
            RuleFor(x => x.ControladorCategoria.Descricao)
               .NotEmpty().WithMessage(Traducao.Resource.MsgDescricaoNaoInformada)
               .NotNull().WithMessage(Traducao.Resource.MsgDescricaoNaoInformada);
        }
    }
}
