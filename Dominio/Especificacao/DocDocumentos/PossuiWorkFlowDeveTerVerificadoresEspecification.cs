using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Servico;

namespace Dominio.Especificacao.DocDocumentos
{
    public class PossuiWorkFlowDeveTerVerificadoresEspecification : ISpecification<DocDocumento>
    {
        public bool IsSatisfiedBy(DocDocumento docDocumento)
        {
            if (docDocumento.FlWorkFlow == true)
            {
                if (UtilsServico.ListaEstaPreenchido(docDocumento.Verificadores))
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }
    }
}
