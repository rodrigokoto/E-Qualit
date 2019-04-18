using System.Collections.Generic;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using Dominio.Interface.Servico;
using System.Linq;
using Dominio.Validacao.Produtos.View;

namespace Dominio.Servico
{
    public class ProdutoServico : IProdutoServico
    {
        private IProdutoRepositorio _produtoRepositorio;
        private ICriterioQualificacaoRepositorio _criterioQualificacaoRepositorio;
        private CriterioQualificacaoServico _criterioQualificacaoServico;

        private IAvaliaCriterioAvaliacaoRepositorio _avaliaCriterioAvaliacaoRepositorio;
        private readonly IHistoricoCriterioAvaliacaoRepositorio _historicoCriterioAvaliacaoRepositorio;
        private AvaliaCriterioAvaliacaoServico _avaliaCriterioAvaliacaoServico;

        public ProdutoServico(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _criterioQualificacaoServico = new CriterioQualificacaoServico(_criterioQualificacaoRepositorio);

            _avaliaCriterioAvaliacaoServico = new AvaliaCriterioAvaliacaoServico(_avaliaCriterioAvaliacaoRepositorio, _historicoCriterioAvaliacaoRepositorio);
        }

        public List<Produto> ListaPorSite(int idSite)
        {
            return _produtoRepositorio.Get(x => x.IdSite == idSite).ToList();
        }

        public void AdicionarCriteriosQualificacao(int idProduto, List<int> listaCriteriosQualificacao)
        {
            var produto = _produtoRepositorio.Get(x => x.IdProduto == idProduto).FirstOrDefault();
            var listaProdutoCriterioQualificacao = new List<CriterioQualificacao>();

            listaCriteriosQualificacao.ForEach(id => {
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

            listaAvaliacoesCriticidade.ForEach(id => {
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

            listaFornecedores.ForEach(id => {
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
    
        public void ValidarCampos(Produto produto, ref List<string> erros)
        {
            var validarCampos = new CriarProdutoViewValidation().Validate(produto);
            if (!validarCampos.IsValid)
            {
                erros.AddRange(UtilsServico.PopularErros(validarCampos.Errors));
            }
        }
        
        public void ValidaCriteriosQualificacao(Produto produto, ref List<string> erros)
        {
            var errosAux = new List<string>();
            produto.CriteriosQualificacao.ToList().ForEach(x => {
                _criterioQualificacaoServico.ValidaCampos(x, ref errosAux);
            });
            erros.AddRange(errosAux);
        }

        public void ValidaNotasAvaliacao(Produto produto, ref List<string> erros)
        {
            var erroAux = new List<string>();

            produto.CriteriosAvaliacao.ToList().ForEach(x=> {
                x.Avaliacoes.ToList().ForEach(y => {
                    _avaliaCriterioAvaliacaoServico.ValidaAvaliaCriterioAvaliacao(y, ref erroAux);
                });
            });
            erros.AddRange(erroAux);           
        }
    }
}
