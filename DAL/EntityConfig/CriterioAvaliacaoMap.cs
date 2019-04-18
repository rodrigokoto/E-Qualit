using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class CriterioAvaliacaoMap : EntityTypeConfiguration<CriterioAvaliacao>
    {
        public CriterioAvaliacaoMap()
        {
            ToTable("CriterioAvaliacao");

            HasKey(x => x.IdCriterioAvaliacao);

            Property(x => x.Titulo)
                .HasColumnName("Titulo")
                .IsRequired();

            Property(x => x.DtCriacao)
                .HasColumnName("DtCriacao")
                .IsRequired();

            Property(x => x.DtAlteracao)
                .HasColumnName("DtAlteracao")
                .IsRequired();

            Property(x => x.IdProduto)
                .HasColumnName("IdProduto")
                .IsRequired();

            Property(x => x.Ativo)
               .IsRequired()
               .HasColumnName("Ativo");

            #region Relecionamentos

            HasRequired(t => t.Produto)
               .WithMany(t => t.CriteriosAvaliacao)
               .HasForeignKey(d => d.IdProduto);                      

            #endregion


        }
    }
}
