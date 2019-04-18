using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.GestaoDeRiscos
{
    public class CamposObrigatoriosParaRegistro : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosParaRegistro()
        {
            RuleFor(x => x.DescricaoRegistro)
            .NotEmpty().WithMessage("Traduçao para Não não pode está Vazio")
            .Length(4, 2000).WithMessage("Traduçao para limite de tamanho");

            //RuleFor(x => x.NumeroSequencial);
            RuleFor(x => x.IdEmissor)
           .NotNull().WithMessage("Traduçao para Não não pode está Nulo")
           .NotEmpty().WithMessage("Traduçao para Não não pode está Vazio");
            //RuleFor(x => x.DtEmissao);
        }
    }
}
