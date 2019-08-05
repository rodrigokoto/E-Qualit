using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ArquivoNaoConformidadeAnexoMap : EntityTypeConfiguration<ArquivoNaoConformidadeAnexo>
    {
        public ArquivoNaoConformidadeAnexoMap()
        {
            Ignore(x => x.ApagarAnexo);

            ToTable("ArquivoNaoConformidadeAnexo");

            HasKey(x => x.IdArquivoNaoConformidadeAnexo);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdRegistroConformidade)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.Anexo)
                .WithMany(s => s.ArquivoNaoConformidadeAnexo)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
