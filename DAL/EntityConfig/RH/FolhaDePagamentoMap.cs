using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class FolhaDePagamentoMap : EntityTypeConfiguration<FolhaDePagamento>
    {
        public FolhaDePagamentoMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Desconto)
                .IsRequired();

            Property(x => x.Valor)
               .IsRequired();

            Property(x => x.DataCadastro)
               .IsRequired();

            Property(x => x.Ativo)
             .IsRequired();

            HasRequired(x => x.Funcionario)
               .WithMany(x => x.FolhaDePagamentos)
            .HasForeignKey(x => x.CodigoFuncionario);

        }
    }
}
