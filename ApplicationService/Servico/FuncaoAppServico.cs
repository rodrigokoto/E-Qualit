using Dominio.Entidade;
using System.Collections.Generic;
using System.Linq;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;

namespace ApplicationService.Servico
{
    public class FuncaoAppServico : BaseServico<Funcao>, IFuncaoAppServico
    {
        private readonly IFuncaoRepositorio _funcaoRepositorio;

        public FuncaoAppServico(IFuncaoRepositorio funcaoRepositorio) :base(funcaoRepositorio)
        {
            _funcaoRepositorio = funcaoRepositorio;
        }

        public List<Funcao> ObterFuncoesPorFuncionalidade(int idModulo) =>
            _funcaoRepositorio.Get(x => x.IdFuncionalidade == idModulo).ToList();
    }
}
