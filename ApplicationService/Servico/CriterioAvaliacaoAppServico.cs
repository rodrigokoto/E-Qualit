using Dominio.Entidade;
using ApplicationService.Interface;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class CriterioAvaliacaoAppServico : BaseServico<CriterioAvaliacao>, ICriterioAvaliacaoAppServico
    {
        private ICriterioAvaliacaoRepositorio _criterioAvaliacaoRepositorio;

        public CriterioAvaliacaoAppServico(ICriterioAvaliacaoRepositorio criterioAvaliacaoRepositorio) : base(criterioAvaliacaoRepositorio)
        {
            _criterioAvaliacaoRepositorio = criterioAvaliacaoRepositorio;
        }

    }
}
