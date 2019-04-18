using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class PlanoMap : EntityTypeConfiguration<Plano>
    {
        public PlanoMap()
        {
            HasKey(x => x.Codigo);
            Ignore(x => x.ValidationResult);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.TipoPlano)
                .WithMany()
            .HasForeignKey(x => x.CodigoTipoPlano);
        }
    }
}
