using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.Servico
{
    public class ProdutoFornecedorServico : IProdutoFornecedorServico
    {
        private IProdutoFornecedorRepositorio _produtoFornecedorRepositorio;

        public ProdutoFornecedorServico(IProdutoFornecedorRepositorio produtoFornecedorRepositorio) 
        {
            _produtoFornecedorRepositorio = produtoFornecedorRepositorio;
        }

        public List<Fornecedor> ObterFonecedorPorProduto(int idProduto) =>
            _produtoFornecedorRepositorio.Get(x => x.IdProduto == idProduto).Select(y=>y.Fornecedor).ToList();
        



    }
}
