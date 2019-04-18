using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class SiteModuloMap : EntityTypeConfiguration<SiteFuncionalidade>
    {
        public SiteModuloMap()
        {
            ToTable("SiteModulo");

            HasKey(t => t.IdSiteFuncionalidade);

            Property(t => t.IdSiteFuncionalidade)
                .HasColumnName("IdSiteModulo");

            Property(t => t.IdSite)
                .HasColumnName("IdSite");

            Property(t => t.IdFuncionalidade)
                .HasColumnName("IdModulo");

            HasRequired(t => t.Funcionalidade)
                .WithMany(t => t.SiteModulos)
                .HasForeignKey(d => d.IdFuncionalidade);

            HasRequired(t => t.Site)
                .WithMany(t => t.SiteFuncionalidades)
                .HasForeignKey(d => d.IdSite);

        }
    }
}
