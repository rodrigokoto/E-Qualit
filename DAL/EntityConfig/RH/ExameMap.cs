using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class ExameMap : EntityTypeConfiguration<Exame>
    {
        public ExameMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Funcionario)
                .WithMany(x => x.Exames)
            .HasForeignKey(x => x.CodigoFuncionario);
        }
    }
}
