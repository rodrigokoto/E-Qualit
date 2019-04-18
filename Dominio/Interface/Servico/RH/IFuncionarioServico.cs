using Dominio.Entidade.RH;
using System.Collections.Generic;

namespace Dominio.Interface.Servico.RH
{
    public interface IFuncionarioServico 
    {
        void Valido(Funcionario funcionario, ref List<string> erros);
    }
}
