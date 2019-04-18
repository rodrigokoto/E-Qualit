using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class CargoProcessoMap : EntityTypeConfiguration<Dominio.Entidade.CargoProcesso>
    {
        public CargoProcessoMap()
        {

            // Primary Key
            HasKey(t => t.IdCargoProcesso);

            // Properties
            // Table & Column Mappings
            ToTable("CargoProcesso");
            Property(t => t.IdCargoProcesso).HasColumnName("IdCargoProcesso");
            Property(t => t.IdProcesso).HasColumnName("IdProcesso");
            Property(t => t.IdCargo).HasColumnName("IdCargo");
            Property(t => t.IdFuncao).HasColumnName("IdFuncao");

            // Relationships
            HasRequired(t => t.Cargo)
                .WithMany(t => t.CargoProcessos)
                .HasForeignKey(d => d.IdCargo);

            HasRequired(t => t.Funcao)
                .WithMany(t => t.CargoProcessos)
                .HasForeignKey(d => d.IdFuncao);

            HasRequired(t => t.Processo)
                .WithMany(t => t.CargoProcessoes)
                .HasForeignKey(d => d.IdProcesso);

        }
    }
}
