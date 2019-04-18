using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ClienteLogoMap : EntityTypeConfiguration<ClienteLogo>
    {
        public ClienteLogoMap()
        {
            HasKey(x => x.IdClienteLogo);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdCliente)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.Cliente)
                .WithMany(x => x.ClienteLogo)
                .HasForeignKey(s => s.IdCliente)
                .WillCascadeOnDelete(true);

            HasRequired(s => s.Anexo)
                .WithMany(s => s.ClientesLogo)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
