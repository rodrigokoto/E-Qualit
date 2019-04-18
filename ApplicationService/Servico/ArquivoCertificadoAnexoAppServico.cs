using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class ArquivoCertificadoAnexoAppServico : BaseServico<ArquivoCertificadoAnexo>, IArquivoCertificadoAnexoAppServico
    {
        private readonly IArquivoCertificadoAnexoRepositorio _arquivoCertificadoAnexoRepositorio;

        public ArquivoCertificadoAnexoAppServico(IArquivoCertificadoAnexoRepositorio arquivoCertificadoAnexoRepositorio) 
            : base(arquivoCertificadoAnexoRepositorio)
        {
            _arquivoCertificadoAnexoRepositorio = arquivoCertificadoAnexoRepositorio;
        }
    }
}
