using Dominio.Entidade;

namespace Dominio.Validacao.Produtos.View
{
    public class CriarProdutoViewValidation : ValidaCampoProduto<Produto>
    {
        public CriarProdutoViewValidation()
        {
            NomeObrigatorio();
            ResponsavelObrigatorio();
            SiteObrigatorio();
            TamanhoMaximoEspecificacao();
            TamanhoMaximoCodigo();
            //StatusNaoCritico();
        }
    }
}
