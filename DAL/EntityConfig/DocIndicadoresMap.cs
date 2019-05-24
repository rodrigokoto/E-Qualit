using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class DocIndicadoresMap : EntityTypeConfiguration<DocIndicadores>
    {
        public DocIndicadoresMap()
        {
            HasKey(x => x.IdIndicadores);

            //Property(x => x.ResponsavelNomeCompleto)
            //    .IsOptional();

            //Property(x => x.OQue)
            //    .IsRequired();

            //Property(x => x.Quem)
            //    .IsRequired();

            //Property(x => x.Registro)
            //    .IsRequired();

            //Property(x => x.Como)
            //    .IsRequired();

            //Property(x => x.Item)
            //    .IsRequired();

            HasRequired(x => x.Documento)
                .WithMany(z => z.Indicadores)
                .HasForeignKey(y => y.IdDocumento)
                .WillCascadeOnDelete(true);

        }
    }
}
