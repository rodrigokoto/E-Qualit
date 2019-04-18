using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class EstoqueMap : EntityTypeConfiguration<Estoque>
    {
        public EstoqueMap()
        {
            HasKey(x => x.Codigo);
            Ignore(x => x.ValidationResult);

            Property(x => x.Quantidade)
                .IsRequired();

            Property(x => x.DataCadastro)
            .IsRequired();

            Property(x => x.Ativo)
               .IsRequired();

            HasRequired(x => x.EPI)
               .WithMany()
               .HasForeignKey(x => x.CodigoEPI);
        }
    }
}
