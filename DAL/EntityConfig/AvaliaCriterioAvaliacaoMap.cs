using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class AvaliaCriterioAvaliacaoMap: EntityTypeConfiguration<AvaliaCriterioAvaliacao>
    {
        public AvaliaCriterioAvaliacaoMap()
        {
            ToTable("AvaliaCriterioAvaliacao");
            HasKey(x => x.IdAvaliaCriterioAvaliacao);

            Property(x => x.IdFornecedor)
                .HasColumnName("IdFornecedor")
                .IsRequired();

            Property(x => x.IdCriterioAvaliacao)
                .HasColumnName("IdCriterioAvaliacao")
                .IsRequired();

            Property(x => x.NotaAvaliacao)
                .HasColumnName("NotaAvaliacao")
                .IsRequired();

            Property(x => x.DtAvaliacao)
                .HasColumnName("DtAvaliacao")
                .IsRequired();

            Property(x => x.DtProximaAvaliacao)
                .HasColumnName("DtProximaAvaliacao")
                .IsRequired();

            Property(t => t.IdUsuarioAvaliacao)
                .HasColumnName("IdUsuarioAvaliacao");

            Property(t => t.GuidAvaliacao)
                .HasColumnName("GuidAvaliacao");

            #region Relacionamentos

            HasRequired(x => x.CriterioAvaliacao)
                .WithMany(x => x.Avaliacoes)
                .HasForeignKey(x => x.IdCriterioAvaliacao);

            HasRequired(x => x.Fornecedor)
                .WithMany(x => x.AvaliaCriteriosAvaliacao)
                .HasForeignKey(x => x.IdFornecedor);

            HasRequired(t => t.UsuarioAvaliacao)
             .WithMany(t => t.UsuariAvaliaCriterioAvaliacaoFornecedor)
             .HasForeignKey(d => d.IdUsuarioAvaliacao);

            #endregion


        }
    }
}
