using System.Collections.Generic;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System.Linq;

namespace ApplicationService.Servico
{
    public class ProdutoAppServico : BaseServico<Produto>, IProdutoAppServico
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoAppServico(IProdutoRepositorio produtoRepositorio) : base(produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public List<Produto> ListaPorSite(int idSite)
        {
            return _produtoRepositorio.Get(x => x.IdSite == idSite).ToList();
        }

        public void AdicionarCriteriosQualificacao(int idProduto, List<int> listaCriteriosQualificacao)
        {
            var produto = _produtoRepositorio.Get(x => x.IdProduto == idProduto).FirstOrDefault();
            var listaProdutoCriterioQualificacao = new List<CriterioQualificacao>();

            listaCriteriosQualificacao.ForEach(id =>
            {
                var novoProdutoCriterioQualificacao = new CriterioQualificacao
                {
                    IdProduto = produto.IdProduto,
                    IdCriterioQualificacao = id
                };
                listaProdutoCriterioQualificacao.Add(novoProdutoCriterioQualificacao);
            });

            produto.CriteriosQualificacao.ToList().AddRange(listaProdutoCriterioQualificacao);

            _produtoRepositorio.Update(produto);
        }

        public void AdicionarAvaliacaoCriticidade(int idProduto, List<int> listaAvaliacoesCriticidade)
        {
            var produto = _produtoRepositorio.Get(x => x.IdProduto == idProduto).FirstOrDefault();
            var listaProdutoAvaliacaoCriticidade = new List<AvaliacaoCriticidade>();

            listaAvaliacoesCriticidade.ForEach(id =>
            {
                var novoProdutoCriterioQualificacao = new AvaliacaoCriticidade
                {
                    IdProduto = produto.IdProduto,
                    IdAvaliacaoCriticidade = id
                };
                listaProdutoAvaliacaoCriticidade.Add(novoProdutoCriterioQualificacao);
            });

            produto.AvaliacoesCriticidade.ToList().AddRange(listaProdutoAvaliacaoCriticidade);

            _produtoRepositorio.Update(produto);
        }

        public void AdicionarFornecedores(int idProduto, List<int> listaFornecedores)
        {
            var produto = _produtoRepositorio.Get(x => x.IdProduto == idProduto).FirstOrDefault();
            var listaFornecedor = new List<ProdutoFornecedor>();

            listaFornecedores.ForEach(id =>
            {
                var novoProdutoCriterioQualificacao = new ProdutoFornecedor
                {
                    IdProduto = produto.IdProduto,
                    IdFornecedor = id
                };
                listaFornecedor.Add(novoProdutoCriterioQualificacao);
            });

            produto.Fornecedores.ToList().AddRange(listaFornecedor);

            _produtoRepositorio.Update(produto);
        }

        public bool Excluir(int idProduto)
        {
           return _produtoRepositorio.Excluir(idProduto);
        }
    }
}
