using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface IUsuarioCargoRepositorio : IBaseRepositorio<UsuarioCargo>
    {
        List<UsuarioCargo> ObterPorIdUsuario(int idUsuario);

        void DeletaTodosCargoUsuario(int idUsuario);

        List<UsuarioCargo> ListarUsuarioCargoPorCargo(int idCargo);
    }
}
