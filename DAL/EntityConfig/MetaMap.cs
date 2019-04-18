using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class MetaMap : EntityTypeConfiguration<Meta>
    {
        public MetaMap()
        {
            ToTable("Meta");

            HasKey(t => t.Id);

            Ignore(t => t.Mes);

            Property(t => t.Id)
                .HasColumnName("Id");

            Property(t => t.Valor)
                .HasColumnName("Valor");

            Property(t => t.UnidadeMedida)
                .HasColumnName("UnidadeMedida");

            Property(t => t.DataReferencia)
                .HasColumnName("DataReferencia");

            Property(t => t.IdPeriodicidadeAnalise)
                .HasColumnName("IdPeriodicidadeAnalise");

            HasRequired(t => t.PeriodicidadeAnalise)
                .WithMany(t => t.PlanoDeVoo)
                .HasForeignKey(t => t.IdPeriodicidadeAnalise);
        }
    }
}
