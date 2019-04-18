using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class SiteModuloRepositorio : BaseRepositorio<SiteFuncionalidade>, ISiteModuloRepositorio
    {
        public List<SiteFuncionalidade> ListarSiteModuloPorSite(int idSite)
        {
            return Db.SiteModulo.Where(x => x.IdSite == idSite).ToList();
        }
    }
}
