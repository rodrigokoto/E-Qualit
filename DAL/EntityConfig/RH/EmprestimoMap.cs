using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class EmprestimoMap : EntityTypeConfiguration<Emprestimo>
    {
        public EmprestimoMap()
        {
            HasKey(x => x.Codigo);
            Ignore(x => x.ValidationResult);

            Property(x => x.Valor)
                .IsRequired();

            Property(x => x.Parcelas)
                .IsRequired();

            Property(x => x.DescontoFerias)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Funcionario)
               .WithMany(x => x.Emprestimos)
            .HasForeignKey(x => x.CodigoFuncionario);

            HasRequired(x => x.TipoEmprestimo)
               .WithMany()
            .HasForeignKey(x => x.CodigoTipoEmprestimo);
        }
    }
}
