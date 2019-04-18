using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class FormacaoAcademicaMap : EntityTypeConfiguration<FormacaoAcademica>
    {
        public FormacaoAcademicaMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Competencia)
                .WithMany(x => x.FormacoesAcademicas)
            .HasForeignKey(x => x.CodigoCompetencia);
        }
    }
}
