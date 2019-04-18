using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;

namespace ApplicationService.Servico
{
    public class AnaliseCriticaFuncionarioAppServico : BaseServico<AnaliseCriticaFuncionario>, 
                                                        IAnaliseCriticaFuncionarioAppServico
    {
        private readonly IAnaliseCriticaFuncionarioRepositorio _analiseCriticaFuncionariorepositorio;

        public AnaliseCriticaFuncionarioAppServico(IAnaliseCriticaFuncionarioRepositorio analiseCriticaFuncionariorepositorio) : base(analiseCriticaFuncionariorepositorio)
        {
            _analiseCriticaFuncionariorepositorio = analiseCriticaFuncionariorepositorio;
        }
    }
}
