using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IFuncaoAppServico : IBaseServico<Funcao>
    {
        List<Funcao> ObterFuncoesPorFuncionalidade(int idModulo);
    }
}
