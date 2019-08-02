using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ArquvioAcaoCorretivaAnexoMap : EntityTypeConfiguration<ArquivoAcaoCorretivaAnexo>
    {
        public ArquvioAcaoCorretivaAnexoMap()
        {
            Ignore(x => x.ApagarAnexo);

            ToTable("ArquvioAcaoCorretivaAnexo");

            HasKey(x => x.IdArquivoAcaoCorretivaAnexo);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdRegistroConformidade)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.AcaoCorretiva)
                .WithMany(s => s.ArquivoAcaoCorretiva)
                .HasForeignKey(s => s.IdRegistroConformidade)
                .WillCascadeOnDelete(false);

            HasRequired(s => s.Anexo)
                .WithMany(s => s.ArquivoAcaoCorretivaAnexo)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
