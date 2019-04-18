using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class ArquivosEvidenciaCriterioQualificacaoAppServico : BaseServico<ArquivosEvidenciaCriterioQualificacao>, IArquivosEvidenciaCriterioQualificacaoAppServico
    {
        private readonly IArquivosEvidenciaCriterioQualificacaoRepositorio _ArquivosEvidenciaCriterioQualificacaoRepositorio;

        public ArquivosEvidenciaCriterioQualificacaoAppServico(IArquivosEvidenciaCriterioQualificacaoRepositorio arquivosEvidenciaCriterioQualificacaoRepositorio) 
            : base(arquivosEvidenciaCriterioQualificacaoRepositorio)
        {
            _ArquivosEvidenciaCriterioQualificacaoRepositorio = arquivosEvidenciaCriterioQualificacaoRepositorio;
        }
    }
}
