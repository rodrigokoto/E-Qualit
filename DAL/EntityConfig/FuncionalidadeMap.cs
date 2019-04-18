using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class FuncionalidadeMap : EntityTypeConfiguration<Funcionalidade>
    {
        public FuncionalidadeMap()
        {
            ToTable("Funcionalidade");

            HasKey(t => t.IdFuncionalidade);

            Property(t => t.IdFuncionalidade)
                .HasColumnName("IdFuncionalidade");

            Property(t => t.Nome)
                .HasColumnName("NmNome");

            Property(t => t.Tag)
                .HasColumnName("Tag");

            Property(t => t.Url)
                .HasColumnName("Url");

            Property(t => t.NuOrdem)
                .HasColumnName("NuOrdem");

            Property(t => t.CdFormulario)
                .HasColumnName("CdFormulario");

            Property(t => t.Ativo)
             .HasColumnName("Ativo");
            

            Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(30);

            Property(t => t.CdFormulario)
                .HasMaxLength(20);
        }
    }
}
