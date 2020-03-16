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
            using (var context = new BaseContext())
            {
                indicador.PeriodicidadeDeAnalises[0].IdIndicador = indicador.Id;
                indicador.PeriodicidadeDeAnalises[0].MetasRealizadas.ForEach(metaRealizada =>
                {
                    if (metaRealizada.Id > 0)
                    {

                        var planoDeVooCtx = context.Set<PlanoVoo>().Find(metaRealizada.Id);
                        if (planoDeVooCtx.IdGestaoRisco != null)
                            planoDeVooCtx.GestaoDeRisco = context.Set<RegistroConformidade>().Find(planoDeVooCtx.IdGestaoRisco);

                        planoDeVooCtx.Analise = metaRealizada.Analise;

                        if (metaRealizada.Realizado != null)
                        {
                            planoDeVooCtx.Realizado = metaRealizada.Realizado;
                            planoDeVooCtx.CorRisco = metaRealizada.CorRisco;
                            planoDeVooCtx.IdProcesso = indicador.IdProcesso;

                            if (metaRealizada.GestaoDeRisco != null)
                            {
                                if (planoDeVooCtx.IdGestaoRisco != null)
                                {

                                    planoDeVooCtx.GestaoDeRisco.TipoRegistro = "gr";
                                    planoDeVooCtx.GestaoDeRisco.IdSite = indicador.IdSite;
                                    planoDeVooCtx.GestaoDeRisco.IdUsuarioIncluiu = indicador.IdUsuarioIncluiu;
                                    planoDeVooCtx.GestaoDeRisco.IdUsuarioAlterou = indicador.IdUsuarioIncluiu;
                                    planoDeVooCtx.GestaoDeRisco.IdEmissor = indicador.IdUsuarioIncluiu;
                                    planoDeVooCtx.IdProcesso = indicador.IdProcesso;
                                    planoDeVooCtx.GestaoDeRisco.StatusEtapa = 1;
                                    planoDeVooCtx.GestaoDeRisco.EProcedente = true;

                                    context.Entry(planoDeVooCtx.GestaoDeRisco).State = EntityState.Modified;
                                }
                                else
                                {
                                    if (metaRealizada.GestaoDeRisco.DescricaoRegistro != null)
                                    {
                                        planoDeVooCtx.GestaoDeRisco = metaRealizada.GestaoDeRisco;
                                        planoDeVooCtx.GestaoDeRisco.TipoRegistro = "gr";
                                        planoDeVooCtx.GestaoDeRisco.IdSite = indicador.IdSite;
                                        planoDeVooCtx.GestaoDeRisco.IdUsuarioIncluiu = indicador.IdUsuarioIncluiu;
                                        planoDeVooCtx.GestaoDeRisco.IdProcesso = indicador.IdProcesso;
                                        planoDeVooCtx.GestaoDeRisco.IdUsuarioAlterou = indicador.IdUsuarioIncluiu;
                                        planoDeVooCtx.GestaoDeRisco.IdEmissor = indicador.IdUsuarioIncluiu;
                                        planoDeVooCtx.GestaoDeRisco.IdResponsavelInicarAcaoImediata = indicador.IdResponsavel;
                                        planoDeVooCtx.IdProcesso = indicador.IdProcesso;
                                        planoDeVooCtx.GestaoDeRisco.StatusEtapa = 1;
                                        planoDeVooCtx.GestaoDeRisco.EProcedente = true;

                                        planoDeVooCtx.GestaoDeRisco = new RegistroConformidadesRepositorio().GerarNumeroSequencialPorSite(planoDeVooCtx.GestaoDeRisco);
                                    }
                                }
                            }
                            //gestão de risco

                            //planoDeVooCtx.DataAlteracao = DateTime.Now;
                            context.Entry(planoDeVooCtx).State = EntityState.Modified;

                        }
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
