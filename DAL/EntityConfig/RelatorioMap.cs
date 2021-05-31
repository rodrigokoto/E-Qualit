using Dominio.Entidade;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class RelatorioaMap : EntityTypeConfiguration<Relatorio>
    {
        public RelatorioaMap()
        {
            ToTable("Relatorio");

            Ignore(x => x.Parametros);
            
            Property(x => x.IdRelatorio)
                .IsRequired()
                .HasColumnName("IdRelatorio");

            Property(x => x.Nome)
                .IsRequired()
                .HasColumnName("Nome");

            Property(x => x.Url)
                .IsRequired()
                .HasColumnName("Url");

            Property(t => t.IdFuncionalidade)
               .HasColumnName("IdFuncionalidade")
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(t => t.Funcionalidade)
                .WithMany(t => t.Relatorios)
                .HasForeignKey(d => d.IdFuncionalidade)
                .WillCascadeOnDelete(true);
        }
    }
}
