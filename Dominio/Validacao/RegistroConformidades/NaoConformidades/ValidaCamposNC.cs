using Dominio.Entidade;
using Dominio.Enumerado;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.NaoConformidades
{
    public abstract class ValidaCamposNC<T> : AbstractValidator<T> where T : RegistroConformidade
    {
        protected void DescricaoRegistroObrigatoria()
        {
            RuleFor(x => x.DescricaoRegistro)
              .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_DescricaoRegistro)
              .Length(4, 2000).WithMessage(Traducao.Resource.NaoConformidade_msg_erro_min_e_max_DescricaoRegistro);

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
                .NotNull().WithMessage(Traducao.Resource.MsgNaoNulo)
                .Must(x => x == "nc").WithMessage(Traducao.Resource.MsgRegistroNaoConformidade);
        }


        protected void EProcedenteObrigatorioEDeveSerFalso()
        {
            RuleFor(x => x.EProcedente)
                .NotNull().WithMessage(Traducao.Resource.MsgCampoProcedente)
                .NotEqual(true).WithMessage(Traducao.Resource.MsgProcedenteOperacao);

        }
        
        protected void JustificatiVaObrigatorio()
        {
            RuleFor(x => x.DescricaoAcao)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_DescricaoAcao)
                .Length(0, 500).WithMessage(Traducao.Resource.MsgMax500Descricao)
                //.Length(x =< 500).WithMessage(Traducao.Resource.MsgMax500Descricao)
                .When(x => x.EProcedente.Value == false);

        }

        protected void AEtapaDeveSerEncerrada()
        {
            RuleFor(x => x.StatusEtapa)
                .Equal((byte)EtapasRegistroConformidade.Encerrada).WithMessage(Traducao.Resource.MsgDadoEtapaValido);

        }

        protected void EProcedenteObrigatorioEDeveSerTrue()
        {
            RuleFor(x => x.EProcedente)
               .NotNull().WithMessage(Traducao.Resource.MsgCampoProcedente)
               .Equal(true).WithMessage(Traducao.Resource.MsgProcedenteOprecaoSim);
        }

        protected void DeveconterAcaoImediata()
        {
            RuleFor(x => x.AcoesImediatas)
                .Must(x => x.Count > 0).WithMessage(Traducao.Resource.MsgAcaoImediataNecessaria);

        }

        protected void AEtapaDeveSerImplementacao()
        {
            RuleFor(x => x.StatusEtapa)
                .Equal((byte)EtapasRegistroConformidade.Implementacao).WithMessage(Traducao.Resource.MsgDadoEtapaValido);
        }

        protected void NecessitaAcaoCorretivaObrigatorio()
        {
            RuleFor(x => x.NecessitaAcaoCorretiva)
                .NotNull().WithMessage(Traducao.Resource.MsgCampoNecessitaAcaoCorrecao)
                .When(x => x.EProcedente == true);
        }

        protected void DescricaoAnaliseCausaOgrigatoria()
        {
            RuleFor(x => x.DescricaoAnaliseCausa)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_DescricaoAnaliseCausa)
                .Length(4, 2000).WithMessage(Traducao.Resource.NaoConformidade_msg_erro_min_e_max_DescricaoAnaliseCausa)
                .When(x => x.NecessitaAcaoCorretiva == true);
        }

        protected void ResponsavelPorIniciarTratativaAcaoCorretivaObrigatorio()
        {
            RuleFor(x => x.IdResponsavelPorIniciarTratativaAcaoCorretiva)
                .NotNull().WithMessage(Traducao.Resource.MsgCampoReverificador)
                .Must(x => x > 0).WithMessage(Traducao.Resource.MsgCampoReverificador)
                .When(x => x.NecessitaAcaoCorretiva == true);

        }

        protected void ResponsavelReverificadorObrigatorio()
        {
            RuleFor(x => x.IdResponsavelReverificador)
                .NotNull().WithMessage(Traducao.Resource.MsgCampoReverificador)
                .Must(x => x > 0).WithMessage(Traducao.Resource.MsgCampoReverificador)
                .When(x => x.ECorrecao == true);
        }

        protected void ECorrecaoObrigatorio()
        {
            RuleFor(x => x.ECorrecao)
                .NotNull().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_ECorrecao)
                .When(x => x.EProcedente == true);
        }

    }
}
