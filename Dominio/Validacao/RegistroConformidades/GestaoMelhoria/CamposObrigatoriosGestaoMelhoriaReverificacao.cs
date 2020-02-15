using Dominio.Entidade;
using FluentValidation;
using System.Linq;

namespace Dominio.Validacao.RegistroConformidades.GestaoMelhorias
{
    public class CamposObrigatoriosGestaoMelhoriaReverificacao : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosGestaoMelhoriaReverificacao()
        {
            RuleFor(x => x.AcoesImediatas)
                .Must(x => x.Count == x.Where(y => y.Aprovado != null).Count()).WithMessage(Traducao.Resource.MsgCampoAprovacaoPreenchido);
        }
    }
}
