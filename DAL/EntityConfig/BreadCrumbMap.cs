using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class BreadCrumbMap : EntityTypeConfiguration<BreadCrumb>
    {
        public BreadCrumbMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Descricao)
               .IsRequired();

            Property(x => x.Lingua)
               .IsRequired();

            Property(x => x.Ativo);
        }
    }
}
