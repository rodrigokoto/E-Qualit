using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;

namespace Dominio.Especificacao.Temas
{
    public class DeveTerGestaoDeRiscoInformadoEspecification : ISpecification<AnaliseCriticaTema>
    {
        public bool IsSatisfiedBy(AnaliseCriticaTema tema)
        {
            if (tema.GestaoDeRisco == null)
            {
                return false;
            }
            return true;
        }
    }
}
