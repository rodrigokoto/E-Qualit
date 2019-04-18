using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class TreinamentoMap : EntityTypeConfiguration<Treinamento>
    {
        public TreinamentoMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Competencia)
              .WithMany(x => x.Treinamentos)
            .HasForeignKey(x => x.CodigoCompetencia);
        }
    }
}
