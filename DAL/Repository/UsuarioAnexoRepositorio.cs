using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class UsuarioAnexoRepositorio : BaseRepositorio<UsuarioAnexo>, IUsuarioAnexoRepositorio
    {

        public List<UsuarioAnexo> ListarUsuarioAnexoPorUsuario(int idUsuario)
        {
            return Db.UsuarioAnexo.Where(x => x.IdUsuario == idUsuario).ToList();
        }
        
    }
}
