using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.NaoConformidades
{
    public class AptoParaSalvarSegundaEtapaResponsavelPorEtapaValidacao : AbstractValidator<RegistroConformidade>
    {
        public AptoParaSalvarSegundaEtapaResponsavelPorEtapaValidacao()
        {
            RuleFor(x => x.IdUsuarioAlterou)
                .NotEqual(x=>x.IdResponsavelEtapa.Value).WithMessage(Traducao.Resource.MsgUsuarioResponsavel);
        }
    }
}
