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
            if (calibracao.SubmitArquivosCertificado.Count > 0)
            {
                calibracao.SubmitArquivosCertificado.ForEach(x =>
                {
                    if (x.IdCalibracao == 0)
                    {
                        try
                        {
                            using (var context = new BaseContext())
                            {
                                x.Anexo.TratarComNomeCerto();
                                context.Entry(calibracao).State = EntityState.Modified;

                                context.Set<Anexo>().Add(x.Anexo);
                                context.SaveChanges();

                                var certificado = new ArquivoCertificadoAnexo();

                                certificado.IdAnexo = x.Anexo.IdAnexo;
                                certificado.IdCalibracao = calibracao.IdCalibracao;

                                context.Set<ArquivoCertificadoAnexo>().Add(certificado);
                                context.SaveChanges();

                            }
                        }
                        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        if (x.ApagarAnexo == 1)
                        {
                            try
                            {
                                using (var context = new BaseContext())
                                {
                                    
                                    var arqanexo = context.ArquivoCertificadoAnexo.Where(u => u.IdAnexo == x.IdAnexo).FirstOrDefault();
                                    context.Entry(arqanexo).State = EntityState.Deleted;

                                    context.ArquivoCertificadoAnexo.Remove(arqanexo);
                                    context.SaveChanges();
                                }
                            }
                            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                            {
                                throw ex;
                            }
                        }
                    }
                });
                //if (calibracao.ArquivoCertificado.Count == 0)
                //{

                //    calibracao.ArquivoCertificadoAux.ForEach(x =>
                //    {
                //        var anexo = new Anexo();

                //        anexo = x;

                //        anexo.TratarComNomeCerto();


                //        //anexo.Arquivo = x.Arquivo;
                //        //anexo.ArquivoB64 = x.ArquivoB64;
                //        //anexo.Extensao = x.Extensao.Split('.')[1];
                //        //anexo.Nome = x.Extensao.Split('.')[0];
                //        //anexo.DtCriacao = DateTime.Now;

                //        try
                //        {
                //            using (var context = new BaseContext())
                //            {
                //                context.Entry(calibracao).State = EntityState.Modified;

                //                context.Set<Anexo>().Add(anexo);
                //                context.SaveChanges();

                //                var certificado = new ArquivoCertificadoAnexo();

                //                certificado.IdAnexo = anexo.IdAnexo;
                //                certificado.IdCalibracao = calibracao.IdCalibracao;

                //                context.Set<ArquivoCertificadoAnexo>().Add(certificado);
                //                context.SaveChanges();

                //            }
                //        }
                //        catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                //        {
                //            throw ex;
                //        }
                //    });
            }
        }
    }
}

