using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class EnderecoMap : EntityTypeConfiguration<Dominio.Entidade.Endereco>
    {
        public EnderecoMap()
        {
            // Primary Key
            HasKey(t => t.IdEndereco);

            // Properties
            Property(t => t.FlTipoEndereco)
                .IsRequired()
                .HasMaxLength(3);

            Property(t => t.DsLogradouro)
                .IsRequired()
                .HasMaxLength(60);

            Property(t => t.NuNumero)
                .IsRequired()
                .HasMaxLength(15);

            Property(t => t.DsComplemento)
                .HasMaxLength(30);

            Property(t => t.NmBairro)
                .IsRequired()
                .HasMaxLength(30);

            Property(t => t.NmCidade)
                .IsRequired()
                .HasMaxLength(30);

            Property(t => t.CdEstado)
                .IsRequired()
                .HasMaxLength(2);

            Property(t => t.NuCep)
                .IsRequired()
                .HasMaxLength(9);

            Property(t => t.DsPais)
                .HasMaxLength(32);

            // Table & Column Mappings
            ToTable("Endereco");
            Property(t => t.IdEndereco).HasColumnName("IdEndereco");
            Property(t => t.IdRelacionado).HasColumnName("IdRelacionado");
            Property(t => t.FlTipoEndereco).HasColumnName("FlTipoEndereco");
            Property(t => t.DsLogradouro).HasColumnName("DsLogradouro");
            Property(t => t.NuNumero).HasColumnName("NuNumero");
            Property(t => t.DsComplemento).HasColumnName("DsComplemento");
            Property(t => t.NmBairro).HasColumnName("NmBairro");
            Property(t => t.NmCidade).HasColumnName("NmCidade");
            Property(t => t.CdEstado).HasColumnName("CdEstado");
            Property(t => t.NuCep).HasColumnName("NuCep");
            Property(t => t.DsPais).HasColumnName("DsPais");
        }
    }
}
