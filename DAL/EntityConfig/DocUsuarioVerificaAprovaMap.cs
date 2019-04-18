using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class DocUsuarioVerificaAprovaMap : EntityTypeConfiguration<Dominio.Entidade.DocUsuarioVerificaAprova>
    {
        public DocUsuarioVerificaAprovaMap()
        {
            // Primary Key
            HasKey(t => t.IdDocUsuarioVerificaAprova);

            // Properties
            Property(t => t.TpEtapa)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            ToTable("DocUsuarioVerificaAprova");
            Property(t => t.IdDocUsuarioVerificaAprova).HasColumnName("IdDocUsuarioVerificaAprova");
            Property(t => t.IdDocumento).HasColumnName("IdDocumento");
            Property(t => t.IdUsuario).HasColumnName("IdUsuario");
            Property(t => t.TpEtapa).HasColumnName("TpEtapa");
            Property(t => t.FlAprovou).HasColumnName("FlAprovou");
            Property(t => t.FlVerificou).HasColumnName("FlVerificou");
            Property(t => t.Ativo).HasColumnName("Ativo");

            // Relationships
            HasRequired(t => t.Usuario)
                .WithMany()
                .HasForeignKey(d => d.IdUsuario);

            // DocUsuarioVerificaAprova para DocDocumento
            HasRequired(t => t.DocDocumento)
                .WithMany(t => t.DocUsuarioVerificaAprova)
                .HasForeignKey(d => d.IdDocumento)
                .WillCascadeOnDelete(true);

            Ignore(x => x.deletar);

        }
    }
}
