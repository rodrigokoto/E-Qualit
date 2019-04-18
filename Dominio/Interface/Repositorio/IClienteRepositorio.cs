using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface IClienteRepositorio : IBaseRepositorio<Cliente>
    {
        IEnumerable<Cliente> ListarClientesPorUsuario(int idUsuario);

        bool Excluir(int id);
    }
}
