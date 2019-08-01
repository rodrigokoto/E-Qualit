using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ArquivoControleDeDocumentoAnexoMap : EntityTypeConfiguration<ArquivoControleDeDocumentoAnexo>
    {
        public ArquivoControleDeDocumentoAnexoMap()
        {
            Ignore(x => x.ApagarAnexo);

            ToTable("ArquivoPlaiAnexo");

            HasKey(x => x.IdArquivooControleDeDocumentoAnexo);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdoControleDeDocumento)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.DocDocumento)
                .WithMany(s => s.arqui)
                .HasForeignKey(s => s.IdPlai)
                .WillCascadeOnDelete(false);

            HasRequired(s => s.Anexo)
                .WithMany(s => s.)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
