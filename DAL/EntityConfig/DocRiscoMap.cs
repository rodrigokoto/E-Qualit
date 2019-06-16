using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class DocRiscoMap : EntityTypeConfiguration<DocRisco>
    {
        public DocRiscoMap()
        {
            HasKey(x => x.IdDocRisco);

            Property(x => x.DescricaoRegistro)
    .HasColumnName("DsOqueTexto");

            HasRequired(x => x.Documento)
                .WithMany(z => z.DocRisco)
                .HasForeignKey(y => y.IdDocumento)
                .WillCascadeOnDelete(true);

        }
    }
}
