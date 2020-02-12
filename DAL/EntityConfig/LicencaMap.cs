using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class LicencaMap : EntityTypeConfiguration<Licenca>
    {
        public LicencaMap()
        {
            Ignore(x => x.ArquivosLicencaAnexos);
            Ignore(x => x.ArquivosLicencaAux);
            Ignore(x => x.ValidationResult);
            HasKey(x => x.IdLicenca);

            Property(x => x.IdResponsavel)
                .IsRequired()
                .HasColumnName("IdResponsavel");

            Property(x => x.IdProcesso)
                .HasColumnName("IdProcesso");

            Property(x => x.Titulo)
                .IsRequired()
                .HasColumnName("Titulo");

            Property(x => x.DataCriacao)
                .HasColumnName("DataCriacao");

            Property(x => x.DataEmissao)
                .HasColumnName("DataEmissao");

            Property(x => x.DataVencimento)
                .HasColumnName("DataVencimento");

            Property(x => x.Obervacao)
                .HasColumnName("Obervacao");

            Property(x => x.DataProximaNotificacao)
                .HasColumnName("DataProximaNotificacao");

            #region Relacionamento

            HasRequired(x => x.Cliente)
                .WithMany()
                .HasForeignKey(x => x.Idcliente);

            HasRequired(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.IdResponsavel);

            #endregion
        }
    }
}
