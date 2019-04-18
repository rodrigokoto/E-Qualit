using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class DocTemplateMap : EntityTypeConfiguration<Dominio.Entidade.DocTemplate>
    {
        public DocTemplateMap()
        {
            // Primary Key
            HasKey(t => t.IdDocTemplate);

            // Properties
            Property(t => t.TpTemplate)
                .HasMaxLength(2);

            // Table & Column Mappings
            ToTable("DocTemplate");
            Property(t => t.IdDocTemplate).HasColumnName("IdDocTemplate");
            Property(t => t.IdDocumento).HasColumnName("IdDocumento");
            Property(t => t.TpTemplate).HasColumnName("TpTemplate");
            Property(t => t.Ativo).HasColumnName("ATIVO");

            // Relationships
            HasRequired(t => t.DocDocumento)
                .WithMany(t => t.DocTemplate)
                .HasForeignKey(d => d.IdDocumento)
                .WillCascadeOnDelete(true);

        }
    }
}
