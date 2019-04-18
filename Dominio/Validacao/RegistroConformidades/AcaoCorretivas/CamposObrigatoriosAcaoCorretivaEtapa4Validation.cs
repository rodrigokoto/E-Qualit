using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.AcaoCorretivas
{
    public class CamposObrigatoriosAcaoCorretivaEtapa4Validation : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosAcaoCorretivaEtapa4Validation()
        {
            RuleFor(x => x.AcoesImediatas)
              .Must(x => x.Count > 0)
              .WithMessage(Traducao.Resource.MsgAcaoImediataNecessaria);

            RuleFor(x => x.IdResponsavelReverificador)
                .NotEmpty().WithMessage(Traducao.Resource.MsgCampoReverificador);
        }
    }
}
