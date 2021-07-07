using Dominio.Entidade;
using FluentValidation;
using System.Linq;

namespace Dominio.Validacao.RegistroConformidades.NaoConformidades
{
    public class CamposObrigatoriosNaoConformidadeReverificacao : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosNaoConformidadeReverificacao()
        {
           
            RuleFor(x => x.FlEficaz)
                .NotNull().WithMessage(Traducao.Resource.MsgNaoPodeVazio);

            RuleFor(x => x.Parecer)
                .NotNull().WithMessage("Parecer é obrigatório");

        }
    }
}
