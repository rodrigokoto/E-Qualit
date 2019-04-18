using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class LogMap : EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            ToTable("Log");

            HasKey(x => x.IdLog);
            
            Property(x => x.IdAcao)
             .IsRequired()
            .HasColumnName("IdAcao");

            Property(x => x.IP)
            .HasColumnName("IP");

            Property(x => x.Browser)
            .HasColumnName("Browser");

            Property(x => x.Mensagem)
             .IsRequired()
            .HasColumnName("Mensagem");

            Property(x => x.DataCadastro)
             .IsRequired()
            .HasColumnName("DataCadastro");

            Property(x => x.IdUsuario)
            .HasColumnName("IdUsuario");
        }
    }
}
