using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.GestaoDeRiscos
{
    public class CamposObrigatoriosGestaoDeRiscoEtapa2Validation : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosGestaoDeRiscoEtapa2Validation()
        {
            RuleFor(x => x.AcoesImediatas)
              .Must(x => x.Count > 0).WithMessage(Traducao.Resource.MsgAcaoImediataNecessaria);


            RuleFor(x => x.IdResponsavelReverificador)
                .NotNull().WithMessage(Traducao.Resource.MsgCampoReverificador)
                .Must(x => x > 0).WithMessage(Traducao.Resource.MsgCampoReverificador);
        }
    }
}
