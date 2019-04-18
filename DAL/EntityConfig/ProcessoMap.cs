using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ProcessoMap : EntityTypeConfiguration<Processo>
    {
        public ProcessoMap()
        {
            Ignore(x => x.ValidationResult);

            ToTable("Processo");

            HasKey(x => x.IdProcesso);

            Ignore(x => x.CargoProcessoes);
            //Ignore(x => x.DocDocumentos);
            Ignore(x => x.Notificacoes);
            Ignore(x => x.Fornecedores);

            Property(x => x.IdProcesso)
                .HasColumnName("IdProcesso");

            Property(x => x.IdSite)
                .HasColumnName("IdSite");

            Property(x => x.Nome)
                .IsRequired()
                .HasColumnName("NmProcesso");

            Property(x => x.FlAtivo)
                .IsRequired()
                .HasColumnName("FlAtivo");

            Property(x => x.FlQualidade)
                .IsRequired()
                .HasColumnName("FlQualidade");

            Property(x => x.IdUsuarioIncluiu)
            .HasColumnName("IdUsuarioIncluiu");

            Property(x => x.Atividade)
              .HasColumnName("Atividade");

            Property(x => x.DataCadastro)
                .HasColumnName("DataCadastro");

            Property(x => x.DataAlteracao)
                .HasColumnName("DataAlteracao");

            Property(x => x.DocumentosAplicaveis)
                .HasColumnName("DocumentosAplicaveis");

            HasRequired(t => t.Site)
                .WithMany(t => t.Processos)
                .HasForeignKey(d => d.IdSite);

        }
    }
}
