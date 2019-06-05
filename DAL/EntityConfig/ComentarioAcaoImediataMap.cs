using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ComentarioAcaoImediataMap : EntityTypeConfiguration<ComentarioAcaoImediata>
    {
        public ComentarioAcaoImediataMap()
        {

            ToTable("ComentarioAcaoImediata");

            HasKey(x => x.IdComentarioAcaoImediata);


            //Property(t => t.IdComentarioAcaoImediata)
            //    .HasColumnName("IdComentarioAcaoImediata");


            //Property(x => x.IndicadoresMeta).HasPrecision(18, 3);

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


            //HasRequired(t => t.Registro)
            //    .WithMany(t => t.IdRegistroConformidade )
            //    .HasForeignKey(d => d.IdRegistroAcaoImediata)
            //    .WillCascadeOnDelete(true);

            Property(x => x.IdAcaoImediata)
                .HasColumnName("IdRegistroAcaoImediata");


            HasRequired(x => x.RegistroAcaoImediata)
                .WithMany(z => z.ComentariosAcaoImediata)
                .HasForeignKey(y => y.IdAcaoImediata)
                .WillCascadeOnDelete(true);

        }
    }
}
