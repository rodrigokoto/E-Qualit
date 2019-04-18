using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;

namespace Dominio.Especificacao.Usuarios
{
    public class DeveEstarDesbloqueado : ISpecification<Usuario>
    {
        public bool IsSatisfiedBy(Usuario usuario)
        {
            if (usuario.FlBloqueado)
            {
                return false;
            }
            return true;
        }
    }
}
