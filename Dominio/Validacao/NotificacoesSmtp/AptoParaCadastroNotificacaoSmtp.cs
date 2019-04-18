using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.NotificacoesSmtp
{
    public class AptoParaCadastroNotificacaoSmtp : AbstractValidator<NotificacaoSmtp>
    {
        public AptoParaCadastroNotificacaoSmtp()
        {
            RuleFor(x => x.CdSenha)
                .NotEmpty().WithMessage(Traducao.Resource.CtrlUsuario_lbl_CdSenha);

            RuleFor(x => x.CdUsuario)
                .NotEmpty().WithMessage(Traducao.Resource.CdUsuario);

            RuleFor(x=>x.DsSmtp)
                .NotEmpty().WithMessage(Traducao.Resource.DsSmtp);

            RuleFor(x => x.NmNome)
                .NotEmpty().WithMessage(Traducao.Resource.Fornecedor_lbl_Nome);

            RuleFor(x=>x.NuPorta)
                .NotEmpty().WithMessage(Traducao.Resource.NuPorta);

        }
    }
}
