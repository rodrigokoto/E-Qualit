using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class NotificacaoMap : EntityTypeConfiguration<Notificacao>
    {
        public NotificacaoMap()
        {
            ToTable("Notificacao");

            HasKey(x => x.IdNotificacao);

            Property(t => t.IdNotificacao)
                .HasColumnName("IdNotificacao");

            Property(t => t.IdUsuario)
                .HasColumnName("IdUsuario");

            Property(t => t.IdProcesso)
                .HasColumnName("IdProcesso");

            Property(t => t.IdSite)
                .HasColumnName("IdSite");

            Property(t => t.IdRelacionado)
                .HasColumnName("IdRelacionado");

            Property(t => t.IdFuncionalidade)
                .HasColumnName("IdFuncionalidade");

            Property(t => t.TpNotificacao)
                .HasColumnName("TpNotificacao")
                .IsRequired()
                .HasMaxLength(2);

            Property(t => t.DtVencimento)
                .HasColumnName("DtVencimento");

            Property(t => t.DtEnvioFilaDisparo)
                .HasColumnName("DtEnvioFilaDisparo");

            Property(t => t.Descricao)
                .HasColumnName("DsDescricao")
                .HasMaxLength(1000);

            Property(t => t.NuDiasAntecedencia)
                .HasColumnName("NuDiasAntecedencia");

            Property(t => t.FlEtapa)
                .HasColumnName("FlEtapa")
                .HasMaxLength(20);

            HasRequired(t => t.Usuario)
                .WithMany(t => t.Notificacoes)
                .HasForeignKey(d => d.IdUsuario);

            HasRequired(t => t.Funcionalidade)
                .WithMany(t => t.Notificacoes)
                .HasForeignKey(d => d.IdFuncionalidade);

            HasOptional(t => t.Processo)
                .WithMany(t => t.Notificacoes)
                .HasForeignKey(d => d.IdProcesso);

            HasRequired(t => t.Site)
                .WithMany(t => t.Notificacoes)
                .HasForeignKey(d => d.IdSite);
        }
    }
}
