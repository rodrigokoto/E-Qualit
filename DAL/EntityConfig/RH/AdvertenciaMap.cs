using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class AdvertenciaMap : EntityTypeConfiguration<Advertencia>
    {
        public AdvertenciaMap()
        {
            HasKey(x => x.Codigo);
            Ignore(x => x.ValidationResult);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Funcionario)
                .WithMany(x => x.Advertencias)
            .HasForeignKey(x => x.CodigoFuncionario);

            HasRequired(x => x.TipoAdvertencia)
               .WithMany()
            .HasForeignKey(x => x.CodigoTipoAdvertencia);
        }
    }
}
