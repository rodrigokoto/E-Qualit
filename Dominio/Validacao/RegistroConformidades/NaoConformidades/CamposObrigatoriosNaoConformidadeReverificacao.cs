using Dominio.Entidade;
using FluentValidation;
using System.Linq;

namespace Dominio.Validacao.RegistroConformidades.NaoConformidades
{
    public class CamposObrigatoriosNaoConformidadeReverificacao : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosNaoConformidadeReverificacao()
        {
            RuleFor(x => x.AcoesImediatas)
                .Must(x => x.Count == x.Where(y => y.Aprovado != null).Count()).WithMessage(Traducao.Resource.MsgCampoAprovacaoPreenchido);

            RuleFor(x => x.FlEficaz)
                .NotNull().WithMessage(Traducao.Resource.MsgNaoPodeVazio);
        }
    }
}
