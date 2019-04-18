using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;

namespace ApplicationService.Servico
{
    public class ControleImpressaoAppServico : BaseServico<ControleImpressao>, IControleImpressaoAppServico
    {
        private readonly IControleImpressaoRepositorio _controleImpressaoRepositorio;

        public ControleImpressaoAppServico(IControleImpressaoRepositorio controleImpressao) : base(controleImpressao)
        {
            _controleImpressaoRepositorio = controleImpressao;
        }

    }
}
