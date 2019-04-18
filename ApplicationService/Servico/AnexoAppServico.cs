using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class AnexoAppServico : BaseServico<Anexo>, IAnexoAppServico
    {
        private readonly IAnexoRepositorio _anexoRepositorio;

        public AnexoAppServico(IAnexoRepositorio anexoRepositorio) : base(anexoRepositorio)
        {
            _anexoRepositorio = anexoRepositorio;
        }
    }
}
