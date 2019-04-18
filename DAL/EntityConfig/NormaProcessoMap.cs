using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class NormaProcessoMap : EntityTypeConfiguration<NormaProcesso>
    {
        public NormaProcessoMap()
        {
            ToTable("NormaProcesso");

            HasKey(x => x.IdNormaProcesso);

            Property(x => x.IdNorma)
               .IsRequired()
               .HasColumnName("IdNorma");

            Property(x => x.IdProcesso)
               .IsRequired()
               .HasColumnName("IdProcesso");

            Property(x => x.Requisitos)
                .HasColumnName("Requisitos");

            HasRequired(x => x.Norma)
               .WithMany(x => x.Processos)
               .HasForeignKey(x => x.IdNorma);

            HasRequired(x => x.Processo)
               .WithMany(x => x.Normas)
               .HasForeignKey(x => x.IdProcesso);
        }
    }
}
