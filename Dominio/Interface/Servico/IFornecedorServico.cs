using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IFornecedorServico 
    {
        List<Fornecedor> ObterPorSite(int idSite);
        void ValidaCampos(Fornecedor fornecedor, ref List<string> erros);
    }
}
