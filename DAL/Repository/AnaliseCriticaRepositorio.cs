using DAL.Context;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Data.Entity;

namespace DAL.Repository
{
    public class AnaliseCriticaRepositorio : BaseRepositorio<AnaliseCritica>, IAnaliseCriticaRepositorio
    {
        private readonly RegistroConformidadesRepositorio _registroConformidadesRepositorio = new RegistroConformidadesRepositorio();

        public void AtualizaAnaliseCriticaFuncionario(AnaliseCritica analiseCritica)
        {
            using (var context = new BaseContext())
            {
                analiseCritica.Funcionarios.ForEach(funcionario =>
                {
                    var funcionarioCtx = context.Set<AnaliseCriticaFuncionario>().Find(funcionario.IdAnaliseCriticaFuncionario);
                    funcionarioCtx.IdUsuario = funcionario.IdUsuario;
                    funcionarioCtx.Funcao = funcionario.Funcao;

                    context.Entry(funcionarioCtx).State = EntityState.Modified;
                });

                context.SaveChanges();
            }
        }

        public void AtualizaAnaliseCriticaTema(AnaliseCritica analiseCritica)
        {
            using (var context = new BaseContext())
            {
                analiseCritica.Temas.ForEach(tema =>
                {
                    var temaCtx = context.Set<AnaliseCriticaTema>().Find(tema.IdTema);

                    if (temaCtx != null)
                    {
                        temaCtx.Descricao = tema.Descricao;
                        context.Entry(temaCtx).State = EntityState.Modified;
                    }
                    else
                    {
                        context.Set<AnaliseCriticaTema>().Add(tema);
                    }

                    

                });

                context.SaveChanges();
            }
        }

        public void SalvarAnaliseCritica(AnaliseCritica analiseCritica)
        {
            using (var context = new BaseContext())
            {
                int count = 0;
                analiseCritica.Temas.ForEach(tema =>
                {

					if (tema.GestaoDeRisco != null) {
						if (count == 0)
						{
							_registroConformidadesRepositorio.GerarNumeroSequencialPorSite(tema.GestaoDeRisco);
							count = tema.GestaoDeRisco.NuRegistro;

						}
						else
						{

							tema.GestaoDeRisco.NuRegistro = count;
						}

						if (tema.GestaoDeRisco.StatusEtapa == 4)
						{
							tema.GestaoDeRisco.DtEnceramento = DateTime.Now;
							tema.GestaoDeRisco.IdResponsavelImplementar = tema.GestaoDeRisco.IdEmissor;
							tema.GestaoDeRisco.IdResponsavelEtapa = tema.GestaoDeRisco.IdEmissor;
						}

					}
                    context.Set<AnaliseCriticaTema>().Add(tema);
                    count++;

                });

                context.Set<AnaliseCritica>().Add(analiseCritica);
                context.SaveChanges();
            }
        }
    }
}
