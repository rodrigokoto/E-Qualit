using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class EPIMap : EntityTypeConfiguration<EPI>
    {
        public EPIMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Funcionario)
               .WithMany(x => x.Epis)
           .HasForeignKey(x => x.CodigoFuncionario);
        }
    }
}
