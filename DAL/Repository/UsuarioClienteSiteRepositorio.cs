using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class UsuarioClienteSiteRepositorio : BaseRepositorio<UsuarioClienteSite>, IUsuarioClienteSiteRepositorio
    {
        public List<UsuarioClienteSite> ListarPorUsuario(int idUsuario)
        {
            return Db.UsuarioClienteSite.Where(x => x.IdUsuario == idUsuario).ToList();
        }

        public List<UsuarioClienteSite> ListarUsuarioClienteSite(int idCliente, int idSite)
        {
            return Db.UsuarioClienteSite.Where(x => x.IdCliente == idCliente && x.IdSite == idSite).ToList();
        }

        
    }
}
