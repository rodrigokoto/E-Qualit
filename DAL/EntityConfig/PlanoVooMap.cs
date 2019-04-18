using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class PlanoVooMap : EntityTypeConfiguration<PlanoVoo>
    {
        public PlanoVooMap()
        {
            ToTable("PlanoVoo");

            HasKey(t => t.Id);

            Property(t => t.Id)
                .HasColumnName("Id");

            Property(t => t.Realizado)
                .HasColumnName("Realizado");

            Property(t => t.DataAlteracao)
                .HasColumnName("DataAlteracao");

            Property(t => t.DataInclusao)
                .HasColumnName("DataInclusao");

            Property(t => t.DataReferencia)
                .HasColumnName("DataReferencia");

            Ignore(x => x.AtingiuAMeta);

            HasRequired(t => t.PeriodicidadeAnalise)
                .WithMany(t => t.MetasRealizadas)
                .HasForeignKey(t => t.IdPeriodicidadeAnalise);
        }
    }
}
