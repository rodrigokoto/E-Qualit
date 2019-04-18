using DAL.Context;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repository
{
    public class CalibracaoRepositorio : BaseRepositorio<Calibracao>, ICalibracaoRepositorio
    {
        public void RemoverComDelecaoDosRelacionamentos(int id)
        {
            try
            {
                var calibracao = Db.Calibracao
                                    .Include("CriterioAceitacao")
                                    .Where(x => x.IdCalibracao == id).FirstOrDefault();

                Db.Calibracao.Remove(calibracao);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CriarCalibracao(Calibracao calibracao)
        {
            using (var context = new BaseContext())
            {
                if (calibracao.ArquivoCertificado.Count > 0)
                {
                    var listaAdiconar = calibracao.ArquivoCertificado.Where(x => x.IdArquivoCertificadoAnexo == 0).ToList();
                    listaAdiconar.ForEach(x =>
                    {
                        context.Set<ArquivoCertificadoAnexo>().Add(x);
                    });

                    var listaEditarAnexo = calibracao.ArquivoCertificado.Where(x => x.IdArquivoCertificadoAnexo > 0).ToList();
                    listaAdiconar.ForEach(x =>
                    {
                        context.Set<Anexo>().Add(x.Anexo);
                    });
                }

                context.Set<Calibracao>().Add(calibracao);

                context.SaveChanges();
            }
        }

        public void Atualizar(Calibracao calibracao)
        {
            try
            {
                using (var context = new BaseContext())
                {
                    context.Entry(calibracao).State = EntityState.Modified;

                    if (calibracao.ArquivoCertificado.Count > 0)
                    {
                        var listaAdiconar = calibracao.ArquivoCertificado.Where(x => x.IdArquivoCertificadoAnexo == 0).ToList();
                        listaAdiconar.ForEach(x =>
                        {
                            context.Set<ArquivoCertificadoAnexo>().Add(x);
                        });

                        var listaEditarAnexo = calibracao.ArquivoCertificado.Where(x => x.IdArquivoCertificadoAnexo > 0).ToList();
                        listaAdiconar.ForEach(x =>
                        {
                            context.Set<Anexo>().Add(x.Anexo);
                        });
                    }

                    
                    context.SaveChanges();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                throw ex;
            }
        }
    }
}
