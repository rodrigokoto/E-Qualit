using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class CargoMap : EntityTypeConfiguration<Dominio.Entidade.Cargo>
    {
        public CargoMap()
        {
            Ignore(x => x.ValidationResult);
            // Primary Key
            HasKey(t => t.IdCargo);

            // Properties
            Property(t => t.NmNome)
                .IsRequired()
                .HasMaxLength(40);

            // Table & Column Mappings
            ToTable("Cargo");
            Property(t => t.IdCargo).HasColumnName("IdCargo");
            Property(t => t.IdSite).HasColumnName("IdSite");
            Property(t => t.NmNome).HasColumnName("NmNome");
            Property(t => t.IdUsuarioIncluiu).HasColumnName("IdUsuarioIncluiu");
            Property(t => t.DtInclusao).HasColumnName("DtInclusao");
            Property(t => t.Ativo).HasColumnName("FlAtivo");

            // Relationships
            HasRequired(t => t.Site)
                .WithMany(t => t.Cargos)
                .HasForeignKey(d => d.IdSite);

        }
    }
}
