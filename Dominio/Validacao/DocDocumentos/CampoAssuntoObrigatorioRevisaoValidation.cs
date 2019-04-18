using Dominio.Entidade;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Validacao.DocDocumentos
{

    public class CampoAssuntoObrigatorioRevisaoValidation : AbstractValidator<DocumentoAssunto>
    {
       
        public CampoAssuntoObrigatorioRevisaoValidation()
        {
            RuleFor(assunto => assunto.Descricao)
                .NotEmpty().WithMessage(Traducao.Resource.DocDocumento_msg_erro_required_DocAssunto);           
        }

    }
}
