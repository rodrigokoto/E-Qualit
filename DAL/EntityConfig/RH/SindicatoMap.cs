using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class SindicatoMap : EntityTypeConfiguration<Sindicato>
    {

        public SindicatoMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.ValorAnual)
                .IsRequired();

            Property(x => x.PercentualAnual)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();
        }

    }
}
