using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class FornecedorMap : EntityTypeConfiguration<Fornecedor>
    {
        public FornecedorMap() 
        {
            ToTable("Fornecedor");

            HasKey(t => t.IdFornecedor);

            Property(t => t.Nome)
                .HasColumnName("Nome")
                .IsRequired();

            Property(t => t.Email)
                .HasColumnName("Email");


            Property(t => t.Telefone)
                .HasColumnName("Telefone");
            

            Property(t => t.IdSite)
                .HasColumnName("IdSite")
                .IsRequired();

            Property(t => t.IdProcesso)
                .HasColumnName("IdProcesso");

            Property(t => t.IdUsuarioAvaliacao)
                .HasColumnName("IdUsuarioAvaliacao");

            Property(t => t.TipoFornecedor)
                .HasColumnName("TpFornecedor");

            #region Relacionamentos

            HasRequired(t => t.Site)
              .WithMany(t => t.Fornecedores)
              .HasForeignKey(d => d.IdSite);

            HasRequired(t => t.Processo)
              .WithMany(t => t.Fornecedores)
              .HasForeignKey(d => d.IdProcesso);

            HasRequired(t => t.UsuarioAvaliacao)
              .WithMany(t => t.UsuarioAvaliacaoFornecedor)
              .HasForeignKey(d => d.IdUsuarioAvaliacao);

            #endregion

        }
    }
}
