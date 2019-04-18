using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;

namespace ApplicationService.Servico
{
    public class AnaliseCriticaTemaAppServico : BaseServico<AnaliseCriticaTema>, 
                                                IAnaliseCriticaTemaAppServico
    {
        private readonly IAnaliseCriticaTemaRepositorio _analiseCriticaTemaRepositorio;
        private readonly ILogRepositorio _logRepositorio;

        public AnaliseCriticaTemaAppServico(IAnaliseCriticaTemaRepositorio analiseCriticaTemaRepositorio,
                                         ILogRepositorio logRepositorio) : base(analiseCriticaTemaRepositorio)
        {
            _analiseCriticaTemaRepositorio = analiseCriticaTemaRepositorio;
            _logRepositorio = logRepositorio;
        }
    }
}
