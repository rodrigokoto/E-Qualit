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
            using (var context = new BaseContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {

                    try
                    {
                        var clienteContratos = context.ClienteContrato.Where(x => x.IdCliente == id).ToList();

                        foreach (var item in clienteContratos)
                        {
                            context.Entry(item).State = EntityState.Deleted;

                            var anexo = context.Anexo.Where(x => x.IdAnexo == item.IdAnexo).FirstOrDefault();

                            context.Entry(anexo).State = EntityState.Deleted;


                        }

                        var clienteLogos = context.ClienteLogo.Where(x => x.IdCliente == id).ToList();

                        foreach (var item in clienteLogos)
                        {
                            context.Entry(item).State = EntityState.Deleted;

                            var anexo = context.Anexo.Where(x => x.IdAnexo == item.IdAnexo).FirstOrDefault();
                            context.Entry(anexo).State = EntityState.Deleted;
                        }

                        var sites = context.Site.Where(x => x.IdCliente == id).ToList();

                        foreach (var item in sites)
                        {
                            var siteAnexos = context.SiteAnexo.Where(x => x.IdSite == item.IdSite).ToList();

                            foreach (var siteAnexo in siteAnexos)
                            {
                                context.Entry(siteAnexo).State = EntityState.Deleted;

                                var anexo = context.Anexo.Where(x => x.IdAnexo == siteAnexo.IdAnexo).FirstOrDefault();
                                context.Entry(anexo).State = EntityState.Deleted;
                            }

                            var processos = context.Processo.Where(x => x.IdSite == item.IdSite).ToList();

                            foreach (var processo in processos)
                            {
                                context.Entry(processo).State = EntityState.Deleted;                                
                            }

                            var subModulos = context.SubModulo.Where(x => x.CodigoSite == item.IdSite).ToList();

                            foreach (var subModulo in subModulos)
                            {
                                context.Entry(subModulo).State = EntityState.Deleted;
                            }

                            var siteModulos = context.SiteModulo.Where(x => x.IdSite == item.IdSite).ToList();

                            foreach (var siteModulo in siteModulos)
                            {
                                context.Entry(siteModulo).State = EntityState.Deleted;
                            }

                            var cargos = context.Cargo.Where(x => x.IdSite == item.IdSite).ToList();

                            foreach (var cargo in cargos)
                            {

                                var usuarioCargos = context.UsuarioCargo.Where(x => x.IdCargo == cargo.IdCargo).ToList();

                                foreach (var usuarioCargo in usuarioCargos)
                                {
                                    context.Entry(usuarioCargo).State = EntityState.Deleted;
                                }

                                var cargoProcessos = context.CargoProcesso.Where(x => x.IdCargo == cargo.IdCargo).ToList();

                                foreach (var cargoProcesso in cargoProcessos)
                                {
                                    context.Entry(cargoProcesso).State = EntityState.Deleted;
                                }

                                context.Entry(cargo).State = EntityState.Deleted;
                            }

                            var usuariosClientesSites = context.UsuarioClienteSite.Where(x => x.IdSite == item.IdSite && x.IdCliente == id).ToList();

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

                            context.Entry(item).State = EntityState.Deleted;
                        }

                        var cliente = context.Cliente.Where(x => x.IdCliente == id).FirstOrDefault();
                        context.Entry(cliente).State = EntityState.Deleted;

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
