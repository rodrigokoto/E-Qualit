using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class ArquivoDeEvidenciaAcaoImediataAppServico : BaseServico<ArquivoDeEvidenciaAcaoImediata>, IArquivoDeEvidenciaAcaoImediataAppServico
    {
        private readonly IArquivoDeEvidenciaAcaoImediataRepositorio _arquivoDeEvidenciaAcaoImediataRepositorio;

        public ArquivoDeEvidenciaAcaoImediataAppServico(IArquivoDeEvidenciaAcaoImediataRepositorio arquivoDeEvidenciaAcaoImediataRepositorio) 
            : base(arquivoDeEvidenciaAcaoImediataRepositorio)
        {
            _arquivoDeEvidenciaAcaoImediataRepositorio = arquivoDeEvidenciaAcaoImediataRepositorio;
        }
    }
}
