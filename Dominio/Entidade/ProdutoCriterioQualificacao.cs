namespace Dominio.Entidade
{
    public class ProdutoCriterioQualificacao
    {
        public int IdProdutoCriterioQualificacao { get; set; }
        public int IdProduto { get; set; }
        public int IdCriterioQualificacao { get; set; }

        #region Relacionamentos

        public virtual Produto Produto { get; set; }

        public virtual CriterioQualificacao CriterioQualificacao { get; set; }

        #endregion
    }
}
