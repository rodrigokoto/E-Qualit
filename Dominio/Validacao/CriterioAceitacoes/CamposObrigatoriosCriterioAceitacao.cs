using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.CriterioAceitacoes
{
    public class CamposObrigatoriosCriterioAceitacao : AbstractValidator<CriterioAceitacao>
    {
        public CamposObrigatoriosCriterioAceitacao()
        {
            RuleFor(criterio => criterio.Incerteza)
                .NotNull().WithMessage(Traducao.Resource.MsgCampoPreenchido);

            RuleFor(criterio => criterio.Erro)
            .NotNull().WithMessage(Traducao.Resource.MsgCampoPreenchido);
        }
    }
}
