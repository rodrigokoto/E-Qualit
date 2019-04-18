using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class TipoAdvertenciaMap : EntityTypeConfiguration<TipoAdvertencia>
    {
        public TipoAdvertenciaMap()
        {
            HasKey(x => x.Codigo);
            Ignore(x => x.ValidationResult);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
               .IsRequired();
        }
    }
}
