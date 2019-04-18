using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class AtribuicaoMap : EntityTypeConfiguration<Atribuicao>
    {
        public AtribuicaoMap()
        {
            HasKey(x => x.Codigo);
            Ignore(x => x.ValidationResult);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
               .IsRequired();

            HasRequired(x => x.Competencia)
              .WithMany(x => x.Atribuicoes)
            .HasForeignKey(x => x.CodigoCompetencia);
        }
    }
}
