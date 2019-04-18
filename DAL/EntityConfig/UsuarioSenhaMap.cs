using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class UsuarioSenhaMap : EntityTypeConfiguration<UsuarioSenha>
    {
        public UsuarioSenhaMap()
        {
            ToTable("UsuarioSenha");

            HasKey(t => t.IdUsuarioSenha);

            Property(x => x.IdUsuario)
                    .IsRequired();

            Property(x => x.CdSenha)
                    .IsRequired();

            Property(x => x.DtInclusaoSenha)
                    .IsRequired();

            #region Relacionamento

            HasRequired(s => s.Usuario)
                  .WithMany(x => x.UsuarioSenha)
                  .HasForeignKey(s => s.IdUsuario)
                  .WillCascadeOnDelete(true);


            #endregion
        }
    }
}
