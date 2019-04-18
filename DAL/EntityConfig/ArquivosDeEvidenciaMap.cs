using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ArquivosDeEvidenciaMap : EntityTypeConfiguration<ArquivosDeEvidencia>
    {
        public ArquivosDeEvidenciaMap()
        {
            ToTable("ArquivosEvidencia");

            HasKey(x => x.IdArquivosDeEvidencia);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdRegistroConformidade)
                .IsRequired();

            Property(x => x.TipoRegistro)
                .HasMaxLength(2)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.Anexo)
                .WithMany(x => x.ArquivosDeEvidencia)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            HasRequired(s => s.RegistroConformidade)
                .WithMany(s => s.ArquivosDeEvidencia)
                .HasForeignKey(s => s.IdRegistroConformidade)
                .WillCascadeOnDelete(true);

            #endregion

        }
    }
}
