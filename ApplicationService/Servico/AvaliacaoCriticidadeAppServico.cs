using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;

namespace ApplicationService.Servico
{
    public class AvaliacaoCriticidadeAppServico : BaseServico<AvaliacaoCriticidade>, IAvaliacaoCriticidadeAppServico
    {
        private IAvaliacaoCriticidadeRepositorio _avaliacaoDeCriticidadeRepositorio;

        public AvaliacaoCriticidadeAppServico(IAvaliacaoCriticidadeRepositorio avaliacaoDeCriticidadeRepositorio) : base(avaliacaoDeCriticidadeRepositorio)
        {
            _avaliacaoDeCriticidadeRepositorio = avaliacaoDeCriticidadeRepositorio;
        }

    }
}
