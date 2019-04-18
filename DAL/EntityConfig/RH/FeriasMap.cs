using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class FeriasMap : EntityTypeConfiguration<Ferias>
    {
        public FeriasMap()
        {
            HasKey(x => x.Codigo);
            Ignore(x => x.Dias);

            Property(x => x.DataInicial)
                .IsRequired();

            Property(x => x.DataFinal)
                .IsRequired();

            Property(x => x.Remuneracao)
                .IsRequired();

            Property(x => x.TeveAdiantamentoDecimoTerceiro)
                .IsRequired();

            Property(x => x.Obervacao);

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Funcionario)
                .WithMany(x => x.Ferias)
            .HasForeignKey(x => x.CodigoFuncionario);

        }
    }
}
