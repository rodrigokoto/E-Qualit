using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class DocumentoCargoMap : EntityTypeConfiguration<DocumentoCargo>
    {
        public DocumentoCargoMap()
        {
            HasKey(t => t.Id);

            ToTable("DocumentoCargo");
   
            Property(t => t.Id)
                .HasColumnName("Id");

            Property(t => t.IdDocumento)
                .HasColumnName("IdDocumento");

            Property(t => t.IdCargo)
                .HasColumnName("IdCargo");

            HasRequired(t => t.DocDocumento)
                .WithMany(t => t.DocCargo)
                .HasForeignKey(d => d.IdDocumento)
                .WillCascadeOnDelete(true);
        }
    }
}
