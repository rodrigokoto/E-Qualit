using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class DocumentoAssuntoMap : EntityTypeConfiguration<DocumentoAssunto>
    {
        public DocumentoAssuntoMap()
        {
            ToTable("DocumentoAssunto");

            HasKey(x => x.Id);

            Property(x => x.IdDocumento)
                .IsRequired()
                .HasColumnName("IdDocumento");

            Property(x => x.Descricao)
                .IsRequired()
                .HasColumnName("Descricao");

            Property(x => x.DataAssunto)
                .IsRequired()
                .HasColumnName("DataAssunto");

            Property(x => x.Revisao)
                .IsRequired()
                .HasColumnName("Revisao");

            HasRequired(t => t.Documento)
                .WithMany(t => t.Assuntos)
                .HasForeignKey(d => d.IdDocumento)
                .WillCascadeOnDelete(true);
        }
    }
}
