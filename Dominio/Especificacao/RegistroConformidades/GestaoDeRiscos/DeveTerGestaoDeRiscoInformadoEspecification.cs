using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;

namespace Dominio.Especificacao.RegistroConformidades.GestaoDeRiscos
{
    class DeveTerGestaoDeRiscoInformadoEspecification : ISpecification<RegistroConformidade>
    {
        public bool IsSatisfiedBy(RegistroConformidade registroConformidade)
        {
            if (registroConformidade == null)
            {
                return false;
            }
            return true;
        }
    }
}
