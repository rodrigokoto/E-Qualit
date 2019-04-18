using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace Dominio.Especificacao.Usuarios
{
    public class DevePossuirCPFUnico : ISpecification<Usuario>
    {
        private readonly IUsuarioRepositorio _repositorio;

        public DevePossuirCPFUnico(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool IsSatisfiedBy(Usuario usuario)
        {
            return usuario.NuCPF != null ? _repositorio.Get(x => x.NuCPF == usuario.NuCPF).FirstOrDefault() == null : true;
        }
    }
}
