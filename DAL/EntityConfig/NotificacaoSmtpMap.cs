using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class NotificacaoSmtpMap : EntityTypeConfiguration<Dominio.Entidade.NotificacaoSmtp>
    {
        public NotificacaoSmtpMap()
        {
            // Primary Key
            HasKey(t => t.IdSmptNotificacao);

            Property(t => t.DsSmtp)
                .HasMaxLength(1000);

            // Table & Column Mappings
            ToTable("NotificacaoSmtp");
            Property(t => t.IdSmptNotificacao).HasColumnName("IdSmptNotificacao");
            Property(t => t.DsSmtp).HasColumnName("DsSmtp");
            Property(t => t.NuPorta).HasColumnName("NuPorta");
            Property(t => t.CdUsuario).HasColumnName("CdUsuario");
            Property(t => t.CdSenha).HasColumnName("CdSenha");
            Property(t => t.NmNome).HasColumnName("NmNome");
            Property(t => t.FlAtivo).HasColumnName("FlAtivo");         
        }
    }
}
