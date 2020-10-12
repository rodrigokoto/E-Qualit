using Dominio.Entidade;
using System.Data.Entity.ModelConfiguration;

namespace DAL.EntityConfig
{
    public class RegistroConformidadesMap : EntityTypeConfiguration<RegistroConformidade>
    {
        public RegistroConformidadesMap()
        {
            ToTable("Registros");

            Ignore(x => x.DescricaoAnaliseCausa);
            Ignore(x => x.IdResponsavelPorIniciarTratativaAcaoCorretiva);
            Ignore(x => x.ValidationResult);
            Ignore(x => x.ArquivosDeEvidenciaAux);


            HasKey(t => t.IdRegistroConformidade);

            Property(t => t.IdRegistroConformidade)
                .HasColumnName("IdRegistro");

            Property(t => t.DescricaoRegistro)
                .HasColumnName("DsOqueTexto")
                .IsRequired();

            Property(t => t.DtEmissao)
                .HasColumnName("DtEmissao")
                .IsRequired();

            Property(t => t.DtInclusao)
                .HasColumnName("DtInclusao");

            Property(t => t.IdProcesso)
                .HasColumnName("IdProcesso")
                .IsRequired();

            Property(t => t.IdEmissor)
                .HasColumnName("IdEmissor")
                .IsRequired();

            Property(x => x.ENaoConformidadeAuditoria)
                .HasColumnName("FlNaoConfirmidadeAuditoria");

            Property(x => x.IdTipoNaoConformidade)
                .HasColumnName("IdTipoNaoConformidade")
                .IsOptional();

            Property(x => x.Tags)
                .HasColumnName("Tags")
                .HasColumnType("varchar")
                .IsOptional();

            Property(x => x.IdResponsavelInicarAcaoImediata)
                .HasColumnName("IdResponsavelInicarAcaoImediata");

            Property(t => t.TipoRegistro)
                .HasColumnName("TpRegistro")
                .HasMaxLength(2);
            //.IsRequired();

            Property(t => t.IdSite)
                .HasColumnName("IdSite")
                .IsRequired();

            Property(t => t.NuRegistro)
                .HasColumnName("NuRegistro");


            Property(t => t.IdResponsavelEtapa)
                .HasColumnName("IdResponsavelEtapa")
                .IsOptional();

            Property(t => t.IdNuRegistroFilho)
                .HasColumnName("IdRegistroFilho");

            Property(t => t.IdResponsavelAcaoCorretiva)
                .HasColumnName("IdResponsavelAcaoCorretiva");

            Property(t => t.FlEficaz)
                .HasColumnName("FlEficaz");

            Property(t => t.EProcedente)
                .HasColumnName("FlProcedente");

            Property(t => t.DescricaoAcao)
                .HasColumnName("DsDescricaoCausa");

            Property(t => t.ECorrecao)
                .HasColumnName("FlCorrecao");

            Property(t => t.JustificativaAnulacao)
                .HasColumnName("DsJustificativaAnulacao");

            Property(t => t.NecessitaAcaoCorretiva)
                .HasColumnName("FlNecessitaAcaoCorretiva")
                .IsOptional();

            Property(t => t.DsAcao)
                .HasColumnName("DsAcao");

            Property(t => t.DsJustificativa)
                .HasColumnName("DsJustificativa");


            Property(t => t.DtAnalise)
                .HasColumnName("DtAnalise");

            Property(t => t.DtDescricaoAcao)
                .HasColumnName("DtDescricaoAcao");

            Property(t => t.DtEfetivaImplementacao)
                .HasColumnName("DtEfetivaImplementacao");

            Property(t => t.DtEnceramento)
                .HasColumnName("DtEnceramento");

            Property(t => t.DtPrazoImplementacao)
                .HasColumnName("DtPrazoImplementacao");

            Property(t => t.IdNaoConformidade)
                .HasColumnName("IdNaoConformidade");

            Property(t => t.IdResponsavelAnalisar)
                .HasColumnName("IdResponsavelAnalisar");

            Property(t => t.IdResponsavelReverificador)
                .HasColumnName("IdResponsavelDefinir");

            Property(t => t.IdResponsavelImplementar)
                .HasColumnName("IdResponsavelImplementar");

            Property(t => t.FlDesbloqueado)
                .HasColumnName("FlDesbloqueado");

            Property(t => t.FlStatusAntesAnulacao)
                .HasColumnName("FlStatusAntesAnulacao");

            Property(t => t.StatusEtapa)
                .HasColumnName("FlStatus");


            Property(t => t.IdUsuarioIncluiu)
                .HasColumnName("IdUsuarioIncluiu");

            Property(t => t.IdUsuarioAlterou)
                .HasColumnName("IdUsuarioAlterou");


            Property(t => t.DtAlteracao)
                .HasColumnName("DtAlteracao");

            Property(t => t.CriticidadeGestaoDeRisco)
                .HasColumnName("CriticidadeGestaoRisco")
                .IsOptional();


            Property(t => t.Causa)
           .HasColumnName("Causa")
           .IsOptional();


            Property(t => t.StatusRegistro)
            .HasColumnName("StatusRegistro")
            .IsOptional();

            Property(t => t.IdRegistroPai)
            .HasColumnName("IdRegistroPai")
            .IsOptional();

            Property(t => t.Parecer)
                .HasColumnName("Parecer")
                .IsOptional();

            #region Relacionamentos

            HasRequired(t => t.Emissor)
                .WithMany(t => t.RegistrosEmissor)
                .HasForeignKey(d => d.IdEmissor);

            HasOptional(t => t.ResponsavelInicarAcaoImediata)
                .WithMany(t => t.RegistrosInicarAcaoImediata)
                .HasForeignKey(d => d.IdResponsavelInicarAcaoImediata);

            HasOptional(t => t.ResponsavelEtapa)
                .WithMany(t => t.RegistrosResponsavelEtapa)
                .HasForeignKey(d => d.IdResponsavelEtapa);

            HasOptional(t => t.ResponsavelAcaoCorretiva)
                .WithMany(t => t.RegistrosResponsavelAcaoCorretiva)
                .HasForeignKey(d => d.IdResponsavelAcaoCorretiva);

            HasOptional(t => t.ResponsavelAnalisar)
                .WithMany(t => t.Registros1)
                .HasForeignKey(d => d.IdResponsavelAnalisar);

            HasOptional(t => t.ResponsavelReverificador)
                .WithMany(t => t.Registros2)
                .HasForeignKey(d => d.IdResponsavelReverificador);

            HasOptional(t => t.ResponsavelImplementar)
                .WithMany(t => t.Registros4)
                .HasForeignKey(d => d.IdResponsavelImplementar);

            HasOptional(t => t.TipoNaoConformidade)
               .WithMany()
               .HasForeignKey(d => d.IdTipoNaoConformidade);

            HasRequired(t => t.Site)
                .WithMany(t => t.Registros)
                .HasForeignKey(d => d.IdSite);
            //.WillCascadeOnDelete(true);


            #endregion

        }
    }
}
