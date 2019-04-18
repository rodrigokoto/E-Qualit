using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationService.Servico
{
    public class IndicadorAppServico : BaseServico<Indicador>, IIndicadorAppServico
    {
        private readonly IIndicadorRepostorio _indicadorRepositorio;

        public IndicadorAppServico(IIndicadorRepostorio indicadorRepositorio) : base(indicadorRepositorio)
        {
            _indicadorRepositorio = indicadorRepositorio;
        }

        public IEnumerable<Indicador> IndicadoresPorProcessoESite(int idSite, int ? idProcesso = null)
        {
            return _indicadorRepositorio.Get(x => (x.IdProcesso == idProcesso || idProcesso == null) && x.IdSite == idSite);
        }

        public IEnumerable<Indicador> IndicadoresPorProcessoESiteEIndicador(int id, int? idProcesso = null)
        {
            return _indicadorRepositorio.Get(x => (x.IdProcesso == idProcesso || idProcesso == null) && x.Id == id);
        }

        public void BateuAMeta(string seta,List<Meta> metas, List<PlanoVoo> realizado)
        {
            metas.ForEach(x => {

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

        public void Atualizar(Indicador indicador)
        {
            _indicadorRepositorio.Atualizar(indicador);
        }
    }
}
