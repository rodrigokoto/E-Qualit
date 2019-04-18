using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class PerfilMap : EntityTypeConfiguration<Perfil>
    {
        public PerfilMap()
        {
            ToTable("Perfil");

            HasKey(t => t.IdPerfil);

            Property(t => t.NmNome)
                .IsRequired()
                .HasMaxLength(30);
        
            Property(t => t.IdPerfil)
                .HasColumnName("IdPerfil");

            Property(t => t.NmNome)
                .HasColumnName("NmNome");
        }
    }
}
