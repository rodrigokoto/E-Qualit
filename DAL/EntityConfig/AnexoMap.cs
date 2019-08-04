using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class AnexoMap : EntityTypeConfiguration<Anexo>
    {
        public AnexoMap()
        {
            Ignore(x => x.ArquivoB64);
            Ignore(x => x.ValidationResult);
            Ignore(x => x.ApagarAnexo);

            ToTable("Anexo");

            HasKey(x => x.IdAnexo);

            Property(x => x.IdAnexo)
                .HasColumnName("IdAnexo");

            Property(x => x.Nome)
            .HasColumnName("Nome")
            .IsRequired();

            Property(x => x.Extensao)
                .HasColumnName("Extensao")
                .IsRequired();

            Property(x => x.Arquivo)
                .HasColumnName("Arquivo")
                .IsRequired();

            Property(x => x.DtCriacao)
                .HasColumnName("DtCriacao")
                .IsRequired();

            Property(x => x.DtAlteracao)
                .HasColumnName("DtAlteracao")
                .IsRequired();

            #region Relacionamentos

            //HasMany(x => x.Plais)
            //    .WithRequired(p => p.Arquivo)
            //    .HasForeignKey(a => a.IdArquivo)
            //    .WillCascadeOnDelete(true);

            #endregion

        }
    }
}
