using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class SiteMap : EntityTypeConfiguration<Site>
    {
        public SiteMap()
        {
            Ignore(x => x.ValidationResult);

            HasKey(t => t.IdSite);

            Property(t => t.NmFantasia)
                .IsRequired();

            Property(t => t.NmRazaoSocial)
                .IsRequired();

            Property(t => t.NuCNPJ);

            Property(t => t.DsObservacoes);

            Property(t => t.DsFrase);


            // Relationships
            HasRequired(t => t.Cliente)
                .WithMany()//t => t.Sites
                .HasForeignKey(d => d.IdCliente);

        }
    }
}
