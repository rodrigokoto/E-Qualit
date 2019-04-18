using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface IClienteContratoRepositorio : IBaseRepositorio<ClienteContrato>
    {
        IEnumerable<ClienteContrato> ListarClientesContratosPorCliente(int idCliente);
    }
}
