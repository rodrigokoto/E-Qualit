using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class SiteAnexoRepositorio : BaseRepositorio<SiteAnexo>, ISiteAnexoRepositorio
    {

        public IEnumerable<SiteAnexo> ListarSiteAnexoPorSite(int idSite)
        {
            return Db.SiteAnexo.Where(x => x.IdSite == idSite);
        }
        

    }
}
