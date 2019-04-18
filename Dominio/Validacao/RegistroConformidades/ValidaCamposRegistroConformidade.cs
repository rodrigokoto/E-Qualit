using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades
{
    public abstract class ValidaCamposRegistroConformidade<T> : AbstractValidator<T> where T : RegistroConformidade
    {
        protected void DescricaoRegistroObrigatoria()
        {
            //RuleFor(x => x.DescricaoRegistro)
            //  .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_DescricaoRegistro)
            //  .Length(4, 2000).WithMessage(Traducao.Resource.NaoConformidade_msg_erro_min_e_max_DescricaoRegistro);


        }

        protected void EmissorObrigatorio()
        {

            RuleFor(x => x.IdEmissor)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_IdEmissor);
        }
        protected void UsuarioQueIncluiuObrigatorio()
        {
            RuleFor(x => x.IdUsuarioIncluiu)
               .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_IdUsuarioIncluiu)
               .NotEqual(0).WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_IdUsuarioIncluiu);
        }
        protected void ProcessoObrigatorio()
        {
            RuleFor(x => x.IdProcesso)
               .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_IdProcesso);


        }
        protected void SiteObrigatorio()
        {
            RuleFor(x => x.IdSite)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_IdSite);

        }
        protected void TipoNaoConformidadeObrigatorio()
        {
            RuleFor(x => x.IdTipoNaoConformidade)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_IdTipoNaoConformidade);

        }
        protected void ResponsavelPorDefinirAcaoImediata()
        {
            RuleFor(x => x.IdResponsavelInicarAcaoImediata)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_IdResponsavelIniciarAcaoCorretiva);

        }
        protected void ENaoConformidadeAuditoriaObrigatorio()
        {
            RuleFor(x => x.ENaoConformidadeAuditoria)
              .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_IsNaoConformidadeAuditoria);

        }
        protected void TipoRegistro()
        {
            RuleFor(x => x.TipoRegistro)
                .NotNull().WithMessage(Traducao.Resource.MsgNaoPodeVazio)
                .Must(x => x == "nc").WithMessage(Traducao.Resource.MsgRegistroNaoConformidade);
        }
    }
}
