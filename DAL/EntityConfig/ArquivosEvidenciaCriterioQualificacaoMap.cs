using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ArquivosEvidenciaCriterioQualificacaoMap : EntityTypeConfiguration<ArquivosEvidenciaCriterioQualificacao>
    {
        public ArquivosEvidenciaCriterioQualificacaoMap()
        {
            ToTable("ArquivosEvidenciaCriterioQualificacao");

            HasKey(x => x.IdArquivosEvidenciaCriterioQualificacao);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdAvaliaCriterioQualificacao)
                .IsRequired();
            
            #region Relacionamento

            HasRequired(s => s.Anexo)
                .WithMany(x => x.ArquivosEvidenciaCriterioQualificacao)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            HasRequired(s => s.AvaliaCriterioQualificacao)
               .WithMany(s => s.ArquivosEvidenciaCriterioQualificacao)
               .HasForeignKey(s => s.IdAvaliaCriterioQualificacao)
               .WillCascadeOnDelete(true);

            #endregion

        }
    }
}
