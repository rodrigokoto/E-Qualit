using Dominio.Entidade;
using FluentValidation;
using System.Linq;

namespace Dominio.Validacao.RegistroConformidades.AcaoCorretivas
{
    public class CamposObrigatoriosAcaoCorretivaEncerrada : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosAcaoCorretivaEncerrada()
        {
            RuleFor(x => x.AcoesImediatas)
                .Must(x => x.Count == x.Where(y => y.Aprovado != null).Count()).WithMessage(Traducao.Resource.MsgCampoAprovacaoPreenchido);

            RuleFor(x => x.Parecer)
                .NotNull().WithMessage("Parecer é obrigatório");
        }
    }
}
