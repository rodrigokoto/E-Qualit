using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            HasKey(t => t.IdUsuario);

            Ignore(x => x.FotoPerfilAux);
            Ignore(x => x.ValidationResult);

            Ignore(x => x.ConfirmaSenha);
            Ignore(x => x.SenhaAtual);

            Property(t => t.IdPerfil)
           .IsRequired();

            Property(t => t.NmCompleto)
                .HasMaxLength(60)
                .IsRequired();

            Property(t => t.CdIdentificacao)
                .IsRequired();

            Property(t => t.NuCPF)
                .HasMaxLength(11)
                .IsOptional();
            

            Property(t => t.CdSenha)
                .IsOptional();

            Property(t => t.DtExpiracao)
                .IsOptional();

            Property(t => t.NmFuncao);

            Property(t => t.FlCompartilhado);

            Property(t => t.FlRecebeEmail);

            Property(t => t.FlBloqueado);

            Property(t => t.FlBloqueado);

            Property(t => t.FlAtivo);

            Property(t => t.FlSexo)
               .IsOptional();

            Property(t => t.DtUltimoAcesso);

            Property(t => t.NuFalhaLNoLogin);

            Property(t => t.DtAlteracaoSenha);

            Property(t => t.IdUsuarioIncluiu);

            Property(t => t.DtAlteracao)
                .IsOptional();

            Property(t => t.DtInclusao);

            Property(x => x.Token)
                .HasColumnType("uniqueidentifier")
                .IsOptional();

        
            #region Relacionamentos

            //HasOptional(s => s.FotoPerfil)
            //    .WithMany(t => t.Usuarios)
            //    .HasForeignKey(s => s.IdFotoPerfil)
            //    .WillCascadeOnDelete(true);

            #endregion
        }
    }
}