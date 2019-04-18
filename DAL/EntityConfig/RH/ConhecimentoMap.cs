using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class ConhecimentoMap : EntityTypeConfiguration<Conhecimento>
    {
        public ConhecimentoMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Competencia)
                .WithMany(x => x.Conhecimentos)
            .HasForeignKey(x => x.CodigoCompetencia);
        }
    }
}
