using Dominio.Entidade;
using Dominio.Enumerado;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.GestaoDeRiscos
{
    public class CamposObrigatoriosGestaoDeRiscoEtapa1Validation : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosGestaoDeRiscoEtapa1Validation()
        {
            RuleFor(x => x.DescricaoRegistro)                
                .Length(4, 1000).WithMessage(Traducao.Resource.GestaoDeRisco_msg_erro_min_e_max_DescricaoRegistro);

            RuleFor(x => x.StatusEtapa)
                .Must(x => x.Equals((byte)EtapasRegistroConformidade.AcaoImediata))
                .When(x=>x.IdResponsavelEtapa != null)
                .WithMessage(Traducao.Resource.StatusInvalido);

            RuleFor(x => x.IdEmissor)
                .NotEmpty().WithMessage(Traducao.Resource.GestaoDeRisco_msg_erro_required_IdEmissor);

            //RuleFor(x => x.IdProcesso)
            //    .NotEmpty().WithMessage(Traducao.Resource.GestaoDeRisco_msg_erro_required_IdProcesso);

            RuleFor(x => x.IdSite)
                .NotEmpty().WithMessage(Traducao.Resource.GestaoDeRisco_msg_erro_required_IdSite);

            //RuleFor(x => x.CriticidadeGestaoDeRisco)
            //    .NotEmpty().WithMessage("Cor da criticidade é obrigatório.");

            RuleFor(x => x.EProcedente)
                .NotEmpty().WithMessage(Traducao.Resource.MsgNecessitaAcao);

            RuleFor(x => x.TipoRegistro)
                 .Must(x => x.Equals("gr"))
                 .WithMessage(Traducao.Resource.TraducaoTipoDeRegistro);
        }
    }
}
