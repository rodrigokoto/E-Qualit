using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationService.Servico
{
    public class ProdutoFornecedorAppServico : BaseServico<ProdutoFornecedor>, IProdutoFornecedorAppServico
    {
        private IProdutoFornecedorRepositorio _produtoFornecedorRepositorio;

        public ProdutoFornecedorAppServico(IProdutoFornecedorRepositorio produtoFornecedorRepositorio) : base(produtoFornecedorRepositorio)
        {
            _produtoFornecedorRepositorio = produtoFornecedorRepositorio;
        }

        public List<Fornecedor> ObterFonecedorPorProduto(int idProduto) =>
            _produtoFornecedorRepositorio.Get(x => x.IdProduto == idProduto).Select(y => y.Fornecedor).ToList();

    }
}
