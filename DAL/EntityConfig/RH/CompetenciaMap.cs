using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class CompetenciaMap : EntityTypeConfiguration<Competencia>
    {
        public CompetenciaMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.NivelEscolaridade)
                .IsRequired();

            Property(x => x.NivelFormacaoAcademica)
                .IsRequired();

            Property(x => x.Tipo)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();
        }
    }
}
