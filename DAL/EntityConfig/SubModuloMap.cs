using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class SubModuloMap : EntityTypeConfiguration<SubModulo>
    {
        public SubModuloMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Funcionalidade)
               .WithMany()
            .HasForeignKey(x => x.CodigoFuncionalidade);

            HasRequired(x => x.Site)
               .WithMany()
            .HasForeignKey(x => x.CodigoSite);
        }
    }
}
