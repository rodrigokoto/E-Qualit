using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class DependenteMap : EntityTypeConfiguration<Dependente>
    {
        public DependenteMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Nome)
                .IsRequired();

            Property(x => x.DataNascimento)
                .IsRequired();

            Property(x => x.CodigoParentesco)
                .IsRequired();

            Property(x => x.Sexo)
                .IsRequired();

            Property(x => x.Documentos)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Planos)
                .WithMany();

            HasRequired(x => x.Funcionario)
                .WithMany(x => x.Dependentes)
            .HasForeignKey(x => x.CodigoFuncionario);

            HasRequired(x => x.Parentesco)
                .WithMany()
           .HasForeignKey(x => x.CodigoParentesco);

        }

    }
}
