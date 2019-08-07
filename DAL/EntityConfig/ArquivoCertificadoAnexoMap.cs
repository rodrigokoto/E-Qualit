using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ArquivoCertificadoAnexoMap : EntityTypeConfiguration<ArquivoCertificadoAnexo>
    {
        public ArquivoCertificadoAnexoMap()
        {
            HasKey(x => x.IdArquivoCertificadoAnexo);

            Ignore(x => x.ApagarAnexo);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdCalibracao)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.Calibracao)
                .WithMany(x => x.ArquivoCertificado)
                .HasForeignKey(s => s.IdCalibracao)
                .WillCascadeOnDelete(true);

            HasRequired(s => s.Anexo)
                .WithMany(s => s.ArquivoCertificadoAnexo)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
