using System.Collections.Generic;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Linq;
using DAL.Context;

namespace DAL.Repository
{
    public class ControladorCategoriasRepositorio : BaseRepositorio<ControladorCategoria>, IControladorCategoriasRepositorio
    {
        public List<ControladorCategoria> ListaGetByTipo(string tipo)
        {
            using (var dbContext = Db)
            {
                dbContext.Configuration.ProxyCreationEnabled = false;

                return dbContext.ControladorCategoria.AsNoTracking().Where(x => x.TipoTabela == tipo).ToList();
            }
     
        }

        public List<ControladorCategoria> ListaGetByTipoAndSite(string tipo, int site)
        {
            using (var dbContext = Db)
            {
                dbContext.Configuration.ProxyCreationEnabled = false;

                return dbContext.ControladorCategoria.AsNoTracking().Where(x => x.TipoTabela == tipo && x.IdSite == site).ToList();
            }

        }

        public List<ControladorCategoria> ListaAtivos(string tipo, int site)
        {
            using (var dbContext = new BaseContext())
            {
                dbContext.Configuration.ProxyCreationEnabled = false;

                return dbContext.ControladorCategoria
                    .AsNoTracking()
                    .Where(x => x.TipoTabela == tipo && x.IdSite == site && x.Ativo == true)
                    .OrderBy(x=>x.Descricao)
                    .ToList();
            }
        }

        public ControladorCategoria GetByIdAsNoTracking(int id)
        {
            return Db.ControladorCategoria
                .Select(b => b).FirstOrDefault(x => x.IdControladorCategorias == id);
        }
    }
}
