﻿using DAL.Context;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repository
{
    public class RegistroAcaoImediataRepositorio: BaseRepositorio<RegistroAcaoImediata>, IRegistroAcaoImediataRepositorio
    {
        public void AtualizaAcaoImediataComAnexos(RegistroAcaoImediata obj)
        {
            //using (var context = new BaseContext())
            //{
                try
                {
                    if (obj.ArquivoEvidencia.Any(x => x.IdArquivoDeEvidenciaAcaoImediata == 0))
                    {
                        Db.Set<ArquivoDeEvidenciaAcaoImediata>().Add(obj.ArquivoEvidencia.FirstOrDefault());
                        //context.Set<ArquivoDeEvidenciaAcaoImediata>().Add(obj.ArquivoEvidencia.FirstOrDefault());
                    }

                    //context.Entry(obj).State = EntityState.Modified;
                    Db.Entry(obj).State = EntityState.Modified;

                    //context.SaveChanges();
                    Db.SaveChanges();
                }
                catch (System.Exception ex)
                {

                }
               
            //}
        }
    }
}
