using Dominio.Entidade;
using System.Collections.Generic;

namespace Dominio.Interface.Servico
{
    public interface IProdutoServico 
    {
        List<Produto> ListaPorSite(int idSite);

        void ValidarCampos(Produto produto, ref List<string> erros);

        void ValidaNotasAvaliacao(Produto produto, ref List<string> erros);

        void AdicionarAvaliacaoCriticidade(int idProduto, List<int> listaAvaliacoesCriticidade);

        void AdicionarCriteriosQualificacao(int idProduto, List<int> listaCriteriosQualificacao);

        void AdicionarFornecedores(int idProduto, List<int> listaFornecedores);

    }
}
