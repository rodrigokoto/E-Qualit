using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Linq;

namespace Dominio.Especificacao.Usuarios
{
    public class DeveTerContaNaoCompartilhada : ISpecification<Usuario>
    {
        private readonly IUsuarioRepositorio _repositorio;

        public DeveTerContaNaoCompartilhada(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool IsSatisfiedBy(Usuario usuario)
        {
            usuario = _repositorio.Get(x => x.CdIdentificacao == usuario.CdIdentificacao).FirstOrDefault();
            if (usuario != null)
            {
                return usuario.FlCompartilhado == false;
            } else
            {
                return false;
            }
        }
    }
}
