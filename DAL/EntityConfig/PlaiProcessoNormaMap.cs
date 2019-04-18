using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class PlaiProcessoNormaMap : EntityTypeConfiguration<PlaiProcessoNorma>
    {
        public PlaiProcessoNormaMap()
        {
            ToTable("PlaiProcessoNorma");

            Ignore(x => x.Ativo);
            Ignore(x => x.NomeProcesso);
            

            HasKey(x => x.IdPlaiProcessoNorma);

            Property(x => x.IdPlai)
                .IsRequired()
                .HasColumnName("IdPlai");

            Property(x => x.IdProcesso)
                .IsRequired()
                .HasColumnName("IdProcesso");

            Property(x => x.IdNorma)
                .IsRequired()
                .HasColumnName("IdNorma");

            Property(x => x.Data)
                .IsRequired()
                .HasColumnName("Data");

            HasRequired(x => x.Plai)
             .WithMany(x => x.PlaiProcessoNorma)
             .HasForeignKey(x => x.IdPlai)
             .WillCascadeOnDelete(true);

            HasRequired(x => x.Norma)
            .WithMany()
            .HasForeignKey(x => x.IdNorma);

            HasRequired(x => x.Processo)
            .WithMany()
            .HasForeignKey(x => x.IdProcesso)
            .WillCascadeOnDelete(true);
        }
    }
}
