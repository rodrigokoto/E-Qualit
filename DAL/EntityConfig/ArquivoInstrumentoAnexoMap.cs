using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ArquivoInstrumentoAnexoMap : EntityTypeConfiguration<ArquivoInstrumentoAnexo>
    {
        public ArquivoInstrumentoAnexoMap()
        {
            Ignore(x => x.ApagarAnexo);

            ToTable("ArquivoInstrumentoAnexo");

            HasKey(x => x.IdArquivoInstrumentoAnexo);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdInstrumento)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.Instrumento)
                .WithMany(s => s.ArquivoInstrumento)
                .HasForeignKey(s => s.IdInstrumento)
                .WillCascadeOnDelete(false);

            HasRequired(s => s.Anexo)
                .WithMany(s => s.ArquivoInstrumentoAnexo)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
