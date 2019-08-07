using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class CalibracaoMap : EntityTypeConfiguration<Calibracao>
    {
        public CalibracaoMap()
        {
            ToTable("Calibracao");

            Ignore(x => x.ValidationResult);
            Ignore(x => x.NomeUsuarioAprovador);
            Ignore(x => x.ArquivoCertificadoAux);
            Ignore(x => x.SubmitArquivosCertificado);

            HasKey(t => t.IdCalibracao);

            Property(t => t.IdCalibracao).HasColumnName("IdCalibracao");
            Property(t => t.Certificado).HasColumnName("Certificado");
            Property(t => t.OrgaoCalibrador).HasColumnName("OrgaoCalibrador");
            Property(t => t.Aprovado).HasColumnName("Aprovado");
            Property(t => t.Aprovador).HasColumnName("Aprovador");
            Property(t => t.Observacoes).HasColumnName("Observacoes");
            //Property(t => t.ArquivoCertificado).HasColumnName("ArquivoCertificado");
            Property(t => t.DataCalibracao).HasColumnName("DataCalibracao");
            Property(t => t.DataProximaCalibracao).HasColumnName("DataProximaCalibracao");
            Property(t => t.DataCriacao).HasColumnName("DataCriacao");
            Property(t => t.DataAlteracao).HasColumnName("DataAlteracao");
            Property(t => t.IdUsuarioIncluiu).HasColumnName("IdUsuarioIncluiu");

            Property(t => t.DataRegistro)
                .HasColumnName("DataRegistro");

            Property(t => t.DataNotificacao)
                .HasColumnName("DataNotificacao");

            Ignore(x => x.StatusCalibracao);

            HasRequired(t => t.Instrumento)
                .WithMany(t => t.Calibracao)
                .HasForeignKey(d => d.IdInstrumento);

            HasRequired(t => t.UsuarioAprovador)
                .WithMany()
                .HasForeignKey(d => d.Aprovador);
        }
    }
}
