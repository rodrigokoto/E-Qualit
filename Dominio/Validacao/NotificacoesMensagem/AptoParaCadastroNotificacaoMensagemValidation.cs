using Dominio.Entidade;
using FluentValidation;
using System;

namespace Dominio.Validacao.NotificacoesMensagem
{
    public class AptoParaCadastroNotificacaoMensagemValidation : AbstractValidator<NotificacaoMensagem>
    {
        public AptoParaCadastroNotificacaoMensagemValidation()
        {
            RuleFor(x => x.DsAssunto)
                .NotEmpty().WithMessage(Traducao.Resource.MsgAssunto)
                .Length(3, 200).WithMessage(Traducao.Resource.MsgAssuntoMin);

            RuleFor(x => x.DsMensagem)
                .NotEmpty().WithMessage(Traducao.Resource.MsgMensagem)
                .MinimumLength(4).WithMessage(Traducao.Resource.MsgMensagemMin);

            RuleFor(x => x.IdSite)
                .NotEmpty().WithMessage(Traducao.Resource.MsgIdSite)
                .GreaterThan(0).WithMessage(Traducao.Resource.MsgIdSiteMin);

            RuleFor(x=>x.IdSmtpNotificacao)
                .NotEmpty().WithMessage(Traducao.Resource.MsgNotificacao)
                .GreaterThan(0).WithMessage(Traducao.Resource.MsgNotificacaoMin);

            RuleFor(x => x.DtEnvio.Value)
                .Must(DtEnvioEValida).WithMessage(Traducao.Resource.MsgErroEnvio);

            RuleFor(x => x.NmEmailPara)
                .NotEmpty().WithMessage(Traducao.Resource.MsgRemetenteValido)
                .EmailAddress().WithMessage(Traducao.Resource.MsgRemetenteValido);

            RuleFor(x => x.NmEmailNome)
                .NotEmpty().WithMessage(Traducao.Resource.MsgRemetenteValido);


        }

        private bool DtEnvioEValida(DateTime date)
        {
            return (date < DateTime.Now);
        }
    }
}
