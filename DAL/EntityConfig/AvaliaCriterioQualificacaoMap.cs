using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class AvaliaCriterioQualificacaoMap : EntityTypeConfiguration<AvaliaCriterioQualificacao>
    {
        public AvaliaCriterioQualificacaoMap()
        {
            ToTable("AvaliaCriterioQualificacao");

            Ignore(x => x.ArquivosDeEvidenciaAux);
            Ignore(x => x.ValidationResult);

            HasKey(t => t.IdAvaliaCriterioQualificacao);

            Property(t => t.IdCriterioQualificacao)
               .HasColumnName("IdCriterioQualificacao");

            Property(t => t.IdFornecedor)
               .HasColumnName("IdFornecedor"); 
            
            Property(x => x.DtVencimento)
                .HasColumnName("DtVencimento")
                .IsOptional();

            Property(t => t.ArquivoEvidencia)
                .HasColumnName("ArquivoEvidencia")
                .HasColumnType("varchar")
                .IsOptional();

            Property(t => t.Aprovado)
                .HasColumnName("Aprovado")
                .HasColumnType("bit")
                .IsOptional();

            Property(t => t.Observacoes)
                .HasColumnName("Observacoes")
                .HasColumnType("varchar")
                .IsOptional();

            Property(t => t.IdResponsavelPorControlarVencimento)
                .HasColumnName("IdResponsavelPorControlarVencimento")
                .IsOptional();

            Property(t => t.IdResponsavelPorQualificar)
                .HasColumnName("IdResponsavelPorQualificar")
                .IsOptional();

            Property(t => t.DtEmissao)
                .HasColumnName("DtEmissao")
                .IsOptional();

            Property(t => t.DtAlteracaoEmissao)
                .HasColumnName("DtAlteracaoEmissao")
                .IsOptional();

            Property(t => t.DtQualificacaoVencimento)
                .HasColumnName("DtQualificacaoVencimento")
                .IsOptional();

            Property(t => t.NumeroDocumento)
                .HasColumnName("NumeroDocumento")
                .HasColumnType("varchar")
                .IsOptional();

            Property(t => t.OrgaoExpedidor)
                .HasColumnName("OrgaoExpedidor")
                .HasColumnType("varchar")
                .IsOptional();

            Property(t => t.ObservacoesDocumento)
                .HasColumnName("ObservacoesDocumento")
                .HasColumnType("varchar")
                .IsOptional();            

            #region Relacionamentos

            HasRequired(t => t.CriterioQualificacao)
                .WithMany(t => t.AvaliaCriteriosQualificacao)
                .HasForeignKey(d => d.IdCriterioQualificacao);

            HasRequired(t => t.Fornecedor)
               .WithMany(t => t.AvaliaCriteriosQualificacao)
               .HasForeignKey(d => d.IdFornecedor);

            HasOptional(t => t.ResponsavelPorControlarVencimento)
                .WithMany(t => t.CriteriosQualificacaoPorControleVencimento)
                .HasForeignKey(d => d.IdResponsavelPorControlarVencimento);

            HasOptional(t => t.ResponsavelPorQualificar)
                .WithMany(t => t.CriteriosQualificacaoPorQualificar)
                .HasForeignKey(d => d.IdResponsavelPorQualificar);

            #endregion
        }
    }
}
