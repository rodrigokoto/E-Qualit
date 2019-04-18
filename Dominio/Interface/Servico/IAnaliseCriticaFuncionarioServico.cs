using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IAnaliseCriticaFuncionarioServico 
    {
        void Remove(List<AnaliseCriticaFuncionario> funcionarios);
    }
}
