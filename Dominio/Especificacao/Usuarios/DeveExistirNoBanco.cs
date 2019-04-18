using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace Dominio.Especificacao.Usuarios
{
    public class DeveExistirNoBanco : ISpecification<Usuario>
    {
        private readonly IUsuarioRepositorio _repositorio;

        public DeveExistirNoBanco(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool IsSatisfiedBy(Usuario usuario)
        {
            return _repositorio.GetById(usuario.IdUsuario) != null;
        }
    }
}
