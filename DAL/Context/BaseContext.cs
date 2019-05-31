using DAL.EntityConfig;
using DAL.EntityConfig.RH;
using Dominio.Entidade;
using Dominio.Entidade.RH;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DAL.Context
{
    public class BaseContext : DbContext
    {
        public BaseContext() : base("name=Context")
        {
            Database.SetInitializer<BaseContext>(null);

        }

        public DbSet<RegistroConformidade> RegistroConformidade { get; set; }
        public DbSet<ControladorCategoria> ControladorCategoria { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<CargoProcesso> CargoProcesso { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Perfil> CtrlPerfil { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuarioSenha> UsuarioSenha { get; set; }
        public DbSet<DocumentoCargo> DocumentoCargo { get; set; }
        public DbSet<DocDocumento> DocDocumento { get; set; }
        public DbSet<DocTemplate> DocTemplate { get; set; }
        public DbSet<DocUsuarioVerificaAprova> DocUsuarioVerificaAprova { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Funcao> Funcao { get; set; }
        public DbSet<Funcionalidade> Funcionalidade { get; set; }
        public DbSet<ListaValor> ListaValor { get; set; }
        public DbSet<Notificacao> Notificacao { get; set; }
        public DbSet<NotificacaoMensagem> NotificacaoMensagem { get; set; }
        public DbSet<NotificacaoSmtp> NotificacaoSmtp { get; set; }
        public DbSet<Processo> Processo { get; set; }
        public DbSet<RegistroAcaoImediata> AcaoImediata { get; set; }
        public DbSet<Site> Site { get; set; }
        public DbSet<SiteFuncionalidade> SiteModulo { get; set; }
        public DbSet<Telefone> Telefone { get; set; }
        public DbSet<UsuarioCargo> UsuarioCargo { get; set; }
        public DbSet<UsuarioClienteSite> UsuarioClienteSite { get; set; }
        public DbSet<AnaliseCritica> AnaliseCritica { get; set; }
        public DbSet<AnaliseCriticaFuncionario> AnaliseCriticaFuncionario { get; set; }
        public DbSet<AnaliseCriticaTema> AnaliseCriticaTema { get; set; }
        public DbSet<Norma> Norma { get; set; }
        public DbSet<NormaProcesso> NormaProcesso { get; set; }
        public DbSet<CriterioAceitacao> CriterioAceitacao { get; set; }
        public DbSet<Calibracao> Calibracao { get; set; }
        public DbSet<Instrumento> Instrumento { get; set; }
        public DbSet<Plai> Plai { get; set; }
        public DbSet<PlaiProcessoNorma> PlaiProcessoNorma { get; set; }
        public DbSet<PlaiGerentes> PlaiGerentes { get; set; }
        public DbSet<AvaliacaoCriticidade> AvaliacaoCriticidade { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<CriterioQualificacao> CriterioQualificacao { get; set; }
        public DbSet<AvaliaCriterioQualificacao> AvaliaCriterioQualificacao { get; set; }
        
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<ProdutoFornecedor> ProdutoFornecedor { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<CriterioAvaliacao> CriterioAvaliacao { get; set; }
        public DbSet<AvaliaCriterioAvaliacao> AvaliaCriterioAvaliacao { get; set; }
        public DbSet<ControleImpressao> ControleImpressao { get; set; }
        public DbSet<Meta> Meta { get; set; }
        public DbSet<PlanoVoo> PlanoVoo { get; set; }
        public DbSet<PeriodicidaDeAnalise> PeriodicidaDeAnalise { get; set; }
        public DbSet<Indicador> Indicador { get; set; }
        public DbSet<DocumentoAssunto> DocumentoAssunto { get; set; }
        public DbSet<DocumentoComentario> DocumentoComentario { get; set; }
        public DbSet<HistoricoCriterioAvaliacao> HistoricoCriterioAvaliacao { get; set; }
        public DbSet<BreadCrumb> BreadCrumb { get; set; }
        public DbSet<Anexo> Anexo { get; set; }
        public DbSet<UsuarioAnexo> UsuarioAnexo { get; set; }
        public DbSet<SiteAnexo> SiteAnexo { get; set; }
        public DbSet<ClienteLogo> ClienteLogo { get; set; }
        public DbSet<ClienteContrato> ClienteContrato { get; set; }
        public DbSet<ArquivosDeEvidencia> ArquivosDeEvidencia { get; set; }
        public DbSet<ArquivosEvidenciaCriterioQualificacao> ArquivosEvidenciaCriterioQualificacao { get; set; }
        public DbSet<ArquivoDeEvidenciaAcaoImediata> ArquivoDeEvidenciaAcaoImediata { get; set; }
        public DbSet<ArquivoCertificadoAnexo> ArquivoCertificadoAnexo { get; set; }
        //public DbSet<DocLicenca> DocLicenca { get; set; }

        public DbSet<Licenca> Licenca { get; set; }
        public DbSet<DocExterno> DocExterno { get; set; }
        public DbSet<DocRotina> RotinaDoc { get; set; }
        public DbSet<DocRegistro> RegistroDoc { get; set; }

        public DbSet<DocIndicadores> DocIndicadores { get; set; }
        public DbSet<FilaEnvio> FilaEnvio { get; set; }

        #region RH

        public DbSet<SubModulo> SubModulo { get; set; }

        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<Advertencia> Advertencia { get; set; }
        public DbSet<TipoAdvertencia> TipoAdvertencia { get; set; }
        public DbSet<ValeTransporte> ValeTransporte { get; set; }
        public DbSet<Exame> Exame { get; set; }
        public DbSet<Emprestimo> Emprestimo { get; set; }
        public DbSet<TipoEmprestimo> TipoEmprestimo { get; set; }
        public DbSet<Ferias> Ferias { get; set; }
        public DbSet<EPI> EPI { get; set; }
        public DbSet<Estoque> Estoque { get; set; }
        public DbSet<FolhaDePagamento> FolhaDePagamento { get; set; }

        public DbSet<Dependente> Dependente { get; set; }
        public DbSet<Parentesco> Parentesco { get; set; }
        public DbSet<Plano> Plano { get; set; }
        public DbSet<TipoPlano> TipoPlano { get; set; }

        public DbSet<CargoRH> CargoRH { get; set; }
        public DbSet<Sindicato> Sindicato { get; set; }

        public DbSet<Competencia> Competencia { get; set; }
        public DbSet<FormacaoAcademica> FormacaoAcademica { get; set; }
        public DbSet<Curso> Curso { get; set; }
        public DbSet<Treinamento> Treinamento { get; set; }
        public DbSet<Conhecimento> Conhecimento { get; set; }
        public DbSet<Habilidade> Habilidade { get; set; }
        public DbSet<Atribuicao> Atribuicao { get; set; }
        #endregion
     
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties<string>()
                .Configure(x => x.HasColumnType("varchar"));

            modelBuilder.Properties<string>();

            modelBuilder.Configurations.Add(new RegistroConformidadesMap());
            modelBuilder.Configurations.Add(new ControladorCategoriasMap());
            modelBuilder.Configurations.Add(new CargoMap());
            modelBuilder.Configurations.Add(new CargoProcessoMap());
            modelBuilder.Configurations.Add(new ClienteMap());
            modelBuilder.Configurations.Add(new PerfilMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
            modelBuilder.Configurations.Add(new UsuarioSenhaMap());
            modelBuilder.Configurations.Add(new DocumentoCargoMap());
            modelBuilder.Configurations.Add(new DocDocumentoMap());
            modelBuilder.Configurations.Add(new DocTemplateMap());
            modelBuilder.Configurations.Add(new DocUsuarioVerificaAprovaMap());
            modelBuilder.Configurations.Add(new EnderecoMap());
            modelBuilder.Configurations.Add(new FuncaoMap());
            modelBuilder.Configurations.Add(new FuncionalidadeMap());
            modelBuilder.Configurations.Add(new ListaValorMap());
            modelBuilder.Configurations.Add(new NotificacaoMap());
            modelBuilder.Configurations.Add(new NotificacaoMensagemMap());
            modelBuilder.Configurations.Add(new NotificacaoSmtpMap());
            modelBuilder.Configurations.Add(new ProcessoMap());
            modelBuilder.Configurations.Add(new RegistroAcaoImediataMap());
            modelBuilder.Configurations.Add(new SiteMap());
            modelBuilder.Configurations.Add(new SiteModuloMap());
            modelBuilder.Configurations.Add(new TelefoneMap());
            modelBuilder.Configurations.Add(new UsuarioCargoMap());
            modelBuilder.Configurations.Add(new UsuarioClienteSiteMap());
            modelBuilder.Configurations.Add(new AnaliseCriticaMap());
            modelBuilder.Configurations.Add(new AnaliseCriticaFuncionarioMap());
            modelBuilder.Configurations.Add(new AnaliseCriticaTemaMap());
            modelBuilder.Configurations.Add(new NormaMap());
            modelBuilder.Configurations.Add(new NormaProcessoMap());
            modelBuilder.Configurations.Add(new CriterioAceitacaoMap());
            modelBuilder.Configurations.Add(new CalibracaoMap());
            modelBuilder.Configurations.Add(new InstrumentoMap());
            modelBuilder.Configurations.Add(new PaiMap());
            modelBuilder.Configurations.Add(new PlaiMap());
            modelBuilder.Configurations.Add(new PlaiProcessoNormaMap());
            modelBuilder.Configurations.Add(new PlaiGerentesMap());
            modelBuilder.Configurations.Add(new ProdutoMap());
            modelBuilder.Configurations.Add(new AvaliacaoCriticidadeMap());
            modelBuilder.Configurations.Add(new CriterioAvaliacaoMap());
            modelBuilder.Configurations.Add(new CriterioQualificacaoMap());
            modelBuilder.Configurations.Add(new AvaliaCriterioQualificacaoMap());            
            modelBuilder.Configurations.Add(new FornecedorMap());
            modelBuilder.Configurations.Add(new ProdutoFornecedorMap());
            modelBuilder.Configurations.Add(new LogMap());
            modelBuilder.Configurations.Add(new AvaliaCriterioAvaliacaoMap());
            modelBuilder.Configurations.Add(new ControleImpressaoMap());
            modelBuilder.Configurations.Add(new MetaMap());
            modelBuilder.Configurations.Add(new PlanoVooMap());
            modelBuilder.Configurations.Add(new PeriodicidadeAnaliseMap());
            modelBuilder.Configurations.Add(new IndicadorMap());
            modelBuilder.Configurations.Add(new DocumentoAssuntoMap());
            modelBuilder.Configurations.Add(new DocumentoComentarioMap());
            modelBuilder.Configurations.Add(new HistoricoCriterioAvaliacaoMap());
            modelBuilder.Configurations.Add(new BreadCrumbMap());
            modelBuilder.Configurations.Add(new AnexoMap());
            modelBuilder.Configurations.Add(new UsuarioAnexoMap());
            modelBuilder.Configurations.Add(new SiteAnexoMap());
            modelBuilder.Configurations.Add(new ClienteLogoMap());
            modelBuilder.Configurations.Add(new ClienteContratoMap());
            modelBuilder.Configurations.Add(new ArquivosDeEvidenciaMap());
            modelBuilder.Configurations.Add(new ArquivosEvidenciaCriterioQualificacaoMap());
            modelBuilder.Configurations.Add(new ArquivoDeEvidenciaAcaoImediataMap());
            modelBuilder.Configurations.Add(new ArquivoCertificadoAnexoMap());
            //modelBuilder.Configurations.Add(new DocLicencaMap());
            modelBuilder.Configurations.Add(new LicencaMap());
            modelBuilder.Configurations.Add(new DocExternoMap());

            modelBuilder.Configurations.Add(new DocRotinaMap());
            modelBuilder.Configurations.Add(new DocIndicadoresMap());

            modelBuilder.Configurations.Add(new DocRegistroMap());

            

        #region RH
            modelBuilder.Configurations.Add(new AdvertenciaMap());
            modelBuilder.Configurations.Add(new TipoAdvertenciaMap());
            modelBuilder.Configurations.Add(new AtribuicaoMap());
            modelBuilder.Configurations.Add(new CargoRHMap());
            modelBuilder.Configurations.Add(new CompetenciaMap());
            modelBuilder.Configurations.Add(new ConhecimentoMap());
            modelBuilder.Configurations.Add(new CursoMap());
            modelBuilder.Configurations.Add(new DependenteMap());
            modelBuilder.Configurations.Add(new EmprestimoMap());
            modelBuilder.Configurations.Add(new TipoEmprestimoMap());
            modelBuilder.Configurations.Add(new EPIMap());
            modelBuilder.Configurations.Add(new EstoqueMap());
            modelBuilder.Configurations.Add(new ExameMap());
            modelBuilder.Configurations.Add(new FeriasMap());
            modelBuilder.Configurations.Add(new FolhaDePagamentoMap());
            modelBuilder.Configurations.Add(new FuncionarioMap());
            modelBuilder.Configurations.Add(new FormacaoAcademicaMap());
            modelBuilder.Configurations.Add(new HabilidadeMap());
            modelBuilder.Configurations.Add(new PlanoMap());
            modelBuilder.Configurations.Add(new TipoPlanoMap());
            modelBuilder.Configurations.Add(new ParentescoMap());
            modelBuilder.Configurations.Add(new SindicatoMap());
            modelBuilder.Configurations.Add(new SubModuloMap());
            modelBuilder.Configurations.Add(new TreinamentoMap());
            modelBuilder.Configurations.Add(new ValeTransporteMap());

            #endregion
        }

    }
}
