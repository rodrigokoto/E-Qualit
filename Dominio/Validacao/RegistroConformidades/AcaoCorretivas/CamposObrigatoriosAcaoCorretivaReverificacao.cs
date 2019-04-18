using Dominio.Entidade;
using FluentValidation;
using System.Linq;

namespace Dominio.Validacao.RegistroConformidades.AcaoCorretivas
{
    public class CamposObrigatoriosAcaoCorretivaReverificacao : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosAcaoCorretivaReverificacao()
        {
            RuleFor(x => x.AcoesImediatas)
              .Must(x => x.Count == x.Where(y => y.Aprovado != null).Count()).WithMessage(Traducao.Resource.MsgCampoAprovacaoPreenchido);

            RuleFor(x => x.FlEficaz)
                .NotNull().WithMessage(Traducao.Resource.MsgNaoPodeVazio);
        }
    }
}
