using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Repositorio
{
    public interface IUsuarioClienteSiteRepositorio : IBaseRepositorio<UsuarioClienteSite>
    {
        List<UsuarioClienteSite> ListarPorUsuario(int idUsuario);
        List<UsuarioClienteSite> ListarUsuarioClienteSite(int idCliente, int idSite);
    }
}
