using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ProdutoMap : EntityTypeConfiguration<Produto>
    {
        public ProdutoMap()
        {
            ToTable("Produto");

            Ignore(x => x.ValidationResult);

            HasKey(t => t.IdProduto);

            Property(t => t.IdProduto)
                .HasColumnName("IdProduto");

            Property(x => x.Nome)
               .HasColumnName("Nome")
               .IsRequired();

            Property(x => x.Especificacao)
               .HasColumnName("Especificacao")
               .IsOptional();

            Property(x => x.Tags)
               .HasColumnName("Tags")
               .IsOptional();

            Property(x => x.DtCriacao)
               .HasColumnName("DtCriacao")
               .IsRequired();

            Property(x => x.DtAlteracao)
               .HasColumnName("DtAlteracao")
               .IsRequired();

            Property(x => x.IdSite)
               .HasColumnName("IdSite")
               .IsRequired();

            Property(x => x.IdResponsavel)
               .HasColumnName("IdResponsavel")
               .IsRequired();

            #region Relacionamentos

            HasRequired(t => t.Site)
               .WithMany(t => t.Produtos)
               .HasForeignKey(d => d.IdSite);

            HasRequired(t => t.Responsavel)
                .WithMany(t => t.Produtos)
                .HasForeignKey(d => d.IdResponsavel);

            #endregion

        }
    }
}
