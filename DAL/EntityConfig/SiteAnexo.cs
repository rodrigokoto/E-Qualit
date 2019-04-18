using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class SiteAnexoMap : EntityTypeConfiguration<SiteAnexo>
    {
        public SiteAnexoMap()
        {
            HasKey(x => x.IdSiteAnexo);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdSite)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.Anexo)
                .WithMany(s => s.FotosSite)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            HasRequired(s => s.Site)
                .WithMany(x => x.SiteAnexo)
                .HasForeignKey(s => s.IdSite)
                .WillCascadeOnDelete(true);


            #endregion
        }
    }
}
