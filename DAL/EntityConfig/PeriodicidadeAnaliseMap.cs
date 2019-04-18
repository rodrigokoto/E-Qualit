using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class PeriodicidadeAnaliseMap : EntityTypeConfiguration<PeriodicidaDeAnalise>
    {
        public PeriodicidadeAnaliseMap()
        {
            ToTable("PeriodicidaDeAnalise");

            HasKey(t => t.Id);

            Property(t => t.Id)
                .HasColumnName("Id");

            Property(t => t.PeriodoAnalise)
                .HasColumnName("PeriodoAnalise");

            Property(t => t.Justificativa)
                .HasColumnName("Justificativa");

            Property(t => t.CorRisco)
                .HasColumnName("CorRisco")
                .IsOptional();

            Property(t => t.Inicio)
                .HasColumnName("Inicio");

            Property(t => t.Fim)
                .HasColumnName("Fim");

            Property(t => t.IdIndicador)
                .HasColumnName("IdIndicador");

            HasRequired(t => t.Indicador)
                .WithMany()
                .HasForeignKey(t => t.IdIndicador);

            HasOptional(t => t.PlanoDeAcao)
                .WithMany()
                .HasForeignKey(t => t.IdPlanDeAcao);
        }
    }
}
