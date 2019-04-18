using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class DocRegistroMap : EntityTypeConfiguration<DocRegistro>
    {
        public DocRegistroMap()
        {
            HasKey(x => x.IdDocRegistro);

            Property(x => x.IdDocumento)
                .IsRequired();

            Property(x=>x.Identificar)
                .IsRequired();

            Property(x=>x.Armazenar)
                .IsRequired();

            Property(x=>x.Proteger)
                .IsRequired();

            Property(x=>x.Retencao)
                .IsRequired();

            Property(x=>x.Recuperar)
                .IsRequired();

            Property(x=>x.Disposicao)
                .IsRequired();

            HasRequired(x => x.DocDocumento)
                .WithMany(z => z.Registros)
                .HasForeignKey(y => y.IdDocumento)
                .WillCascadeOnDelete(true);

        }
    }
}
