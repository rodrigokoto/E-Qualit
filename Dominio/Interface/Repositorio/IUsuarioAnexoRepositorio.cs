using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface IUsuarioAnexoRepositorio : IBaseRepositorio<UsuarioAnexo>
    {
        List<UsuarioAnexo> ListarUsuarioAnexoPorUsuario(int idUsuario);
    }
}
