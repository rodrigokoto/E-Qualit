using Dominio.Entidade;
using Dominio.Enumerado;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.GestaoMelhorias
{
    public class CamposObrigatoriosGestaoMelhoriaEtapa1Validation : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosGestaoMelhoriaEtapa1Validation()
        {
            //RuleFor(x => x.DescricaoRegistro)                
            //    .Length(4, 1000).WithMessage(Traducao.Resource.GestaoMelhoria_msg_erro_min_e_max_DescricaoRegistro);

            RuleFor(x => x.StatusEtapa)
                .Must(x => x.Equals((byte)EtapasRegistroConformidade.AcaoImediata))
                .When(x => x.IdResponsavelEtapa != null)
                .WithMessage(Traducao.Resource.StatusInvalido);


            RuleFor(x => x.IdTipoNaoConformidade)
                .NotNull()
                .WithMessage(Traducao.Resource.GestaoMelhoria_msg_required_TipoMelhoria);

            //RuleFor(x => x.IdResponsavelEtapa)
            //    .NotEmpty()
            //    .NotNull()
            //    .WithMessage(Traducao.Resource.GestaoMelhoria_msg_required_IdResponsavelEtapa);

            RuleFor(x => x.DtEmissao)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgDataEmissao);

            RuleFor(x => x.IdProcesso)
                .NotNull()
                .WithMessage(Traducao.Resource.GestaoDeRisco_msg_erro_required_IdProcesso);

            RuleFor(x => x.IdEmissor)
                   .NotNull()
                   .WithMessage(Traducao.Resource.GestaoMelhoria_msg_erro_required_IdEmissor);

            RuleFor(x => x.IdEmissor)
                .NotEmpty().WithMessage(Traducao.Resource.GestaoMelhoria_msg_erro_required_IdEmissor);

            //RuleFor(x => x.IdProcesso)
            //    .NotEmpty().WithMessage(Traducao.Resource.GestaoMelhoria_msg_erro_required_IdProcesso);

            RuleFor(x => x.IdSite)
                .NotEmpty().WithMessage(Traducao.Resource.GestaoMelhoria_msg_erro_required_IdSite);

            //RuleFor(x => x.CriticidadeGestaoMelhoria)
            //    .NotEmpty().WithMessage("Cor da criticidade é obrigatório.");

            RuleFor(x => x.TipoRegistro)
             .Must(x => x.Equals("gm"))
             .WithMessage(Traducao.Resource.TraducaoTipoDeRegistro);

            RuleFor(x => x.DescricaoRegistro)
                .NotEmpty()
                .WithMessage(Traducao.Resource.GestaoMelhoria_msg_erro_required_DescricaoRegistro);

            
        }
    }
}
