using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class AnaliseCriticaMap : EntityTypeConfiguration<AnaliseCritica>
    {
        public AnaliseCriticaMap()
        {
            ToTable("AnaliseCritica");

            Ignore(x => x.ValidationResult);

            HasKey(x => x.IdAnaliseCritica);

            Property(x => x.IdResponsavel)
                .IsRequired()
                .HasColumnName("IdResponsavel");

            Property(x => x.IdSite)
                .IsRequired()
                .HasColumnName("IdSite");

            Property(x => x.Ata)
                .IsRequired()
                .HasColumnName("Ata");

            Property(x => x.DataCriacao)
                .IsRequired()
                .HasColumnName("DataCriacao");

            Property(x => x.DataProximaAnalise)
                .IsRequired()
                .HasColumnName("DataProximaAnalise");

            Property(x => x.DataCadastro)
                .HasColumnName("DataCadastro");

            Property(x => x.DataAlteracao)
                .IsOptional();

            Property(x => x.Ativo)
                .IsRequired()
                .HasColumnName("Ativo");

            HasRequired(x => x.Responsavel)
              .WithMany(x => x.AnalisesCriticas)
              .HasForeignKey(x => x.IdResponsavel);

            HasRequired(x => x.Site)
              .WithMany()
              .HasForeignKey(x => x.IdSite);
        }
    }
}
