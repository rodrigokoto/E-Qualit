using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ProdutoFornecedorMap: EntityTypeConfiguration<ProdutoFornecedor>
    {
        public ProdutoFornecedorMap()
        {
            ToTable("ProdutoFornecedor");

            HasKey(x => x.IdProdutoFornecedor);

            Property(x => x.IdFornecedor)
                .HasColumnName("IdFornecedor")
                .IsRequired();

            Property(x => x.IdProduto)
                .HasColumnName("IdProduto")
                .IsRequired();

            #region Relacionamentos

            HasRequired(x => x.Produto)
               .WithMany(x => x.Fornecedores)
               .HasForeignKey(x => x.IdProduto)
               .WillCascadeOnDelete(true);

            HasRequired(x => x.Fornecedor)
              .WithMany(x => x.Produtos)
              .HasForeignKey(x => x.IdFornecedor)
              .WillCascadeOnDelete(true);


            #endregion

        }
    }
}
