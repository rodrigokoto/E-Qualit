using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class SubModuloRepositorio : BaseRepositorio<SubModulo>, ISubModuloRepositorio
    {
        public IEnumerable<SubModulo> ListarSubModuloPorSite(int idSite)
        {
            return Db.SubModulo.Where(x => x.Site.IdSite == idSite);
        }        
    }
}
