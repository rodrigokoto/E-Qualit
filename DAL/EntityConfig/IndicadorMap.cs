using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class IndicadorMap : EntityTypeConfiguration<Indicador>
    {
        public IndicadorMap()
        {
            Ignore(x => x.ValidationResult);

            ToTable("Indicador");

            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasColumnName("IdIndicador");

            Property(x => x.Objetivo)
                 .IsRequired();

            Property(x => x.Descricao)
                 .IsRequired();

            Property(x => x.MetaAnual)
                .IsRequired();

            Property(x => x.Ano)
                .IsRequired();

            Property(x => x.Periodicidade)
                .IsRequired();

            Property(x => x.Unidade)
                 .IsRequired();

            Property(x => x.Direcao)
                 .IsRequired();

            Property(x => x.IdSite)
                 .IsRequired();

            Property(x => x.IdUsuarioIncluiu)
                 .IsRequired();

            Property(x => x.DataAlteracao)
                 .IsRequired();

            Property(x => x.DataInclusao)
                 .IsRequired();

            Property(x => x.StatusRegistro)
                 .HasColumnName("StatusRegistro")
               .IsRequired();

            Property(x => x.Medida)
                .IsRequired();
          
            #region Relacionamento

            HasRequired(x => x.Processo)
                .WithMany()
                .HasForeignKey(t => t.IdProcesso);

            HasRequired(x => x.Responsavel)
                .WithMany()
                .HasForeignKey(t => t.IdResponsavel);

            #endregion



        }
    }
}
