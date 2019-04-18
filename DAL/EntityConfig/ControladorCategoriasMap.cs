using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ControladorCategoriasMap : EntityTypeConfiguration<Dominio.Entidade.ControladorCategoria>
    {
        public ControladorCategoriasMap()
        {
            Ignore(x => x.ValidationResult);

            HasKey(t => t.IdControladorCategorias);

            Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(200);

            Property(t => t.TipoTabela)
                .IsRequired()
                .HasMaxLength(10);

            ToTable("Cadastro");
            Property(t => t.IdControladorCategorias).HasColumnName("IdCadastro");
            Property(t => t.IdSite).HasColumnName("IdSite");
            Property(t => t.Descricao).HasColumnName("DsDescricao");
            Property(t => t.TipoTabela).HasColumnName("CdTabela");
            Property(t => t.Ativo).HasColumnName("FlAtivo");

            HasRequired(t => t.Site)
                .WithMany(t => t.ControladorCategorias)
                .HasForeignKey(d => d.IdSite);

        }
    }
}
