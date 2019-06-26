using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ArquivoPlaiAnexoMap : EntityTypeConfiguration<ArquivoPlaiAnexo>
    {
        public ArquivoPlaiAnexoMap()
        {
            Ignore(x => x.ApagarAnexo);

            ToTable("ArquivoPlaiAnexo");

            HasKey(x => x.IdArquivoPlaiAnexo);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdPlai)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.Plai)
                .WithMany(s => s.ArquivoPlai)
                .HasForeignKey(s => s.IdPlai)
                .WillCascadeOnDelete(false);

            HasRequired(s => s.Anexo)
                .WithMany(s => s.ArquivoPlaiAnexo)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
