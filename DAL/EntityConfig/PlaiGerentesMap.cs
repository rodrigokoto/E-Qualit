using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class PlaiGerentesMap : EntityTypeConfiguration<PlaiGerentes>
    {
        public PlaiGerentesMap()
        {
            ToTable("PlaiGerentes");

            HasKey(x => x.IdPlaiGerente);

            Property(x => x.IdUsuario)
                    .IsRequired()
                    .HasColumnName("IdUsuario");

            Property(x => x.IdPlai)
                    .IsRequired()
                    .HasColumnName("IdPlai");

            HasRequired(x => x.Plai)
            .WithMany(x => x.PlaiGerentes)
            .HasForeignKey(x => x.IdPlai)
            .WillCascadeOnDelete(true);

            HasRequired(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.IdPlai);
            
        }
    }
}
