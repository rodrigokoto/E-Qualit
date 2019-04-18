using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IProdutoFornecedorAppServico: IBaseServico<ProdutoFornecedor>
    {
        List<Fornecedor> ObterFonecedorPorProduto(int idProduto);
    }
}
