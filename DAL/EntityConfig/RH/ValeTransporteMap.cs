using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig.RH
{
    public class ValeTransporteMap : EntityTypeConfiguration<ValeTransporte>
    {
        public ValeTransporteMap()
        {
            HasKey(x => x.Codigo);

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.DataVigencia);

            Property(x => x.Possui);
           
            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Funcionario)
             .WithMany(x => x.ValeTransportes)
            .HasForeignKey(x => x.CodigoFuncionario);
        }
    }
}
