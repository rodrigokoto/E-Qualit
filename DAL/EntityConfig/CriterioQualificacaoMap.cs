using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class CriterioQualificacaoMap : EntityTypeConfiguration<CriterioQualificacao>
    {
        public CriterioQualificacaoMap()
        {
            ToTable("CriterioQualificacao");

            Ignore(x => x.ValidationResult);

            HasKey(t => t.IdCriterioQualificacao);

            Property(x => x.Titulo)
               .HasColumnName("Titulo")
               .HasColumnType("varchar")
               .IsRequired();

            Property(x => x.DtCriacao)
               .HasColumnName("DtCriacao")
               .IsRequired();

            Property(x => x.DtAlteracao)
               .HasColumnName("DtAlteracao")
               .IsRequired();

            Property(x => x.TemControleVencimento)
                .HasColumnName("TemControleVencimento")
                .HasColumnType("bit")
                .IsRequired();


            Property(t => t.IdProduto)
                .HasColumnName("IdProduto");

            Property(x => x.Ativo)
               .IsRequired()
               .HasColumnName("Ativo");
            #region Relacionamentos

            HasRequired(t => t.Produto)
                .WithMany(t => t.CriteriosQualificacao)
                .HasForeignKey(d => d.IdProduto);

          
            #endregion
        }
    }
}
