using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ClienteLogoRepositorio : BaseRepositorio<ClienteLogo>, IClienteLogoRepositorio
    {
        public IEnumerable<ClienteLogo> ListarClientesLogosPorCliente(int idCliente)
        {
            return Db.ClienteLogo.Where(x => x.Cliente.IdCliente == idCliente);
        }
    }
}
