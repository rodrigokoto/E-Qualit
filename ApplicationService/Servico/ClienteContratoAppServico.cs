using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class ClienteContratoAppServico : BaseServico<ClienteContrato>, IClienteContratoAppServico
    {
        private readonly IClienteContratoRepositorio _clienteContratoRepositorio;

        public ClienteContratoAppServico(IClienteContratoRepositorio clienteContratoRepositorio) : base(clienteContratoRepositorio)
        {
            _clienteContratoRepositorio = clienteContratoRepositorio;
        }
    }
}
