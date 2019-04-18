using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.Indicadores
{
    public class PlanoDeVooAptoParaCadastroValidation : AbstractValidator<PlanoVoo>
    {
        public PlanoDeVooAptoParaCadastroValidation()
        {
            RuleFor(indicador => indicador.Realizado)
              .NotEmpty().WithMessage(Traducao.Resource.MsgValorInformado);
        }
    }
}
