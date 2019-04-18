using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class UsuarioAnexoMap : EntityTypeConfiguration<UsuarioAnexo>
    {
        public UsuarioAnexoMap()
        {
            ToTable("UsuarioAnexo");

            HasKey(x => x.IdUsuarioAnexo);

            Property(x => x.IdAnexo)
                .IsRequired();

            Property(x => x.IdUsuario)
                .IsRequired();

            #region Relacionamento

            HasRequired(s => s.Usuario)
                .WithMany(x=>x.FotoPerfil)
                .HasForeignKey(s => s.IdUsuario)
                .WillCascadeOnDelete(true);

            HasRequired(s => s.Anexo)
                .WithMany(s=>s.FotosUsuario)
                .HasForeignKey(s => s.IdAnexo)
                .WillCascadeOnDelete(true);

            #endregion
        }
    }
}
