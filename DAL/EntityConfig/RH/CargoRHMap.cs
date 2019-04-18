using Dominio.Entidade.RH;
using System.Data.Entity.ModelConfiguration;


namespace DAL.EntityConfig.RH
{
    public class CargoRHMap : EntityTypeConfiguration<CargoRH>
    {
        public CargoRHMap()
        {
            HasKey(x => x.Codigo);
            Ignore(x => x.ValidationResult);

            Property(x => x.Descricao)
                .IsRequired();

            Property(x => x.DataCadastro)
                .IsRequired();

            Property(x => x.Ativo)
                .IsRequired();

            HasRequired(x => x.Competencia)
                 .WithMany()
                 .HasForeignKey(x => x.CodigoCompetencia);

            HasRequired(x => x.Sindicato)
                .WithMany()
                .HasForeignKey(x => x.CodigoSindicato);
        }
    }
}
