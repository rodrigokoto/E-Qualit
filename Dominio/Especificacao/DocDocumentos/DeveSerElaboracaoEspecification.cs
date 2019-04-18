using Dominio.Entidade;
using DomainValidation.Interfaces.Specification;
using Dominio.Enumerado;

namespace Dominio.Especificacao.DocDocumentos
{
    public class DeveSerElaboracaoEspecification : ISpecification<DocDocumento>
    {
        public bool IsSatisfiedBy(DocDocumento docDocumento)
        {
            if (docDocumento.FlStatus == (int)StatusDocumento.Elaboracao)
            {
                return true;
            }
            return false;
        }
    }
}
