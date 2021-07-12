using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.GestaoDeRiscos
{
    public class CamposObrigatoriosGestaoDeRiscoReverificacao : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosGestaoDeRiscoReverificacao()
        {
            RuleFor(x => x.Parecer)
                .NotNull().WithMessage("Parecer é obrigatório");
        }
    }
}
