using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class ArquivoLicencaAnexoAppServico : BaseServico<ArquivoLicencaAnexo>, IArquivoLicencaAnexoAppServico
    {
        private readonly IArquivoLicencaAnexoRepositorio _arquivoLicencaAnexoRepositorio;

        public ArquivoLicencaAnexoAppServico(IArquivoLicencaAnexoRepositorio arquivoLicencaAnexoRepositorio) 
            : base(arquivoLicencaAnexoRepositorio)
        {
            _arquivoLicencaAnexoRepositorio = arquivoLicencaAnexoRepositorio;
        }
    }
}
