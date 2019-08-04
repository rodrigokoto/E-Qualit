using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ArquivoDocDocumentoAnexoMap : EntityTypeConfiguration<ArquivoDocDocumentoAnexo>
    {
        public ArquivoDocDocumentoAnexoMap()
        {
            Ignore(x => x.ApagarAnexo);

            ToTable("ArquivoDocDocumentoAnexo");

            HasKey(x => x.IdArquivoControleDeDocumentoAnexo);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdControleDeDocumento)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.DocDocumento)
                .WithMany(s => s.ArquivoDocDocumentoAnexo)
                .HasForeignKey(s => s.IdControleDeDocumento)
                .WillCascadeOnDelete(false);

            HasRequired(s => s.Anexo)
                .WithMany(s => s.ArquivoDocDocumentoAnexo)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
