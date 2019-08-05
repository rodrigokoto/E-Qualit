using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ArquivoDeEvidenciaAcaoImediataMap : EntityTypeConfiguration<ArquivoDeEvidenciaAcaoImediata>
    {
        public ArquivoDeEvidenciaAcaoImediataMap()
        {
            Ignore(x => x.ApagarAnexo);

            HasKey(x => x.IdArquivoDeEvidenciaAcaoImediata);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdAcaoImediata)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.AcaoImediata)
                .WithMany(x => x.ArquivoEvidencia)
                .HasForeignKey(s => s.IdAcaoImediata)
                .WillCascadeOnDelete(true);

            HasRequired(s => s.Anexo)
                .WithMany(s => s.ArquivoEvidenciaAcaoImediata)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
