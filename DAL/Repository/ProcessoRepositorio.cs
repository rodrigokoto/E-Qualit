using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repository
{
    public class ProcessoRepositorio : BaseRepositorio<Processo>, IProcessoRepositorio
    {
        public List<Processo> ListaProcessosPorSite(int site)
        {
            using (var dbContext = Db)
            {
                dbContext.Configuration.ProxyCreationEnabled = false;

                return dbContext.Processo.AsNoTracking().Where(x => x.IdSite == site).ToList();
            }
        }
    }
}
