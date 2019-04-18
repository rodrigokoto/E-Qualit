using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.Notificacoes
{
    public class AptoParaCadastroNotificacaoValidation : AbstractValidator<Notificacao>
    {
        public AptoParaCadastroNotificacaoValidation()
        {
            RuleFor(x => x.Descricao)
                 .NotEmpty().WithMessage(Traducao.Resource.notifcacao_msg_erro_DsDescricao_required)
                 .NotNull().WithMessage(Traducao.Resource.notifcacao_msg_erro_DsDescricao_required)
                 .MaximumLength(1000).WithMessage(Traducao.Resource.notifcacao_msg_erro_DsDescricao_length);

            RuleFor(x => x.NuDiasAntecedencia)
                 .GreaterThanOrEqualTo(0).WithMessage(Traducao.Resource.notifcacao_msg_erro_NuDiasAntecedencia_required);

            RuleFor(x => x.IdRelacionado)
                 .GreaterThanOrEqualTo(0).WithMessage(Traducao.Resource.notifcacao_msg_erro_NuDiasAntecedencia_required);

            RuleFor(x => x.FlEtapa)
                  .MaximumLength(20).WithMessage(Traducao.Resource.notifcacao_msg_erro_FlEtapa_length);

            RuleFor(x => x.TpNotificacao)
                   .NotEmpty().WithMessage(Traducao.Resource.notifcacao_msg_erro_TpNotificacao_required)
                   .NotNull().WithMessage(Traducao.Resource.notifcacao_msg_erro_TpNotificacao_required)
                   .MaximumLength(2).WithMessage(Traducao.Resource.notifcacao_msg_erro_DsDescricao_length);

            RuleFor(x => x.IdProcesso)
                .NotEmpty().WithMessage(Traducao.Resource.AcaoCorretiva_msg_erro_required_IdProcesso);

            RuleFor(x => x.IdSite)
                .NotEmpty().WithMessage(Traducao.Resource.AcaoCorretiva_msg_erro_required_IdSite);

            RuleFor(x => x.IdRelacionado)
                .NotEmpty().WithMessage(Traducao.Resource.Endereco_lbl_IdRelacionado);

            RuleFor(x => x.IdFuncionalidade)
                .NotEmpty().WithMessage(Traducao.Resource.CargoProcesso_msg_erro_required_IdFuncao);

            RuleFor(x => x.IdUsuario)
                .NotEmpty().WithMessage(Traducao.Resource.CtrlUsuario_msg_erro_required_IdUsuario);

        }
    }
}
