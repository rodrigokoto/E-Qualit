using Dominio.Entidade;
using FluentValidation;
using System;

namespace Dominio.Validacao.ControleImpressoes
{
    public class CamposObrigatoriosValidation : AbstractValidator<ControleImpressao>
    {
        public CamposObrigatoriosValidation()
        {
            RuleFor(controle => controle.IdFuncionalidade)
                .GreaterThan(0);

            RuleFor(controle => controle.CodigoReferencia)
                .NotNull()
                .NotEmpty();

            RuleFor(controle => controle.CopiaControlada)
                .NotNull();

            RuleFor(controle => controle.IdUsuarioDestino)
                .NotNull();

            RuleFor(controle => controle.DataImpressao)
                .NotNull()
                .GreaterThan(DateTime.MinValue);

            RuleFor(controle => controle.IdUsuarioIncluiu)
                .NotNull();

            RuleFor(controle => controle.DataInclusao)
                .NotNull()
                .GreaterThan(DateTime.MinValue);
        }
    }
}
