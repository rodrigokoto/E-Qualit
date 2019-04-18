using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class TelefoneMap : EntityTypeConfiguration<Dominio.Entidade.Telefone>
    {
        public TelefoneMap()
        {
            // Primary Key
            HasKey(t => t.IdTelefone);

            // Properties
            Property(t => t.FlTipoTelefone)
                .IsRequired()
                .HasMaxLength(3);

            Property(t => t.NuTelefone)
                .IsRequired()
                .HasMaxLength(15);

            Property(t => t.NuRamal)
                .HasMaxLength(15);

            Property(t => t.DsObservacao)
                .HasMaxLength(300);

            // Table & Column Mappings
            ToTable("Telefone");
            Property(t => t.IdTelefone).HasColumnName("IdTelefone");
            Property(t => t.IdRelacionado).HasColumnName("IdRelacionado");
            Property(t => t.FlTipoTelefone).HasColumnName("FlTipoTelefone");
            Property(t => t.NuTelefone).HasColumnName("NuTelefone");
            Property(t => t.NuRamal).HasColumnName("NuRamal");
            Property(t => t.DsObservacao).HasColumnName("DsObservacao");
        }
    }
}
