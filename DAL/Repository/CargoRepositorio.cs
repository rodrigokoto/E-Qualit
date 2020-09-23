using DAL.Context;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repository
{
    public class CargoRepositorio : BaseRepositorio<Cargo>, ICargoRepositorio
    {

        public IEnumerable<Cargo> ListarCargosPorSite(int idSite)
        {
            return Db.Cargo.Where(x => x.IdSite == idSite);
        }

        public bool Excluir(int id)
        {


            using (var context = new BaseContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {

                    try
                    {
                        var cargos = context.Cargo.Where(x => x.IdCargo == id).ToList();

                        foreach (var cargo in cargos)
                        {

                            var usuarioCargos = context.UsuarioCargo.Where(x => x.IdCargo == cargo.IdCargo).ToList();

                            foreach (var usuarioCargo in usuarioCargos)
                            {

                                var usuariosClientesSites = context.UsuarioClienteSite.Where(x => x.IdUsuario == usuarioCargo.IdUsuario).ToList();

                                foreach (var usuarioClienteSite in usuariosClientesSites)
                                {

                                    var usuarioAnexos = context.UsuarioAnexo.Where(x => x.IdUsuario == usuarioClienteSite.IdUsuario).ToList();

                                    foreach (var usuarioAnexo in usuarioAnexos)
                                    {
                                        context.Entry(usuarioAnexo).State = EntityState.Deleted;

                                        var anexo = context.Anexo.Where(x => x.IdAnexo == usuarioAnexo.IdAnexo).FirstOrDefault();
                                        context.Entry(anexo).State = EntityState.Deleted;
                                    }

                                    var usuarioSenhas = context.UsuarioSenha.Where(x => x.IdUsuario == usuarioClienteSite.IdUsuario).ToList();

                                    foreach (var usuarioSenha in usuarioSenhas)
                                    {
                                        context.Entry(usuarioSenha).State = EntityState.Deleted;
                                    }

                                    context.Entry(usuarioClienteSite).State = EntityState.Deleted;

                                    var usuario = context.Usuario.Where(x => x.IdUsuario == usuarioClienteSite.IdUsuario).FirstOrDefault();

                                    context.Entry(usuario).State = EntityState.Deleted;
                                }

                                context.Entry(usuarioCargo).State = EntityState.Deleted;
                            }

                            var cargoProcessos = context.CargoProcesso.Where(x => x.IdCargo == cargo.IdCargo).ToList();

                            foreach (var cargoProcesso in cargoProcessos)
                            {
                                context.Entry(cargoProcesso).State = EntityState.Deleted;
                            }

                            context.Entry(cargo).State = EntityState.Deleted;
                        }

                        context.SaveChanges();

                        dbContextTransaction.Commit();
                        return true;

                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        return false;
                    }
                }
            }

        }
    }
}
