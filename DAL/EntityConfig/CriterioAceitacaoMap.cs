using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class CriterioAceitacaoMap : EntityTypeConfiguration<CriterioAceitacao>
    {
        public CriterioAceitacaoMap()
        {
            ToTable("CriterioAceitacao");

            Ignore(x => x.ValidationResult);

            HasKey(t => t.IdCriterioAceitacao);

            Property(t => t.Erro).HasColumnName("Erro");
            Property(t => t.Incerteza).HasColumnName("Incerteza");
            Property(t => t.Aceito).HasColumnName("Aceito");
            Property(t => t.IdUsuarioIncluiu).HasColumnName("IdUsuarioIncluiu");
            Property(t => t.DtInclusao).HasColumnName("DataInclusao");
            Property(t => t.DtAlteracao).HasColumnName("DataAlteracao");
            Property(t => t.Resultado).HasColumnName("Resultado");
            Property(t => t.Periodicidade).HasColumnName("Periodicidade");

            HasOptional(t => t.Calibracao)
                .WithMany(t => t.CriterioAceitacao)
                .HasForeignKey(t => t.IdCalibracao);
        }
    }
}
