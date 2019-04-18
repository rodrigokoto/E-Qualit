using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IProdutoFornecedorServico
    {
        List<Fornecedor> ObterFonecedorPorProduto(int idProduto);
    }
}
