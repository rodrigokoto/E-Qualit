using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class NotificacaoMensagemMap : EntityTypeConfiguration<Dominio.Entidade.NotificacaoMensagem>
    {
        public NotificacaoMensagemMap()
        {
            // Primary Key
            HasKey(t => t.IdNotificacaoMenssagem);

            // Properties
            Property(t => t.NmEmailPara)
                .IsRequired()
                .HasMaxLength(150);

            Property(t => t.NmEmailNome)
                .IsRequired()
                .HasMaxLength(60);

            Property(t => t.DsAssunto)
                .HasMaxLength(300);

            // Table & Column Mappings
            ToTable("NotificacaoMensagem");
            Property(t => t.IdNotificacaoMenssagem).HasColumnName("IdNotificacaoMenssagem");
            Property(t => t.IdSite).HasColumnName("IdSite");
            Property(t => t.NmEmailPara).HasColumnName("NmEmailPara");
            Property(t => t.NmEmailNome).HasColumnName("NmEmailNome");
            Property(t => t.DsMensagem).HasColumnName("DsMensagem");
            Property(t => t.DtCadastro).HasColumnName("DtCadastro");
            Property(t => t.DtEnvio).HasColumnName("DtEnvio");
            Property(t => t.IdSmtpNotificacao).HasColumnName("IdSmtpNotificacao");
            Property(t => t.FlEnviada).HasColumnName("FlEnviada");
            Property(t => t.DsAssunto).HasColumnName("DsAssunto");

            // Relationships
            HasRequired(t => t.Site)
                .WithMany(t => t.NotificacaoMensagens)
                .HasForeignKey(d => d.IdSite);

            HasRequired(t => t.NotificacaoSmtp)
                .WithMany(t => t.NotificacaoMensagens)
                .HasForeignKey(d => d.IdSmtpNotificacao);
        }
    }
}
