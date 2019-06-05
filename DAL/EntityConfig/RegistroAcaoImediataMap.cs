using Dominio.Entidade;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class RegistroAcaoImediataMap : EntityTypeConfiguration<RegistroAcaoImediata>
    {
        public RegistroAcaoImediataMap()
        {
            Ignore(x => x.Estado);
            Ignore(x => x.ArquivoEvidenciaAux);

            ToTable("RegistroAcaoImediata");

            HasKey(x => x.IdAcaoImediata);

            Property(x => x.IdAcaoImediata)
                .HasColumnName("IdRegistroAcaoImediata");
                


            Property(t => t.IdRegistroConformidade)
                .HasColumnName("IdRegistro")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Aprovado)
                .HasColumnType("bit")
                .HasColumnName("Aprovado")
                .IsOptional();

            Property(t => t.Descricao)
                .HasColumnName("DsAcao")
                .IsRequired();

            Property(t => t.IdResponsavelImplementar)
                .HasColumnName("IdReponsavelImplementar")
                .IsRequired();

            //Property(t => t.ArquivoEvidencia)
            //    .HasMaxLength(1000);

            Property(t => t.IdUsuarioIncluiu)
                .HasColumnName("IdUsuarioIncluiu")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.DtPrazoImplementacao)
                .HasColumnName("DtPrazoImplementacao")
                .IsRequired();

            Property(t => t.DtEfetivaImplementacao)
                .HasColumnName("DtEfetivaImplementacao")
                .IsOptional();

            //Property(t => t.ArquivoEvidencia)
            //    .HasColumnName("NmArquivoEvidencia");

            Property(t => t.DtInclusao)
                .HasColumnName("DtInclusao")
                .IsRequired();

            #region Relacionamentos

            HasRequired(t => t.ResponsavelImplementar)
                .WithMany(t => t.AcoesImediatas)
                .HasForeignKey(d => d.IdResponsavelImplementar);

            HasRequired(t => t.Registro)
                .WithMany(t => t.AcoesImediatas)
                .HasForeignKey(d => d.IdRegistroConformidade)
                .WillCascadeOnDelete(true);



                        
            #endregion

        }
    }
}
