using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class FuncaoMap : EntityTypeConfiguration<Funcao>
    {
        public FuncaoMap()
        {
            // Primary Key
            HasKey(t => t.IdFuncao);

            // Properties
            Property(t => t.NmNome)
                .IsRequired()
                .HasMaxLength(40);

            // Table & Column Mappings
            ToTable("Funcao");
            Property(t => t.IdFuncao).HasColumnName("IdFuncao");
            Property(t => t.IdFuncionalidade).HasColumnName("IdFuncionalidade");
            Property(t => t.NmNome).HasColumnName("NmNome");
            Property(t => t.NuOrdem).HasColumnName("NuOrdem");

            // Relationships
            HasRequired(t => t.Funcionalidade)
                .WithMany(t => t.Funcoes)
                .HasForeignKey(d => d.IdFuncionalidade);

        }
    }
}
