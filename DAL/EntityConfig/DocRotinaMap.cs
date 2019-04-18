using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class DocRotinaMap : EntityTypeConfiguration<DocRotina>
    {
        public DocRotinaMap()
        {
            HasKey(x => x.IdDocRotina);

            Property(x => x.IdDocumento)
                .IsRequired();

            Property(x => x.OQue)
                .IsRequired();

            Property(x => x.Quem)
                .IsRequired();

            Property(x => x.Registro)
                .IsRequired();

            Property(x => x.Como)
                .IsRequired();

            Property(x => x.Item)
                .IsRequired();

            HasRequired(x => x.DocDocumento)
                  .WithMany(y => y.Rotinas)
                  .HasForeignKey(z => z.IdDocumento)
                  .WillCascadeOnDelete(true);

        }
    }
}
