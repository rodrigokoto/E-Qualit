using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ArquivoLicencaAnexoMap : EntityTypeConfiguration<ArquivoLicencaAnexo>
    {
        public ArquivoLicencaAnexoMap()
        {
            Ignore(x => x.ApagarAnexo);

            ToTable("ArquivoLicencaAnexo");

            HasKey(x => x.IdArquivoLicencaAnexo);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdLicenca)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.Licenca)
                .WithMany(s => s.ArquivoLicenca)
                .HasForeignKey(s => s.IdLicenca)
                .WillCascadeOnDelete(false);

            HasRequired(s => s.Anexo)
                .WithMany(s => s.ArquivoLicencaAnexo)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
