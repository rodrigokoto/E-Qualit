using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class HistoricoCriterioAvaliacaoMap : EntityTypeConfiguration<HistoricoCriterioAvaliacao>
    {
        public HistoricoCriterioAvaliacaoMap()
        {
            ToTable("HistoricoCriterioAvaliacao");
            HasKey(x => x.IdHistoricoCriterioAvaliacao);

            Property(x => x.Nota)
                .HasColumnName("Nota")
                .IsRequired();

            Property(x=>x.DtCriacao)
                .HasColumnName("DtCriacao")
                .IsRequired();

            Property(x => x.IdCriterioAvaliacao)
                .HasColumnName("IdCriterioAvaliacao")
                .IsRequired();

            HasRequired(x => x.CriterioAvaliacao)
                .WithMany()
                .HasForeignKey(x => x.IdCriterioAvaliacao);
        }
    }
}
