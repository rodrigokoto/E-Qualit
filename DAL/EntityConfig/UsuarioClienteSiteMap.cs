using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class UsuarioClienteSiteMap : EntityTypeConfiguration<Dominio.Entidade.UsuarioClienteSite>
    {
        public UsuarioClienteSiteMap()
        {
            // Primary Key
            HasKey(t => t.IdUsuarioClienteSite);

            // Properties
            // Table & Column Mappings
            ToTable("UsuarioClienteSite");
            Property(t => t.IdUsuarioClienteSite).HasColumnName("IdUsuarioClienteSite");
            Property(t => t.IdCliente).HasColumnName("IdCliente");
            Property(t => t.IdUsuario).HasColumnName("IdUsuario");
            Property(t => t.IdSite).HasColumnName("IdSite");

            // Relationships
            HasRequired(t => t.Cliente)
                .WithMany(t => t.UsuarioClienteSites)
                .HasForeignKey(d => d.IdCliente);

            HasRequired(t => t.Usuario)
                .WithMany(t => t.UsuarioClienteSites)
                .HasForeignKey(d => d.IdUsuario);

            HasOptional(t => t.Site)
                .WithMany(t => t.UsuarioClienteSites)
                .HasForeignKey(d => d.IdSite);

        }
    }
}
