using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class HabilidadeMap : EntityTypeConfiguration<Habilidade>
    {
        public HabilidadeMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Competencia)
                .WithMany(x => x.Habilidades)
            .HasForeignKey(x => x.CodigoCompetencia);
        }
    }
}
