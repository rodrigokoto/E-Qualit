using DAL.Context;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Data.Entity;

namespace DAL.Repository
{
    public class IndicadorRepositorio : BaseRepositorio<Indicador>, IIndicadorRepostorio
    {
        public void Atualizar(Indicador indicador)
        {
            using(var context = new BaseContext())
            {
                indicador.PeriodicidadeDeAnalises[0].IdIndicador = indicador.Id;
                indicador.PeriodicidadeDeAnalises[0].MetasRealizadas.ForEach(metaRealizada =>
                {
                    if(metaRealizada.Id> 0)
                    {
                        var planoDeVooCtx = context.Set<PlanoVoo>().Find(metaRealizada.Id);

                        planoDeVooCtx.Realizado = metaRealizada.Realizado;
                        //planoDeVooCtx.DataAlteracao = DateTime.Now;
                        context.Entry(planoDeVooCtx).State = EntityState.Modified;
                    }
                    else
                    {
                        context.Set<PlanoVoo>().Add(metaRealizada);
                    }

                });

                context.SaveChanges();
            }
        }
    }
}
