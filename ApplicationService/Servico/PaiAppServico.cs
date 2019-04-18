using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Linq;

namespace ApplicationService.Servico
{
    public class PaiAppServico : BaseServico<Pai>, IPaiAppServico
    {
        private readonly IPaiRepositorio _paiRepositorio;
        private readonly IPlaiRepositorio _plaiRepositorio;

        public PaiAppServico(IPaiRepositorio paiRepositorio, IPlaiRepositorio plaiRepositorio) : base(paiRepositorio)
        {
            _paiRepositorio = paiRepositorio;
            _plaiRepositorio = plaiRepositorio;
        }
        
        public Pai ObterPorAno(int? ano)
        {
            return _paiRepositorio.Get(x => x.Ano == ano).FirstOrDefault();
        }
    }
}
