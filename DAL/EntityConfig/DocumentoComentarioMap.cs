using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class DocumentoComentarioMap : EntityTypeConfiguration<DocumentoComentario>
    {
        public DocumentoComentarioMap()
        {
            ToTable("DocumentoComentario");

            HasKey(x => x.Id);

            Property(x => x.IdUsuario)
                .IsRequired()
                .HasColumnName("IdUsuario");

            Property(x => x.IdDocumento)
                .IsRequired()
                .HasColumnName("IdDocumento");

            Property(x => x.Descricao)
                .IsRequired()
                .HasColumnName("Descricao");

            Property(x => x.DataComentario)
                .IsRequired()
                .HasColumnName("DataComentario");

            HasRequired(t => t.Documento)
                .WithMany(t => t.Comentarios)
                .HasForeignKey(d => d.IdDocumento)
                .WillCascadeOnDelete(true);
        }
    }
}
