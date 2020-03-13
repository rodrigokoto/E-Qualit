using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using Dominio.Validacao.Indicadores;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class IndicadorServico : IIndicadorServico
    {
        private readonly IIndicadorRepostorio _indicadorRepositorio;

        public IndicadorServico(IIndicadorRepostorio indicadorRepositorio)
        {
            _indicadorRepositorio = indicadorRepositorio;
        }

        public void Valido(Indicador indicador, ref List<string> erros)
        {
            var validacao = new AptoParaCadastroValidation().Validate(indicador);


            foreach (var periodicidade in indicador.PeriodicidadeDeAnalises)
            {
                var per = periodicidade.MetasRealizadas.Where(x => x.Realizado > 0).ToList();

                per = per.Where(x => x.Analise == null).ToList();
                if (per.Count > 0)
                {
                    if (per.Where(x => x.GestaoDeRisco != null).ToList().Count > 0)
                    {
                        erros.Add("Preencha a descrição da análise de resultado.");
                    }
                }
            }

            if (!validacao.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validacao.Errors));
            }
        }

        public IEnumerable<Indicador> IndicadoresPorProcessoESite(int idProcesso, int idSite)
        {
            return _indicadorRepositorio.Get(x => x.IdProcesso == idProcesso && x.IdSite == idSite);
        }

        public IEnumerable<Indicador> IndicadoresPorProcessoESiteEIndicador(int idProcesso, int id)
        {
            return _indicadorRepositorio.Get(x => x.IdProcesso == idProcesso && x.Id == id);
        }

        public void BateuAMeta(string seta, List<Meta> metas, List<PlanoVoo> realizado)
        {
            metas.ForEach(x =>
            {

                var planodeVooRealizado = realizado.Where(r => r.IdPeriodicidadeAnalise == x.IdPeriodicidadeAnalise).FirstOrDefault();

                if (planodeVooRealizado != null)
                {
                    if (seta == "Cima")
                    {
                        realizado.Where(r => r.IdPeriodicidadeAnalise == x.IdPeriodicidadeAnalise).FirstOrDefault().AtingiuAMeta =
                        (planodeVooRealizado.Realizado > x.Valor);
                    }
                    else
                    {
                        realizado.Where(r => r.IdPeriodicidadeAnalise == x.IdPeriodicidadeAnalise).FirstOrDefault().AtingiuAMeta =
                        (planodeVooRealizado.Realizado < x.Valor);
                    }
                }
            });
        }
    }
}
