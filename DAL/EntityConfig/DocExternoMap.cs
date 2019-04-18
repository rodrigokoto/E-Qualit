using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class DocExternoMap : EntityTypeConfiguration<DocExterno>
    {
        public DocExternoMap()
        {
            HasKey(x => x.IdDocExterno);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.LinkDocumentoExterno)
                .IsRequired();

            #region Relacionamento

            HasRequired(x => x.Anexo)
                .WithMany(t => t.DocsExterno)
                .HasForeignKey(d => d.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
