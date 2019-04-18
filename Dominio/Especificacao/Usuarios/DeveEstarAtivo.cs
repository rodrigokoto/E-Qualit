using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;

namespace Dominio.Especificacao.Usuarios
{
    public class DeveEstarAtivo : ISpecification<Usuario>
    {
        public bool IsSatisfiedBy(Usuario usuario)
        {
            if (usuario.FlAtivo)
            {
                return true;
            }
            return false;
        }
    }
}
