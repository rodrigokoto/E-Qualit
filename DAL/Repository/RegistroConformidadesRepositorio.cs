using DAL.Context;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Linq;

namespace DAL.Repository
{
    public class RegistroConformidadesRepositorio : BaseRepositorio<RegistroConformidade>, IRegistroConformidadesRepositorio
    {
        public RegistroConformidade GerarNumeroSequencialPorSite(RegistroConformidade registroConformidade)
        {
            using (var dbContext = new BaseContext())
            {
                dbContext.Configuration.LazyLoadingEnabled = false;
                try
                {
                    var listaMaxGestao = dbContext.RegistroConformidade
                    .Where(x => x.IdSite == registroConformidade.IdSite && x.TipoRegistro == registroConformidade.TipoRegistro).ToList();

                    if (listaMaxGestao.Count == 0)
                    {
                        registroConformidade.NuRegistro = 1;
                    }
                    else
                    {
                        var maxNumeroRegistro = listaMaxGestao.Max(x => x.NuRegistro);
                        registroConformidade.NuRegistro = maxNumeroRegistro + 1;
                    }


                    return registroConformidade;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            
        }

        public void DeletarRegistro(int idGestaoDeRisco)
        {
            try
            {
                var teste = Db.RegistroConformidade
                                    .Where(x => x.IdRegistroConformidade == idGestaoDeRisco).FirstOrDefault();

                Db.RegistroConformidade.Remove(teste);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RegistroConformidade GetByIdAsNoTracking(int id)
        {
            using (var dbContext = Db)
            {
                dbContext.Configuration.LazyLoadingEnabled = false;

                var registroSimple = dbContext.RegistroConformidade
                    .AsNoTracking()
                    .Where(x => x.IdRegistroConformidade == id)
                    .FirstOrDefault();

                registroSimple.Emissor = dbContext.Usuario
                    .Where(x => x.IdUsuario == registroSimple.IdEmissor)
                    .FirstOrDefault();

                return registroSimple;

            }
        }
    }
}
