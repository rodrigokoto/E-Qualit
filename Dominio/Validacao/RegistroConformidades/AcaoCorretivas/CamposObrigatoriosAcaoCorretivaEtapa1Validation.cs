using Dominio.Entidade;
using Dominio.Enumerado;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.AcaoCorretivas
{
    public class CamposObrigatoriosAcaoCorretivaEtapa1Validation : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosAcaoCorretivaEtapa1Validation()
        {
            //RuleFor(x => x.DescricaoRegistro)
            //    .NotEmpty().WithMessage(Traducao.Resource.AcaoCorretivas_msg_erro_required_DescricaoRegistro)
            //    .Length(4, 2000).WithMessage(Traducao.Resource.AcaoCorretiva_msg_erro_min_e_max_DescricaoRegistro);

            RuleFor(x => x.IdEmissor)
                .NotEmpty().WithMessage(Traducao.Resource.AcaoCorretiva_msg_erro_required_IdEmissor);

            RuleFor(x => x.TipoRegistro)
                .Must(x => x.Equals("ac"))
                .WithMessage(Traducao.Resource.MsgCamposObrigatoriosAC);

            RuleFor(x => x.StatusEtapa)
                .Must(x => x.Equals((byte)EtapasRegistroConformidade.AcaoImediata))
                .WithMessage(Traducao.Resource.NaoConformidade_Index_lbl_StatusEtapa);

            RuleFor(x => x.IdSite)
                .NotEmpty().WithMessage(Traducao.Resource.AcaoCorretiva_msg_erro_required_IdSite);

            RuleFor(x => x.IdProcesso)
                .NotEmpty().WithMessage(Traducao.Resource.AcaoCorretiva_msg_erro_required_IdProcesso);

            //RuleFor(x => x.IdResponsavelIniciar)
            //    .NotEmpty().WithMessage("Tradução para responsavel por acao obrigatório");

            RuleFor(x => x.IdResponsavelEtapa)
                .NotEmpty().WithMessage(Traducao.Resource.MsgTraducaoResponsavel);

            RuleFor(x => x.IdResponsavelInicarAcaoImediata)
                .NotEmpty().WithMessage(Traducao.Resource.MsgResponsavelAcaoImediataNeeded);

            RuleFor(x=> x.IdUsuarioIncluiu)
                .NotEmpty().WithMessage(Traducao.Resource.MsgUsuarioIncluido);

            RuleFor(x=> x.IdUsuarioAlterou)
                .NotEmpty().WithMessage(Traducao.Resource.MsgUsuarioAlterado);

        }
    }
}
