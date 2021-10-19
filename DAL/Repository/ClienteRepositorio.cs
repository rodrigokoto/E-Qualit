using DAL.Context;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repository
{
    public class ClienteRepositorio : BaseRepositorio<Cliente>, IClienteRepositorio
    {
        public IEnumerable<Cliente> ListarClientesPorUsuario(int idUsuario)
        {
            return Db.UsuarioClienteSite.Include("Cliente").Where(x => x.IdUsuario == idUsuario).Select(x => x.Cliente);
        }

        public bool Excluir(int id)
        {
            int id2 = 0;
            var parametros = new Dictionary<string, object>();
            parametros.Add("idCliente", id);
            parametros.Add("idSite", 0);

            using (var context = new BaseContext()) {

                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "dbo.ProcDeleteEmploy";

            
                foreach (var pr in parametros)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = pr.Key;
                    p.Value = pr.Value;
                    cmd.Parameters.Add(p);
                }

                try
                {
                    context.Database.Connection.Open();
                    var reader = cmd.ExecuteNonQuery();
                    

                }
                catch (Exception ex)
                {
                    return false;
                }
                finally 
                {
                    cmd.Connection.Close();
                }

                //context.Database.ExecuteSqlCommand("exec dbo.ProcDeleteEmploy @idCliente @idSite", parameters: new { id, id2 });
            }

            return true;
        }
    }
}
