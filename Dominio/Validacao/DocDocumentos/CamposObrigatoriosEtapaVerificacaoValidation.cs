using Dominio.Entidade;
using FluentValidation;

namespace Dominio.Validacao.DocDocumentos
{
    public class CamposObrigatoriosEtapaVerificacaoValidation : AbstractValidator<DocDocumento>
    {
        public CamposObrigatoriosEtapaVerificacaoValidation()
        {
            RuleFor(documento => documento.DocCargo.Count)
             .GreaterThan(0).WithMessage(Traducao.Resource.DocDocumento_msg_erro_required_DocCargo);
        }
    }
}
