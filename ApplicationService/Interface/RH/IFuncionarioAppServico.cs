using Dominio.Entidade.RH;
using System.Collections.Generic;

namespace ApplicationService.Interface.RH
{
    public interface IFuncionarioAppServico : IBaseServico<Funcionario>
    {
        void Valido(Funcionario funcionario, ref List<string> erros);
    }
}
