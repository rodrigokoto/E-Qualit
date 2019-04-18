using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.NaoConformidades
{
    public class AptoParaCriarNovaAcaoCorretiva : AbstractValidator<RegistroConformidade>
    {
        public AptoParaCriarNovaAcaoCorretiva()
        {
            RuleFor(x => x.DescricaoAnaliseCausa)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_DescricaoAnaliseCausa);
            RuleFor(x => x.IdResponsavelInicarAcaoImediata)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_PrimeiraEtapa_lbl_IdResponsavelInicarAcaoImediata);
            RuleFor(x => x.NecessitaAcaoCorretiva.Value)
                .NotEqual(false).WithMessage(Traducao.Resource.MsgNecessitaAcao);

        }
    }
}
