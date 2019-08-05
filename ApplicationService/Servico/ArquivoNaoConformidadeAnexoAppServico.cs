using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class ArquivoNaoConformidadeAnexoAppServico : BaseServico<ArquivoNaoConformidadeAnexo>, IArquivoNaoConformidadeAnexoAppServico
    {
        private readonly IArquivoNaoConformidadeAnexoRepositorio _arquivoNaoConformidadeAnexoRepositorio;

        public ArquivoNaoConformidadeAnexoAppServico(IArquivoNaoConformidadeAnexoRepositorio arquivoNaoConformidadeAnexoRepositorio) 
            : base(arquivoNaoConformidadeAnexoRepositorio)
        {
            _arquivoNaoConformidadeAnexoRepositorio = arquivoNaoConformidadeAnexoRepositorio;
        }
    }
}
