using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ListaValorMap : EntityTypeConfiguration<Dominio.Entidade.ListaValor>
    {
        public ListaValorMap()
        {
            // Primary Key
            HasKey(t => t.IdListaValor);

            // Properties
            Property(t => t.CdTabela)
                .IsRequired()
                .HasMaxLength(10);

            Property(t => t.CdCodigo)
                .IsRequired()
                .HasMaxLength(40);

            Property(t => t.DsDescricao)
                .IsRequired();

            Property(t => t.CdCulture)
                .HasMaxLength(5);

            // Table & Column Mappings
            ToTable("ListaValor");
            Property(t => t.IdListaValor).HasColumnName("IdListaValor");
            Property(t => t.CdTabela).HasColumnName("CdTabela");
            Property(t => t.CdCodigo).HasColumnName("CdCodigo");
            Property(t => t.DsDescricao).HasColumnName("DsDescricao");
            Property(t => t.CdCulture).HasColumnName("CdCulture");
        }
    }
}
