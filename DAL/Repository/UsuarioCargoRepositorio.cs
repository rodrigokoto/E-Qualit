using DAL.Context;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repository
{
    public class UsuarioCargoRepositorio : BaseRepositorio<UsuarioCargo>, IUsuarioCargoRepositorio
    {
        public void DeletaTodosCargoUsuario(int idUsuario)
        {
            var itensDeletados = Get(x=>x.IdUsuario == idUsuario).ToList();
            using(var context = new BaseContext())
            {
                itensDeletados.ForEach(x =>
                Db.Entry(x).State = EntityState.Deleted
                
                );


                context.SaveChanges();
            }
        }

        public List<UsuarioCargo> ObterPorIdUsuario(int idUsuario)
        {
            return Db.UsuarioCargo.Where(x => x.IdUsuario == idUsuario).ToList();
        }

        public List<UsuarioCargo> ListarUsuarioCargoPorCargo(int idCargo)
        {
            return Db.UsuarioCargo.Where(x => x.IdCargo == idCargo).ToList();
        }
        
    }
}
