using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.Indicadores
{
    public class AptoParaCadastroValidation : AbstractValidator<Indicador>
    {
        public AptoParaCadastroValidation()
        {
            RuleFor(x => x.Direcao)
            .NotNull()
            .WithMessage(Traducao.Resource.Msg_SentidoMeta);

            RuleFor(x => x.Periodicidade)
                .NotNull()
                .WithMessage(Traducao.Resource.Msg_Periodicidade);

            RuleFor(x => x.Objetivo)
                .MaximumLength(1000)
                .WithMessage(Traducao.Resource.Msg_Max1000Objetivo);

            RuleFor(indicador => indicador.Descricao)
              .NotEmpty().WithMessage(Traducao.Resource.MsgCampoDescricao);

            RuleFor(indicador => indicador.IdResponsavel)
              .NotEmpty().WithMessage(Traducao.Resource.MsgResponsavel);

            RuleFor(indicador => indicador.IdProcesso)
              .NotEmpty().WithMessage(Traducao.Resource.MsgProcessoPreenchido);

            RuleFor(indicador => indicador.PeriodicidadeDeAnalises)
              .NotEmpty().WithMessage(Traducao.Resource.MsgPeriodo);

            RuleFor(indicador => indicador.PeriodicidadeMedicao)
                .NotEmpty().WithMessage(Traducao.Resource.Msg_Periodicidade);

            RuleFor(indicador => indicador.Objetivo)
              .NotEmpty().WithMessage(Traducao.Resource.MsgObjetivoInformado);

            RuleFor(indicador => indicador.Unidade)
              .NotEmpty().WithMessage(Traducao.Resource.MsgUnidadeInformada);

            RuleFor(indicador => indicador.IdSite)
             .NotEmpty().WithMessage(Traducao.Resource.MsgSiteInformado);

        }
    }
}
