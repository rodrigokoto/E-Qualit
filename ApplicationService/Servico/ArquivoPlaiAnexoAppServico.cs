using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class ArquivoPlaiAnexoAppServico : BaseServico<ArquivoPlaiAnexo>, IArquivoPlaiAnexoAppServico
    {
        private readonly IArquivoPlaiAnexoRepositorio _arquivoPlaiAnexoRepositorio;

        public ArquivoPlaiAnexoAppServico(IArquivoPlaiAnexoRepositorio arquivoPlaiAnexoRepositorio) 
            : base(arquivoPlaiAnexoRepositorio)
        {
            _arquivoPlaiAnexoRepositorio = arquivoPlaiAnexoRepositorio;
        }
    }
}
