using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class LicencaMap : EntityTypeConfiguration<Licenca>
    {
        public LicencaMap()
        {
            HasKey(x => x.IdLicenca);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.DataEmissao)
                .IsRequired();

            Property(x => x.DataVencimento)
                .IsRequired();

            #region Relacionamento

            HasRequired(x=>x.Anexo)
                .WithMany(t => t.Licencas)
                .HasForeignKey(d => d.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
