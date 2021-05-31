using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;

namespace ApplicationService.Servico
{
    public class RelatorioAppServico : BaseServico<Relatorio>, IRelatorioAppServico
    {
        private readonly IRelatorioRepositorio _relatorioRepositorio;

        public RelatorioAppServico(IRelatorioRepositorio relatorioRepositorio) : base(relatorioRepositorio)
        {
            _relatorioRepositorio = relatorioRepositorio;
        }
    }
}
