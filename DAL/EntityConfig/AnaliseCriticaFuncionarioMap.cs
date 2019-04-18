using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class AnaliseCriticaFuncionarioMap : EntityTypeConfiguration<AnaliseCriticaFuncionario>
    {
        public AnaliseCriticaFuncionarioMap()
        {
            ToTable("AnaliseCriticaFuncionario");

            HasKey(x => x.IdAnaliseCriticaFuncionario);

            Property(x => x.IdUsuario)
                .IsRequired()
                .HasColumnName("IdUsuario");

            Property(x => x.IdAnaliseCritica)
                .IsRequired()
                .HasColumnName("IdAnaliseCritica");

            Property(x => x.Funcao)
                .IsRequired()
                .HasColumnName("Funcao");

            Property(x => x.DataCadastro)
                .HasColumnName("DataCadastro");

            Property(x => x.Ativo)
                .IsRequired()
                .HasColumnName("Ativo");

            HasRequired(x => x.Funcionario)
             .WithMany()
             .HasForeignKey(x => x.IdUsuario);

            HasRequired(x => x.AnaliseCritica)
             .WithMany(x => x.Funcionarios)
             .HasForeignKey(x => x.IdAnaliseCritica);
        }
    }
}
