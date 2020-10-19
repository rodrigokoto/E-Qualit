using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.RegistroConformidades.GestaoMelhorias
{
    public class CamposObrigatoriosGestaoMelhoriaImplementacaoDtEfetivaImplementacao : AbstractValidator<RegistroConformidade>
    {
        public CamposObrigatoriosGestaoMelhoriaImplementacaoDtEfetivaImplementacao()
        {
         

            RuleFor(x => x.IdResponsavelReverificador)
                .NotNull().WithMessage(Traducao.Resource.MsgCampoReverificador)
                .Must(x => x > 0).WithMessage(Traducao.Resource.MsgCampoReverificador);


            //RuleFor(documento => documento.Assuntos.Where(p => p.Id == 0).Count())
            // .GreaterThan(0).WithMessage(Traducao.Resource.DocDocumento_msg_erro_required_DocAssunto);
        }
    }
}
