using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class InstrumentoMap : EntityTypeConfiguration<Instrumento>
    {
        public InstrumentoMap()
        {
            ToTable("Instrumento");

            Ignore(x => x.ValidationResult);
            Ignore(x => x.CorStatus);
            Ignore(x => x.NomeStatus);

            HasKey(t => t.IdInstrumento);

            Property(t => t.IdInstrumento).HasColumnName("IdInstrumento");
            Property(t => t.IdSite).HasColumnName("IdSite");
            Property(t => t.IdProcesso).HasColumnName("IdProcesso");
            Property(t => t.IdResponsavel).HasColumnName("IdResponsavel");
            Property(t => t.Equipamento).HasColumnName("Equipamento");
            Property(t => t.Numero).HasColumnName("Numero");
            Property(t => t.LocalDeUso).HasColumnName("LocalDeUso");
            Property(t => t.Marca).HasColumnName("Marca");
            Property(t => t.Modelo).HasColumnName("Modelo");
            Property(t => t.Periodicidade).HasColumnName("Periodicidade");
            Property(t => t.Escala).HasColumnName("Escala");
            Property(t => t.valorAceitacao).HasColumnName("CriterioAceitacao");
            Property(t => t.MenorDivisao).HasColumnName("MenorDivisao");
            Property(t => t.Status).HasColumnName("Status");
            Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            Property(t => t.DataCriacao).HasColumnName("DataCriacao");
            Property(t => t.IdUsuarioIncluiu).HasColumnName("IdUsuarioIncluiu");
            Property(t => t.SistemaDefineStatus).HasColumnName("SistemaDefineStatus");
            Property(t => t.DescricaoCriterio).HasColumnName("DescricaoCriterio");
            Property(t => t.FlagTravado).HasColumnName("FlagTravado");

        }
    }
}
