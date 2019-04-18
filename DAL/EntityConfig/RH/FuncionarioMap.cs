using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class FuncionarioMap : EntityTypeConfiguration<Funcionario>
    {

        public FuncionarioMap()
        {
            HasKey(x => x.Codigo);
            Ignore(x => x.ValidationResult);

            Property(x => x.Nome)
                .IsRequired();

            Property(x => x.Sexo)
                .IsRequired();

            Property(x => x.DataNascimento)
                .IsRequired();

            Property(x => x.DataVencimentoFerias)
                .IsRequired();

            Property(x => x.NumeroRegistro)
                .IsRequired();

            Property(x => x.EstadoCivil)
                .IsRequired();

            Property(x => x.Endereco)
               .IsRequired();

            Property(x => x.Bairro)
               .IsRequired();

            Property(x => x.Cep)
               .IsRequired();

            Property(x => x.Cidade)
               .IsRequired();

            Property(x => x.Uf)
               .IsRequired();

            Property(x => x.TelefoneResidencial);

            Property(x => x.TelefoneCelular);

            Property(x => x.TelefoneRecado);

            Property(x => x.CNH);

            Property(x => x.VencimentoCNH);

            Property(x => x.TituloEleitoral);

            Property(x => x.Outro);

            Property(x => x.Observacao);

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasOptional(x => x.Cargo)
               .WithMany()
               .HasForeignKey(x => x.CodigoCargo);

            HasOptional(x => x.Competencia)
               .WithMany()
               .HasForeignKey(x => x.CodigoCompetencia);
        }
           

    }
}
