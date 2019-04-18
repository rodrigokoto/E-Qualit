using ApplicationService.Interface.RH;
using Dominio.Entidade.RH;
using Dominio.Interface.Repositorio.RH;

namespace ApplicationService.Servico.RH
{
    public class EpiAppServico : BaseServico<EPI>, IEpiAppServico
    {
        
        public EpiAppServico(IEpiRepositorio repositorio) : base(repositorio)
        {

        }
    }
}
