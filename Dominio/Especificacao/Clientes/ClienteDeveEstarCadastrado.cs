using DomainValidation.Interfaces.Specification;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace Dominio.Especificacao.Clientes
{
    public class ClienteDeveEstarCadastrado : ISpecification<Cliente>
    {
        private readonly IClienteRepositorio _repositorio;

        public ClienteDeveEstarCadastrado(IClienteRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool IsSatisfiedBy(Cliente cliente)
        {
            return _repositorio.GetById(cliente.IdCliente) != null;
        }
    }
}
