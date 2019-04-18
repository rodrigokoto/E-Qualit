using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class DocDocumentoMap : EntityTypeConfiguration<DocDocumento>
    {
        public DocDocumentoMap()
        {
            ToTable("DocDocumento");

            Ignore(x => x.ValidationResult);

            HasKey(t => t.IdDocumento);

            Property(t => t.Titulo)
                .HasMaxLength(1000);

            Property(t => t.NumeroDocumento);
                //.HasMaxLength(15);

            Property(t => t.IdDocumento)
                .HasColumnName("IdDocumento");

            Property(t => t.IdDocumentoPai)
                .HasColumnName("IdDocumentoPai");

            Property(t => t.IdSite)
                .HasColumnName("IdSite");

            Property(t => t.IdDocIdentificador)
                .HasColumnName("IdDocIdentificador");

            Property(t => t.IdProcesso)
                .HasColumnName("IdProcesso");

            Property(t => t.IdLicenca)
                .IsOptional();

            Property(t => t.IdDocExterno)
                .IsOptional();

            Property(t => t.TextoDoc)
                .IsOptional();

            Property(t => t.EntradaTextoDoc)
               .IsOptional();

            Property(t => t.SaidaTextoDoc)
               .IsOptional();

            Property(t => t.FluxoDoc)
                .IsOptional();

            Property(t => t.RecursoDoc)
                .IsOptional();


            Property(t => t.IdCategoria)
                .HasColumnName("IdCategoria");
            Property(t => t.Titulo).HasColumnName("Titulo");

            Property(t => t.IdSigla)
                .HasColumnName("IdSigla");

            Property(t => t.NumeroDocumento)
                .HasColumnName("NumeroDocumento");

            Property(t => t.StatusRegistro)
              .HasColumnName("StatusRegistro"); 

            Property(t => t.NuRevisao).HasColumnName("NuRevisao");
            Property(t => t.IdElaborador).HasColumnName("IdElaborador");
            Property(t => t.DtPedidoVerificacao).HasColumnName("DtPedidoVerificacao");
            Property(t => t.DtVerificacao).HasColumnName("DtVerificacao");
            Property(t => t.DtVencimento).HasColumnName("DtVencimento");
            Property(t => t.DtEmissao).HasColumnName("DtEmissao");
            Property(t => t.DtPedidoAprovacao).HasColumnName("DtPedidoAprovacao");
            Property(t => t.DtAprovacao).HasColumnName("DtAprovacao");
            Property(t => t.DtNotificacao).HasColumnName("DtNotificacao");
            Property(t => t.XmlMetadata).HasColumnName("XmlMetadata");
            Property(t => t.FlWorkFlow).HasColumnName("FlWorkFlow");
            Property(t => t.FlRevisaoPeriodica).HasColumnName("FlRevisaoPeriodica");
            Property(t => t.FlStatus).HasColumnName("FlStatus");
            Property(t => t.IdUsuarioIncluiu).HasColumnName("IdUsuarioIncluiu");
            Property(t => t.IdUsuarioAlteracao).HasColumnName("IdUsuarioAlteracao");
            Property(t => t.DtInclusao).HasColumnName("DtInclusao");
            Property(t => t.DtAlteracao).HasColumnName("DtAlteracao");
            Property(t => t.Ativo).HasColumnName("Ativo");
            Property(t => t.IdGestaoDeRisco).HasColumnName("IdRegistro");
            Property(t => t.CorRisco).HasColumnName("Risco");
            Property(t => t.PossuiGestaoRisco).HasColumnName("PossuiGestaoRisco");

            HasRequired(t => t.Elaborador)
                .WithMany()
                .HasForeignKey(d => d.IdElaborador);

            HasOptional(t => t.Processo)
                .WithMany()
                .HasForeignKey(t => t.IdProcesso);

            HasOptional(x => x.GestaoDeRisco)
               .WithMany()
               .HasForeignKey(x => x.IdGestaoDeRisco);


            // Relacionamentos ControladorCategorias
            HasRequired(t => t.Sigla)
                .WithMany()
                .HasForeignKey(d => d.IdSigla);

            HasRequired(t => t.Categoria)
                .WithMany()
                .HasForeignKey(d => d.IdCategoria);

            Ignore(x => x.Verificadores);
            Ignore(x => x.Aprovadores);
            Ignore(x => x.ConteudoDocumento);
        }
    }
}
