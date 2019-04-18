using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class ClienteContratoRepositorio : BaseRepositorio<ClienteContrato>, IClienteContratoRepositorio
    {
        public IEnumerable<ClienteContrato> ListarClientesContratosPorCliente(int idCliente)
        {
            return Db.ClienteContrato.Where(x => x.Cliente.IdCliente == idCliente);
        }
    }
}
