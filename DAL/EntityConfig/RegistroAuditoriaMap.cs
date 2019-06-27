using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class RegistroAuditoriaMap : EntityTypeConfiguration<Dominio.Entidade.RegistroAuditoria>
    {
        public RegistroAuditoriaMap()
        {
            ToTable("RegistroAuditoria");

            HasKey(x => x.IdRegAuditoria);

            Property(x => x.IdRegAuditoria).IsRequired();

            Property(x => x.IdGestor).IsRequired();

            Property(x => x.IdPlai).IsRequired();

            Property(x => x.DtInclusao).IsRequired();

            Property(x => x.IdFilaEnvio).IsRequired();

            Property(x => x.IdUsuarioInclusao).IsRequired();
        }
    }
}
