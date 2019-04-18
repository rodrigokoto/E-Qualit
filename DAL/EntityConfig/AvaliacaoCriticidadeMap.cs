using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class AvaliacaoCriticidadeMap : EntityTypeConfiguration<AvaliacaoCriticidade>
    {
        public AvaliacaoCriticidadeMap()
        {
            ToTable("AvaliacaoCriticidade");

            Ignore(x => x.ValidationResult);

            HasKey(t => t.IdAvaliacaoCriticidade);

            Property(x => x.Titulo)
               .IsRequired()
               .HasColumnName("Titulo");

            Property(x => x.Ativo)
                .IsRequired()
                .HasColumnName("Ativo");

            Property(x => x.DtCriacao)
               .IsRequired()
               .HasColumnName("DtCriacao");

            Property(x => x.DtAlteracao)
               .IsRequired()
               .HasColumnName("DtAlteracao");

            Property(t => t.IdProduto)
                .HasColumnName("IdProduto");


            #region Relacionamentos

            HasRequired(t => t.Produto)
                .WithMany(t => t.AvaliacoesCriticidade)
                .HasForeignKey(d => d.IdProduto);

            #endregion



        }
    }
}
