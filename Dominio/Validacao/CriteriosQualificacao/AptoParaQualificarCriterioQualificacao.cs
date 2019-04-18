using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.CriteriosQualificacao
{
    public class AptoParaQualificarCriterioQualificacao : AbstractValidator<AvaliaCriterioQualificacao>
    {
        public AptoParaQualificarCriterioQualificacao()
        {
            //RuleFor(x => x.ArquivoEvidencia)
            //    .NotEmpty()
            //    .WithMessage("Obrigatório");

            RuleFor(x => x.Aprovado)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgCampoAprovado);

            //RuleFor(x => x.Observacoes)
            //    .NotEmpty()
            //    .WithMessage(Traducao.Resource.MsgCampoObservacoes);

            RuleFor(x => x.IdResponsavelPorControlarVencimento)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgCampoControleVencimento);

            RuleFor(x => x.IdResponsavelPorQualificar)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgCampoQualificar);

            RuleFor(x => x.DtEmissao)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgDataEmissao);

            //RuleFor(x => x.DtAlteracaoEmissao)
            //    .NotEmpty()
            //    .WithMessage("Data alteração emissão é Obrigatória");

            RuleFor(x => x.DtQualificacaoVencimento)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgDataQualificacao);

            RuleFor(x => x.NumeroDocumento)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgNumeroDoc);

            RuleFor(x => x.OrgaoExpedidor)
                .NotEmpty()
                .WithMessage(Traducao.Resource.MsgOrgaoExpedidor);

            //RuleFor(x => x.ObservacoesDocumento)
            //    .NotEmpty()
            //    .WithMessage(Traducao.Resource.MsgObservacoesObrigatorias);
        }
    }
}
       