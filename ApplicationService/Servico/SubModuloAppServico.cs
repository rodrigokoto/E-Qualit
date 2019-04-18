using ApplicationService.Interface;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;

namespace ApplicationService.Servico
{
    public class SubModuloAppServico : BaseServico<SubModulo>, ISubModuloAppServico
    {
        public SubModuloAppServico(ISubModuloRepositorio subModuloRepositorio) : base(subModuloRepositorio)
        {

        }
    }
}
