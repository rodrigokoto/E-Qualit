using Dominio.Entidade;
using System.Collections.Generic;

namespace ApplicationService.Interface
{
    public interface IProdutoAppServico : IBaseServico<Produto>
    {
        List<Produto> ListaPorSite(int idSite);

        void AdicionarAvaliacaoCriticidade(int idProduto, List<int> listaAvaliacoesCriticidade);

        void AdicionarCriteriosQualificacao(int idProduto, List<int> listaCriteriosQualificacao);

        void AdicionarFornecedores(int idProduto, List<int> listaFornecedores);

        bool Excluir(int idProduto);

    }
}
