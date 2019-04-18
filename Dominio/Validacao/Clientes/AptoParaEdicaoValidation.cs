using DomainValidation.Validation;
using Dominio.Entidade;
using Dominio.Especificacao.Clientes;
using Dominio.Interface.Repositorio;

namespace Dominio.Validacao.Clientes
{
    public class AptoParaEdicaoValidation : Validator<Cliente>
    {
        public AptoParaEdicaoValidation(IClienteRepositorio clienteRepositorio)
        {
            var clienteDeveEstarCadastrado = new ClienteDeveEstarCadastrado(clienteRepositorio);

            base.Add(Traducao.Resource.IdCliente, new Rule<Cliente>(clienteDeveEstarCadastrado, Traducao.Cliente.ResourceCliente.Cliente_msg_not_found_IdCliente));
        }
    
    }
}
