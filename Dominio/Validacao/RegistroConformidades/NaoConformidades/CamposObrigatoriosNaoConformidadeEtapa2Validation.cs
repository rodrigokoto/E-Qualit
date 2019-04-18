using Dominio.Entidade;
using Dominio.Enumerado;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.NaoConformidades
{
    public class CamposObrigatoriosNaoConformidadeEtapa2Validation : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosNaoConformidadeEtapa2Validation()
        {
            RuleFor(x => x.IdRegistroConformidade)
                .NotEmpty().WithMessage(Traducao.Resource.MsgNaoPodeVazio);

            RuleFor(x => x.EProcedente)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_EProcedente);

            RuleFor(x => x.DescricaoAcao)
               .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_DescricaoAcao)
               .Length(4, 2000).WithMessage(Traducao.Resource.NaoConformidade_msg_erro_min_e_max_DescricaoAcao)
               .When(x => x.EProcedente == false);

            RuleFor(x => x.DtDescricaoAcao)
                .NotEmpty().WithMessage(Traducao.Resource.MsgTraducao);                

            RuleFor(x=>x.ECorrecao)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_ECorrecao)
                .When(x => x.EProcedente == true);

            RuleFor(x => x.NecessitaAcaoCorretiva)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_NecessitaAcaoCorretiva)
                .When(x => x.EProcedente == true && x.StatusEtapa == (byte)EtapasRegistroConformidade.Reverificacao);

            //RuleFor(x => x.IdRegistroFilho)
            //    .NotEmpty().WithMessage("Traducao Obrigatorio")
            //    .When(x => x.NecessitaAcaoCorretiva == true);

            RuleFor(x=>x.DescricaoAnaliseCausa)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_DescricaoAnaliseCausa)
                .Length(4, 2000).WithMessage(Traducao.Resource.NaoConformidade_msg_erro_min_e_max_DescricaoAnaliseCausa)
                .When(x=> x.NecessitaAcaoCorretiva == true);


            RuleFor(x => x.AcoesImediatas)
                .NotEmpty().WithMessage(Traducao.Resource.NaoConformidade_msg_erro_required_AcoesImediatas)
                .When(x => x.EProcedente == true);

            RuleFor(x => x.StatusEtapa)
              .Must(x => x.Equals((byte)EtapasRegistroConformidade.Implementacao))
              .WithMessage(Traducao.Resource.MsgTraducaoTipoInvalido);

        }

    }
}
