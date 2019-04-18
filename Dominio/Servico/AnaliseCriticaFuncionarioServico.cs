using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System.Collections.Generic;

namespace Dominio.Servico
{
    public class AnaliseCriticaFuncionarioServico : IAnaliseCriticaFuncionarioServico
    {
        private readonly IAnaliseCriticaFuncionarioRepositorio _analiseCriticaFuncionariorepositorio;

        public AnaliseCriticaFuncionarioServico(IAnaliseCriticaFuncionarioRepositorio analiseCriticaFuncionariorepositorio)
        {
            _analiseCriticaFuncionariorepositorio = analiseCriticaFuncionariorepositorio;
        }
         
        public void Remove(List<AnaliseCriticaFuncionario> funcionarios)
        {

            foreach (var funcionario in funcionarios)
            {
                _analiseCriticaFuncionariorepositorio.Remove(funcionario);
            }

        }

    }
}
