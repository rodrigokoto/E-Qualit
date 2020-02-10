using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class RegistroLicencaMap : EntityTypeConfiguration<Dominio.Entidade.RegistroLicenca>
    {
        public RegistroLicencaMap()
        {
            ToTable("RegistroLicenca");

            HasKey(x => x.IdRegLicenca);

            Property(x => x.GuidLicenca).IsRequired();

            Property(x => x.IdUsuarioInclusao).IsRequired();

            Property(x => x.DtInclusao).IsRequired();

            Property(x => x.IdFilaEnvio).IsRequired();

            Property(x => x.IdUsuarioInclusao).IsRequired();


        }
    }
}
