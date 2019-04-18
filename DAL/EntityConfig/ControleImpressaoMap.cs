using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class ControleImpressaoMap : EntityTypeConfiguration<ControleImpressao>
    {
        public ControleImpressaoMap()
        {
            ToTable("ControleImpressao");
            
            HasKey(x => x.Id);

            Property(x => x.Id)
                .IsRequired()
                .HasColumnName("Id");

            Property(x => x.IdFuncionalidade)
                .IsRequired()
                .HasColumnName("IdFuncionalidade");

            Property(x => x.CodigoReferencia)
                .IsRequired()
                .HasColumnName("CodigoReferencia");

            Property(x => x.CopiaControlada)
                .IsRequired()
                .HasColumnName("CopiaControlada");

            Property(x => x.IdUsuarioDestino)
                .IsOptional()
                .HasColumnName("IdUsuarioDestino");

            Property(x => x.DataImpressao)
                .IsRequired()
                .HasColumnName("DataImpressao");

            Property(x => x.IdUsuarioIncluiu)
                .IsRequired()
                .HasColumnName("IdUsuarioIncluiu");

            Property(x => x.DataInclusao)
                .IsRequired()
                .HasColumnName("DataInclusao");
            
            HasRequired(t => t.Funcionalidade)
                .WithMany()
                .HasForeignKey(t => t.IdFuncionalidade);

            HasRequired(t => t.Usuario)
                .WithMany()
                .HasForeignKey(t => t.IdUsuarioIncluiu);

            HasOptional(t => t.UsuarioDestino)
                .WithMany()
                .HasForeignKey(t => t.IdUsuarioDestino);
        }
    }
}
