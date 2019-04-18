using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class CursoMap : EntityTypeConfiguration<Curso>
    {
        public CursoMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.Entidade)
                .IsRequired();

            Property(x => x.DataValidade)
                .IsRequired();

            Property(x => x.DataCadastro)
               .IsRequired();

            Property(x => x.Ativo)
               .IsRequired();

            HasRequired(x => x.Competencia)
              .WithMany(x => x.Cursos)
            .HasForeignKey(x => x.CodigoCompetencia);
        }
    }
}
