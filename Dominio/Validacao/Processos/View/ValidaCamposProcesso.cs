using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.Processos.View
{
    public abstract class ValidaCamposProcesso<T> : AbstractValidator<T> where T : Processo
    {
        protected void NomeObrigatorio()
        {
            RuleFor(x => x.Nome)
                .MinimumLength(2).WithMessage(Traducao.Processo.ResourceProcesso.Processo_msg_required_minlength_Nome)
                .MaximumLength(60).WithMessage(Traducao.Processo.ResourceProcesso.Processo_msg_required_maxlength_Nome)
                .NotEmpty().WithMessage(Traducao.Processo.ResourceProcesso.Processo_msg_required_Nome);
        }

        protected void DeveEstaAtivo()
        {
            RuleFor(x => x.FlAtivo)
               .Must(b=>b.Equals(true)).WithMessage(Traducao.Resource.MsgDeveEstarAtivo);
        }

        protected void SiteObrigatorio()
        {
            RuleFor(x => x.IdSite)
                .NotEmpty().WithMessage(Traducao.Processo.ResourceProcesso.Processo_msg_required_IdSite);
        }

        protected void IdProcessoObrigatorio()
        {
            RuleFor(x => x.IdProcesso)
                .NotEmpty().WithMessage(Traducao.Processo.ResourceProcesso.Processo_msg_required_IdProcesso);
        }
    }
}
