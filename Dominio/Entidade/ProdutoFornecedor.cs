namespace Dominio.Entidade
{
    public class ProdutoFornecedor
    {
        public int IdProdutoFornecedor { get; set; }
        public int IdProduto { get; set; }
        public int IdFornecedor { get; set; }

        #region Relacionamentos

        public virtual Produto Produto { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        #endregion
    }
}
