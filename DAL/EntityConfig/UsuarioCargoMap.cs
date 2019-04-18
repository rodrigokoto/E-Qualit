using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class UsuarioCargoMap : EntityTypeConfiguration<Dominio.Entidade.UsuarioCargo>
    {
        public UsuarioCargoMap()
        {
            // Primary Key
            HasKey(t => t.IdUsuarioProcesso);

            // Properties
            // Table & Column Mappings
            ToTable("UsuarioCargo");
            Property(t => t.IdUsuarioProcesso).HasColumnName("IdUsuarioProcesso");
            Property(t => t.IdUsuario).HasColumnName("IdUsuario");
            Property(t => t.IdCargo).HasColumnName("IdCargo");

            // Relationships
            HasRequired(t => t.Cargo)
                .WithMany(t => t.UsuarioCargos)
                .HasForeignKey(d => d.IdCargo);
            HasRequired(t => t.Usuario)
                .WithMany(t => t.UsuarioCargoes)
                .HasForeignKey(d => d.IdUsuario);
        }
    }
}
