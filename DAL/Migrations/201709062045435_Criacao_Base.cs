namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Criacao_Base : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegistroAcaoImediata",
                c => new
                    {
                        IdRegistroAcaoImediata = c.Int(nullable: false, identity: true),
                        DtPrazoImplementacao = c.DateTime(),
                        DsAcao = c.String(nullable: false, maxLength: 3000, unicode: false),
                        DtEfetivaImplementacao = c.DateTime(),
                        NmArquivoEvidencia = c.String(maxLength: 1000, unicode: false),
                        IdReponsavelImplementar = c.Int(nullable: false),
                        IdUsuarioIncluiu = c.Int(),
                        DtInclusao = c.DateTime(),
                        IdRegistro = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRegistroAcaoImediata)
                .ForeignKey("dbo.Registros", t => t.IdRegistro)
                .ForeignKey("dbo.Usuario", t => t.IdReponsavelImplementar)
                .Index(t => t.IdReponsavelImplementar)
                .Index(t => t.IdRegistro);
            
            CreateTable(
                "dbo.Registros",
                c => new
                    {
                        IdRegistro = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(),
                        TpRegistro = c.String(nullable: false, maxLength: 2, unicode: false),
                        NuRegistro = c.Int(nullable: false),
                        DsOqueTexto = c.String(nullable: false, maxLength: 3000, unicode: false),
                        DsAcao = c.String(maxLength: 3000, unicode: false),
                        NmEvidenciaImagem = c.String(maxLength: 1000, unicode: false),
                        DsJustificativa = c.String(maxLength: 3000, unicode: false),
                        DtAnalise = c.DateTime(),
                        DtDescricaoAcao = c.DateTime(),
                        DtEfetivaImplementacao = c.DateTime(),
                        DtEmissao = c.DateTime(nullable: false),
                        DtEnceramento = c.DateTime(),
                        DtPrazoImplementacao = c.DateTime(),
                        DtRegistroEvidencia = c.DateTime(),
                        IdProcesso = c.Int(),
                        IdEmissor = c.Int(nullable: false),
                        IdNaoConformidade = c.Int(),
                        IdResponsavelAcaoCorretiva = c.Int(),
                        IdResponsavelAnalisar = c.Int(),
                        IdResponsavelDefinir = c.Int(),
                        IdResponsavelImplementar = c.Int(),
                        IdResponsavelIniciar = c.Int(),
                        DsEvidenciaEficaciaAcao = c.String(maxLength: 2000, unicode: false),
                        XmlMetadata = c.String(maxLength: 3000, unicode: false),
                        FlDesbloqueado = c.Byte(nullable: false),
                        FlEficaz = c.Boolean(),
                        FlStatusAntesAnulacao = c.Byte(),
                        IdResponsavelEtapa = c.Int(nullable: false),
                        FlStatus = c.Byte(),
                        IdUsuarioIncluiu = c.Int(nullable: false),
                        IdUsuarioAlterou = c.Int(nullable: false),
                        DtInclusao = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(),
                        FlProcedente = c.Boolean(),
                        FlCorrecao = c.Boolean(),
                        DsJustificativaAnulacao = c.String(maxLength: 3000, unicode: false),
                        IdTipoNaoConformidade = c.Int(),
                        DsDescricaoCausa = c.String(maxLength: 3000, unicode: false),
                        IdRegistroFilho = c.Int(),
                        FlNecessitaAcaoCorretiva = c.Boolean(),
                        FlNaoConfirmidadeAuditoria = c.Boolean(),
                        IdResponsavelInicarAcaoCorretiva = c.Int(),
                        DsTags = c.String(maxLength: 3000, unicode: false),
                    })
                .PrimaryKey(t => t.IdRegistro)
                .ForeignKey("dbo.Usuario", t => t.IdEmissor)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelAcaoCorretiva)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelAnalisar)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelDefinir)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelImplementar)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelInicarAcaoCorretiva)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .ForeignKey("dbo.Cadastro", t => t.IdTipoNaoConformidade)
                .Index(t => t.IdSite)
                .Index(t => t.IdProcesso)
                .Index(t => t.IdEmissor)
                .Index(t => t.IdResponsavelAcaoCorretiva)
                .Index(t => t.IdResponsavelAnalisar)
                .Index(t => t.IdResponsavelDefinir)
                .Index(t => t.IdResponsavelImplementar)
                .Index(t => t.IdTipoNaoConformidade)
                .Index(t => t.IdResponsavelInicarAcaoCorretiva);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        IdUsuario = c.Int(nullable: false, identity: true),
                        IdPerfil = c.Int(nullable: false),
                        NmCompleto = c.String(nullable: false, maxLength: 30, unicode: false),
                        CdIdentificacao = c.String(nullable: false, maxLength: 120, unicode: false),
                        NuCPF = c.String(maxLength: 30, unicode: false),
                        CdSenha = c.String(nullable: false, maxLength: 120, unicode: false),
                        DtExpiracao = c.DateTime(),
                        NmFuncao = c.String(maxLength: 100, unicode: false),
                        FlCompartilhado = c.Boolean(nullable: false),
                        FlRecebeEmail = c.Boolean(nullable: false),
                        FlBloqueado = c.Boolean(nullable: false),
                        FlAtivo = c.Boolean(nullable: false),
                        FlSexo = c.String(nullable: false, maxLength: 1, unicode: false),
                        DtUltimoAcesso = c.DateTime(),
                        NuFalhaLNoLogin = c.Int(nullable: false),
                        DtAlteracaoSenha = c.DateTime(nullable: false),
                        IdUsuarioIncluiu = c.Int(),
                        DtInclusao = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdUsuario);
            
            CreateTable(
                "dbo.CriterioQualificacao",
                c => new
                    {
                        IdCriterioQualificacao = c.Int(nullable: false, identity: true),
                        IdProduto = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 3000, unicode: false),
                        TemControleVencimento = c.Boolean(nullable: false),
                        DtVencimento = c.DateTime(),
                        DtCriacao = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(nullable: false),
                        ArquivoEvidencia = c.String(maxLength: 3000, unicode: false),
                        Aprovado = c.Boolean(),
                        Observacoes = c.String(maxLength: 3000, unicode: false),
                        IdResponsavelPorControlarVencimento = c.Int(),
                        IdResponsavelPorQualificar = c.Int(),
                        DtEmissao = c.DateTime(),
                        DtAlteracaoEmissao = c.DateTime(),
                        DtQualificacaoVencimento = c.DateTime(),
                        NumeroDocumento = c.String(maxLength: 3000, unicode: false),
                        OrgaoExpedidor = c.String(maxLength: 3000, unicode: false),
                        ObservacoesDocumento = c.String(maxLength: 3000, unicode: false),
                    })
                .PrimaryKey(t => t.IdCriterioQualificacao)
                .ForeignKey("dbo.Produto", t => t.IdProduto)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelPorControlarVencimento)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelPorQualificar)
                .Index(t => t.IdProduto)
                .Index(t => t.IdResponsavelPorControlarVencimento)
                .Index(t => t.IdResponsavelPorQualificar);
            
            CreateTable(
                "dbo.Produto",
                c => new
                    {
                        IdProduto = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        IdResponsavel = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 3000, unicode: false),
                        Especificacao = c.String(nullable: false, maxLength: 3000, unicode: false),
                        Tags = c.String(nullable: false, maxLength: 3000, unicode: false),
                        DtCriacao = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        MinAprovado = c.Int(nullable: false),
                        MaxAprovado = c.Int(nullable: false),
                        MinAprovadoAnalise = c.Int(nullable: false),
                        MaxAprovadoAnalise = c.Int(nullable: false),
                        MinReprovado = c.Int(nullable: false),
                        MaxReprovado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProduto)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavel)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .Index(t => t.IdSite)
                .Index(t => t.IdResponsavel);
            
            CreateTable(
                "dbo.AvaliacaoCriticidade",
                c => new
                    {
                        IdAvaliacaoCriticidade = c.Int(nullable: false, identity: true),
                        IdProduto = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 3000, unicode: false),
                        DtCriacao = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdAvaliacaoCriticidade)
                .ForeignKey("dbo.Produto", t => t.IdProduto)
                .Index(t => t.IdProduto);
            
            CreateTable(
                "dbo.CriterioAvaliacao",
                c => new
                    {
                        IdCriterioAvaliacao = c.Int(nullable: false, identity: true),
                        Titulo = c.String(nullable: false, maxLength: 3000, unicode: false),
                        DtCriacao = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(nullable: false),
                        NotaAvaliacao = c.Int(),
                        DtNotaAvaliacao = c.DateTime(),
                        DtNotaAvaliacaoAlteracao = c.DateTime(),
                        IdProduto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCriterioAvaliacao)
                .ForeignKey("dbo.Produto", t => t.IdProduto)
                .Index(t => t.IdProduto);
            
            CreateTable(
                "dbo.ProdutoFornecedor",
                c => new
                    {
                        IdProdutoFornecedor = c.Int(nullable: false, identity: true),
                        IdProduto = c.Int(nullable: false),
                        IdFornecedor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProdutoFornecedor)
                .ForeignKey("dbo.Fornecedor", t => t.IdFornecedor, cascadeDelete: true)
                .ForeignKey("dbo.Produto", t => t.IdProduto, cascadeDelete: true)
                .Index(t => t.IdProduto)
                .Index(t => t.IdFornecedor);
            
            CreateTable(
                "dbo.Fornecedor",
                c => new
                    {
                        IdFornecedor = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 3000, unicode: false),
                        Telefone = c.String(nullable: false, maxLength: 3000, unicode: false),
                        Email = c.String(nullable: false, maxLength: 3000, unicode: false),
                        IdSite = c.Int(nullable: false),
                        IdProcesso = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdFornecedor)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .Index(t => t.IdSite)
                .Index(t => t.IdProcesso);
            
            CreateTable(
                "dbo.Processo",
                c => new
                    {
                        IdProcesso = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        IdUsuarioIncluiu = c.Int(),
                        Atividade = c.String(maxLength: 3000, unicode: false),
                        DocumentosAplicaveis = c.String(maxLength: 3000, unicode: false),
                        DataCadastro = c.DateTime(),
                        DataAlteracao = c.DateTime(),
                        NmProcesso = c.String(nullable: false, maxLength: 3000, unicode: false),
                        FlAtivo = c.Boolean(nullable: false),
                        FlQualidade = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdProcesso)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .Index(t => t.IdSite);
            
            CreateTable(
                "dbo.NormaProcesso",
                c => new
                    {
                        IdNormaProcesso = c.Int(nullable: false, identity: true),
                        IdProcesso = c.Int(nullable: false),
                        Requisitos = c.String(maxLength: 3000, unicode: false),
                        IdNorma = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdNormaProcesso)
                .ForeignKey("dbo.Norma", t => t.IdNorma)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .Index(t => t.IdProcesso)
                .Index(t => t.IdNorma);
            
            CreateTable(
                "dbo.Norma",
                c => new
                    {
                        IdNorma = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        IdUsuarioIncluiu = c.Int(nullable: false),
                        Item = c.String(nullable: false, maxLength: 3000, unicode: false),
                        Codigo = c.String(nullable: false, maxLength: 3000, unicode: false),
                        Titulo = c.String(nullable: false, maxLength: 3000, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdNorma);
            
            CreateTable(
                "dbo.Site",
                c => new
                    {
                        IdSite = c.Int(nullable: false, identity: true),
                        IdCliente = c.Int(nullable: false),
                        NmLogo = c.String(maxLength: 40, unicode: false),
                        NmFantasia = c.String(nullable: false, maxLength: 40, unicode: false),
                        NmRazaoSocial = c.String(nullable: false, maxLength: 60, unicode: false),
                        NuCNPJ = c.String(maxLength: 14, unicode: false),
                        DsObservacoes = c.String(maxLength: 2000, unicode: false),
                        DsFrase = c.String(maxLength: 150, unicode: false),
                        FlAtivo = c.Boolean(nullable: false),
                        IdUsuarioIncluiu = c.Int(),
                        DtInclusao = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdSite)
                .ForeignKey("dbo.Cliente", t => t.IdCliente)
                .Index(t => t.IdCliente);
            
            CreateTable(
                "dbo.Cargo",
                c => new
                    {
                        IdCargo = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        NmNome = c.String(nullable: false, maxLength: 40, unicode: false),
                        IdUsuarioIncluiu = c.Int(),
                        DtInclusao = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdCargo)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .Index(t => t.IdSite);
            
            CreateTable(
                "dbo.CargoProcesso",
                c => new
                    {
                        IdCargoProcesso = c.Int(nullable: false, identity: true),
                        IdProcesso = c.Int(nullable: false),
                        IdCargo = c.Int(nullable: false),
                        IdFuncao = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCargoProcesso)
                .ForeignKey("dbo.Cargo", t => t.IdCargo)
                .ForeignKey("dbo.Funcao", t => t.IdFuncao)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .Index(t => t.IdProcesso)
                .Index(t => t.IdCargo)
                .Index(t => t.IdFuncao);
            
            CreateTable(
                "dbo.Funcao",
                c => new
                    {
                        IdFuncao = c.Int(nullable: false, identity: true),
                        IdFuncionalidade = c.Int(nullable: false),
                        NmNome = c.String(nullable: false, maxLength: 40, unicode: false),
                        NuOrdem = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdFuncao)
                .ForeignKey("dbo.Funcionalidade", t => t.IdFuncionalidade)
                .Index(t => t.IdFuncionalidade);
            
            CreateTable(
                "dbo.Funcionalidade",
                c => new
                    {
                        IdFuncionalidade = c.Int(nullable: false, identity: true),
                        NmNome = c.String(nullable: false, maxLength: 30, unicode: false),
                        IdFuncionalidadePai = c.Int(),
                        NuOrdem = c.Int(nullable: false),
                        CdFormulario = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.IdFuncionalidade);
            
            CreateTable(
                "dbo.Notificacao",
                c => new
                    {
                        IdNotificacao = c.Int(nullable: false, identity: true),
                        IdUsuario = c.Int(nullable: false),
                        IdProcesso = c.Int(nullable: false),
                        IdSite = c.Int(nullable: false),
                        IdRelacionado = c.Int(nullable: false),
                        IdFuncionalidade = c.Int(nullable: false),
                        TpNotificacao = c.String(nullable: false, maxLength: 2, unicode: false),
                        DtVencimento = c.DateTime(),
                        NuDiasAntecedencia = c.Int(nullable: false),
                        DtEnvioFilaDisparo = c.DateTime(),
                        DsDescricao = c.String(maxLength: 1000, unicode: false),
                        FlEtapa = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.IdNotificacao)
                .ForeignKey("dbo.Funcionalidade", t => t.IdFuncionalidade)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario)
                .Index(t => t.IdUsuario)
                .Index(t => t.IdProcesso)
                .Index(t => t.IdSite)
                .Index(t => t.IdFuncionalidade);
            
            CreateTable(
                "dbo.SiteModulo",
                c => new
                    {
                        IdSiteModulo = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        IdModulo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSiteModulo)
                .ForeignKey("dbo.Funcionalidade", t => t.IdModulo)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .Index(t => t.IdSite)
                .Index(t => t.IdModulo);
            
            CreateTable(
                "dbo.UsuarioCargo",
                c => new
                    {
                        IdUsuarioProcesso = c.Int(nullable: false, identity: true),
                        IdUsuario = c.Int(nullable: false),
                        IdCargo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdUsuarioProcesso)
                .ForeignKey("dbo.Cargo", t => t.IdCargo)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario)
                .Index(t => t.IdUsuario)
                .Index(t => t.IdCargo);
            
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        IdCliente = c.Int(nullable: false, identity: true),
                        NmFantasia = c.String(nullable: false, maxLength: 40, unicode: false),
                        NmLogo = c.String(maxLength: 80, unicode: false),
                        NmAquivoContrato = c.String(maxLength: 80, unicode: false),
                        NmUrlAcesso = c.String(nullable: false, maxLength: 60, unicode: false),
                        DtValidadeContrato = c.DateTime(),
                        NuDiasTrocaSenha = c.Int(nullable: false),
                        NuTentativaBloqueioLogin = c.Int(),
                        NuArmazenaSenha = c.Int(),
                        FlExigeSenhaForte = c.Boolean(),
                        FlTemCaptcha = c.Boolean(nullable: false),
                        FlEstruturaAprovador = c.Boolean(),
                        FlAtivo = c.Boolean(nullable: false),
                        IdUsuarioIncluiu = c.Int(),
                        DtInclusao = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdCliente);
            
            CreateTable(
                "dbo.UsuarioClienteSite",
                c => new
                    {
                        IdUsuarioClienteSite = c.Int(nullable: false, identity: true),
                        IdCliente = c.Int(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        IdSite = c.Int(),
                    })
                .PrimaryKey(t => t.IdUsuarioClienteSite)
                .ForeignKey("dbo.Cliente", t => t.IdCliente)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario)
                .Index(t => t.IdCliente)
                .Index(t => t.IdUsuario)
                .Index(t => t.IdSite);
            
            CreateTable(
                "dbo.Cadastro",
                c => new
                    {
                        IdCadastro = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        DsDescricao = c.String(nullable: false, maxLength: 70, unicode: false),
                        CdTabela = c.String(nullable: false, maxLength: 10, unicode: false),
                        FlAtivo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdCadastro)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .Index(t => t.IdSite);
            
            CreateTable(
                "dbo.DocDocumento",
                c => new
                    {
                        IdDocumento = c.Int(nullable: false, identity: true),
                        IdDocumentoPai = c.Int(),
                        IdSite = c.Int(nullable: false),
                        IdDocIdentificador = c.Int(nullable: false),
                        IdProcesso = c.Int(),
                        IdCategoria = c.Int(nullable: false),
                        NmTitulo = c.String(maxLength: 1000, unicode: false),
                        CdLetra = c.String(maxLength: 15, unicode: false),
                        CdNumero = c.String(maxLength: 15, unicode: false),
                        NuRevisao = c.Byte(),
                        IdElaborador = c.Int(nullable: false),
                        DsComentario = c.String(maxLength: 3000, unicode: false),
                        DsAssunto = c.String(maxLength: 3000, unicode: false),
                        DtPedidoVerificacao = c.DateTime(),
                        DtVerificacao = c.DateTime(),
                        DtVencimento = c.DateTime(),
                        DtEmissao = c.DateTime(),
                        DtPedidoAprovacao = c.DateTime(),
                        DtAprovacao = c.DateTime(),
                        DtNotificacao = c.DateTime(),
                        XmlMetadata = c.String(maxLength: 3000, unicode: false),
                        FlWorkFlow = c.Boolean(nullable: false),
                        FlRevisaoPeriodica = c.Boolean(nullable: false),
                        FlStatus = c.Byte(nullable: false),
                        IdUsuarioIncluiu = c.Int(),
                        DtInclusao = c.DateTime(),
                        DtAlteracao = c.DateTime(nullable: false),
                        Ativo = c.Boolean(),
                        Risco = c.String(maxLength: 3000, unicode: false),
                        PossuiGestaoRisco = c.Boolean(),
                        IdRegistro = c.Int(),
                    })
                .PrimaryKey(t => t.IdDocumento)
                .ForeignKey("dbo.Registros", t => t.IdRegistro)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .ForeignKey("dbo.Usuario", t => t.IdElaborador)
                .Index(t => t.IdSite)
                .Index(t => t.IdProcesso)
                .Index(t => t.IdElaborador)
                .Index(t => t.IdRegistro);
            
            CreateTable(
                "dbo.DocCargo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdDocumento = c.Int(),
                        IdCargo = c.Int(),
                        IdUsuarioIncluiu = c.Int(),
                        DtInclusao = c.DateTime(),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocDocumento", t => t.IdDocumento)
                .Index(t => t.IdDocumento);
            
            CreateTable(
                "dbo.DocTemplate",
                c => new
                    {
                        IdDocTemplate = c.Int(nullable: false, identity: true),
                        IdDocumento = c.Int(nullable: false),
                        TpTemplate = c.String(maxLength: 2, unicode: false),
                        ATIVO = c.Boolean(),
                    })
                .PrimaryKey(t => t.IdDocTemplate)
                .ForeignKey("dbo.DocDocumento", t => t.IdDocumento)
                .Index(t => t.IdDocumento);
            
            CreateTable(
                "dbo.DocUsuarioVerificaAprova",
                c => new
                    {
                        IdDocUsuarioVerificaAprova = c.Int(nullable: false, identity: true),
                        IdDocumento = c.Int(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        TpEtapa = c.String(nullable: false, maxLength: 2, unicode: false),
                        FlAprovou = c.Boolean(),
                        FlVerificou = c.Boolean(),
                        Ativo = c.Boolean(),
                    })
                .PrimaryKey(t => t.IdDocUsuarioVerificaAprova)
                .ForeignKey("dbo.DocDocumento", t => t.IdDocumento)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario)
                .Index(t => t.IdDocumento)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.NotificacaoMensagem",
                c => new
                    {
                        IdNotificacaoMenssagem = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        NmEmailPara = c.String(nullable: false, maxLength: 150, unicode: false),
                        NmEmailNome = c.String(nullable: false, maxLength: 60, unicode: false),
                        DsMensagem = c.String(maxLength: 3000, unicode: false),
                        DtCadastro = c.DateTime(nullable: false),
                        DtEnvio = c.DateTime(),
                        IdSmtpNotificacao = c.Int(nullable: false),
                        FlEnviada = c.Boolean(),
                        DsAssunto = c.String(maxLength: 300, unicode: false),
                    })
                .PrimaryKey(t => t.IdNotificacaoMenssagem)
                .ForeignKey("dbo.NotificacaoSmtp", t => t.IdSmtpNotificacao)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .Index(t => t.IdSite)
                .Index(t => t.IdSmtpNotificacao);
            
            CreateTable(
                "dbo.NotificacaoSmtp",
                c => new
                    {
                        IdSmptNotificacao = c.Int(nullable: false, identity: true),
                        DsSmtp = c.String(maxLength: 1000, unicode: false),
                        NuPorta = c.Int(nullable: false),
                        CdUsuario = c.String(maxLength: 3000, unicode: false),
                        CdSenha = c.String(maxLength: 3000, unicode: false),
                        NmNome = c.String(maxLength: 3000, unicode: false),
                        FlAtivo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdSmptNotificacao);
            
            CreateTable(
                "dbo.AnaliseCritica",
                c => new
                    {
                        IdAnaliseCritica = c.Int(nullable: false, identity: true),
                        Ata = c.Int(nullable: false),
                        DataCriacao = c.DateTime(nullable: false),
                        DataProximaAnalise = c.DateTime(nullable: false),
                        IdSite = c.Int(nullable: false),
                        IdResponsavel = c.Int(nullable: false),
                        DataCadastro = c.DateTime(),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdAnaliseCritica)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavel)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .Index(t => t.IdSite)
                .Index(t => t.IdResponsavel);
            
            CreateTable(
                "dbo.AnaliseCriticaFuncionario",
                c => new
                    {
                        IdAnaliseCriticaFuncionario = c.Int(nullable: false, identity: true),
                        IdUsuario = c.Int(nullable: false),
                        IdAnaliseCritica = c.Int(nullable: false),
                        Funcao = c.String(nullable: false, maxLength: 3000, unicode: false),
                        DataCadastro = c.DateTime(),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdAnaliseCriticaFuncionario)
                .ForeignKey("dbo.AnaliseCritica", t => t.IdAnaliseCritica)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario)
                .Index(t => t.IdUsuario)
                .Index(t => t.IdAnaliseCritica);
            
            CreateTable(
                "dbo.AnaliseCriticaTema",
                c => new
                    {
                        IdTema = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 3000, unicode: false),
                        Risco = c.String(nullable: false, maxLength: 3000, unicode: false),
                        PossuiGestaoRisco = c.Boolean(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        IdControladorCategoria = c.Int(nullable: false),
                        IdRegistro = c.Int(),
                        IdAnaliseCritica = c.Int(nullable: false),
                        DataCadastro = c.DateTime(),
                        Ativo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdTema)
                .ForeignKey("dbo.AnaliseCritica", t => t.IdAnaliseCritica)
                .ForeignKey("dbo.Cadastro", t => t.IdControladorCategoria)
                .ForeignKey("dbo.Registros", t => t.IdRegistro)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario)
                .Index(t => t.IdUsuario)
                .Index(t => t.IdControladorCategoria)
                .Index(t => t.IdRegistro)
                .Index(t => t.IdAnaliseCritica);
            
            CreateTable(
                "dbo.Calibracao",
                c => new
                    {
                        IdCalibracao = c.Int(nullable: false, identity: true),
                        IdInstrumento = c.Int(),
                        Certificado = c.String(maxLength: 3000, unicode: false),
                        OrgaoCalibrador = c.String(maxLength: 3000, unicode: false),
                        Aprovado = c.Byte(nullable: false),
                        Aprovador = c.Int(nullable: false),
                        Observacoes = c.String(maxLength: 3000, unicode: false),
                        ArquivoCertificado = c.String(maxLength: 3000, unicode: false),
                        DataCalibracao = c.DateTime(nullable: false),
                        DataProximaCalibracao = c.DateTime(nullable: false),
                        DataCriacao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(nullable: false),
                        IdUsuarioIncluiu = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCalibracao)
                .ForeignKey("dbo.Instrumento", t => t.IdInstrumento)
                .Index(t => t.IdInstrumento);
            
            CreateTable(
                "dbo.CriterioAceitacao",
                c => new
                    {
                        IdCriterioAceitacao = c.Int(nullable: false, identity: true),
                        Periodicidade = c.String(maxLength: 3000, unicode: false),
                        Erro = c.Double(),
                        Incerteza = c.Double(),
                        Resultado = c.Double(),
                        Aceito = c.Boolean(),
                        IdUsuarioIncluiu = c.Int(),
                        DataInclusao = c.DateTime(),
                        DataAlteracao = c.DateTime(nullable: false),
                        IdCalibracao = c.Int(),
                    })
                .PrimaryKey(t => t.IdCriterioAceitacao)
                .ForeignKey("dbo.Calibracao", t => t.IdCalibracao)
                .Index(t => t.IdCalibracao);
            
            CreateTable(
                "dbo.Instrumento",
                c => new
                    {
                        IdInstrumento = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        IdProcesso = c.Int(nullable: false),
                        IdResponsavel = c.Int(),
                        Equipamento = c.String(maxLength: 3000, unicode: false),
                        Numero = c.String(maxLength: 3000, unicode: false),
                        LocalDeUso = c.String(maxLength: 3000, unicode: false),
                        Marca = c.String(maxLength: 3000, unicode: false),
                        Modelo = c.String(maxLength: 3000, unicode: false),
                        Periodicidade = c.Byte(nullable: false),
                        Escala = c.String(maxLength: 3000, unicode: false),
                        CriterioAceitacao = c.String(maxLength: 3000, unicode: false),
                        MenorDivisao = c.String(maxLength: 3000, unicode: false),
                        Status = c.Byte(),
                        DataCriacao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(nullable: false),
                        IdUsuarioIncluiu = c.Int(nullable: false),
                        SistemaDefineStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdInstrumento)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .Index(t => t.IdSite)
                .Index(t => t.IdProcesso);
            
            CreateTable(
                "dbo.Perfil",
                c => new
                    {
                        IdPerfil = c.Int(nullable: false, identity: true),
                        NmNome = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.IdPerfil);
            
            CreateTable(
                "dbo.Endereco",
                c => new
                    {
                        IdEndereco = c.Int(nullable: false, identity: true),
                        IdRelacionado = c.Int(nullable: false),
                        FlTipoEndereco = c.String(nullable: false, maxLength: 3, unicode: false),
                        DsLogradouro = c.String(nullable: false, maxLength: 60, unicode: false),
                        NuNumero = c.String(nullable: false, maxLength: 15, unicode: false),
                        DsComplemento = c.String(maxLength: 30, unicode: false),
                        NmBairro = c.String(nullable: false, maxLength: 30, unicode: false),
                        NmCidade = c.String(nullable: false, maxLength: 30, unicode: false),
                        CdEstado = c.String(nullable: false, maxLength: 2, unicode: false),
                        NuCep = c.String(nullable: false, maxLength: 9, unicode: false),
                        DsPais = c.String(maxLength: 32, unicode: false),
                    })
                .PrimaryKey(t => t.IdEndereco);
            
            CreateTable(
                "dbo.ListaValor",
                c => new
                    {
                        IdListaValor = c.Int(nullable: false, identity: true),
                        CdTabela = c.String(nullable: false, maxLength: 10, unicode: false),
                        CdCodigo = c.String(nullable: false, maxLength: 40, unicode: false),
                        DsDescricao = c.String(nullable: false, maxLength: 3000, unicode: false),
                        CdCulture = c.String(maxLength: 5, unicode: false),
                    })
                .PrimaryKey(t => t.IdListaValor);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        IdLog = c.Int(nullable: false, identity: true),
                        IdAcao = c.Int(nullable: false),
                        IP = c.String(maxLength: 3000, unicode: false),
                        Browser = c.String(maxLength: 3000, unicode: false),
                        Mensagem = c.String(nullable: false, maxLength: 3000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        IdUsuario = c.Int(),
                    })
                .PrimaryKey(t => t.IdLog)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.Plai",
                c => new
                    {
                        IdPlai = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        IdRepresentanteDaDirecao = c.Int(nullable: false),
                        Agendado = c.Boolean(nullable: false),
                        Mes = c.Int(nullable: false),
                        EnviouEmail = c.Boolean(nullable: false),
                        Arquivo = c.Binary(),
                        Endereco = c.String(nullable: false, maxLength: 3000, unicode: false),
                        Escopo = c.String(nullable: false, maxLength: 3000, unicode: false),
                        Gestores = c.String(nullable: false, maxLength: 3000, unicode: false),
                        DataReuniaoAbertura = c.DateTime(nullable: false),
                        DataReuniaoEncerramento = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                        DataCadastro = c.DateTime(nullable: false),
                        Bloqueado = c.Boolean(nullable: false),
                        IdPai = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPlai)
                .ForeignKey("dbo.Pai", t => t.IdPai)
                .Index(t => t.IdPai);
            
            CreateTable(
                "dbo.Pai",
                c => new
                    {
                        IdPai = c.Int(nullable: false, identity: true),
                        Ano = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        IdGestor = c.Int(nullable: false),
                        IdSite = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPai)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .ForeignKey("dbo.Usuario", t => t.IdGestor)
                .Index(t => t.IdGestor)
                .Index(t => t.IdSite);
            
            CreateTable(
                "dbo.PlaiProcessoNorma",
                c => new
                    {
                        IdPlaiProcessoNorma = c.Int(nullable: false, identity: true),
                        IdPlai = c.Int(nullable: false),
                        IdProcesso = c.Int(nullable: false),
                        IdNorma = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdPlaiProcessoNorma)
                .ForeignKey("dbo.Norma", t => t.IdNorma)
                .ForeignKey("dbo.Plai", t => t.IdPlai)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .Index(t => t.IdPlai)
                .Index(t => t.IdProcesso)
                .Index(t => t.IdNorma);
            
            CreateTable(
                "dbo.Telefone",
                c => new
                    {
                        IdTelefone = c.Int(nullable: false, identity: true),
                        IdRelacionado = c.Int(nullable: false),
                        FlTipoTelefone = c.String(nullable: false, maxLength: 3, unicode: false),
                        NuTelefone = c.String(nullable: false, maxLength: 15, unicode: false),
                        NuRamal = c.String(maxLength: 15, unicode: false),
                        DsObservacao = c.String(maxLength: 300, unicode: false),
                    })
                .PrimaryKey(t => t.IdTelefone);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlaiProcessoNorma", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.PlaiProcessoNorma", "IdPlai", "dbo.Plai");
            DropForeignKey("dbo.PlaiProcessoNorma", "IdNorma", "dbo.Norma");
            DropForeignKey("dbo.Plai", "IdPai", "dbo.Pai");
            DropForeignKey("dbo.Pai", "IdGestor", "dbo.Usuario");
            DropForeignKey("dbo.Pai", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Log", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.Calibracao", "IdInstrumento", "dbo.Instrumento");
            DropForeignKey("dbo.Instrumento", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Instrumento", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.CriterioAceitacao", "IdCalibracao", "dbo.Calibracao");
            DropForeignKey("dbo.AnaliseCriticaTema", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.AnaliseCriticaTema", "IdRegistro", "dbo.Registros");
            DropForeignKey("dbo.AnaliseCriticaTema", "IdControladorCategoria", "dbo.Cadastro");
            DropForeignKey("dbo.AnaliseCriticaTema", "IdAnaliseCritica", "dbo.AnaliseCritica");
            DropForeignKey("dbo.AnaliseCritica", "IdSite", "dbo.Site");
            DropForeignKey("dbo.AnaliseCritica", "IdResponsavel", "dbo.Usuario");
            DropForeignKey("dbo.AnaliseCriticaFuncionario", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.AnaliseCriticaFuncionario", "IdAnaliseCritica", "dbo.AnaliseCritica");
            DropForeignKey("dbo.RegistroAcaoImediata", "IdReponsavelImplementar", "dbo.Usuario");
            DropForeignKey("dbo.RegistroAcaoImediata", "IdRegistro", "dbo.Registros");
            DropForeignKey("dbo.Registros", "IdTipoNaoConformidade", "dbo.Cadastro");
            DropForeignKey("dbo.Registros", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Registros", "IdResponsavelInicarAcaoCorretiva", "dbo.Usuario");
            DropForeignKey("dbo.Registros", "IdResponsavelImplementar", "dbo.Usuario");
            DropForeignKey("dbo.Registros", "IdResponsavelDefinir", "dbo.Usuario");
            DropForeignKey("dbo.Registros", "IdResponsavelAnalisar", "dbo.Usuario");
            DropForeignKey("dbo.Registros", "IdResponsavelAcaoCorretiva", "dbo.Usuario");
            DropForeignKey("dbo.Registros", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.Registros", "IdEmissor", "dbo.Usuario");
            DropForeignKey("dbo.CriterioQualificacao", "IdResponsavelPorQualificar", "dbo.Usuario");
            DropForeignKey("dbo.CriterioQualificacao", "IdResponsavelPorControlarVencimento", "dbo.Usuario");
            DropForeignKey("dbo.CriterioQualificacao", "IdProduto", "dbo.Produto");
            DropForeignKey("dbo.Produto", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Produto", "IdResponsavel", "dbo.Usuario");
            DropForeignKey("dbo.ProdutoFornecedor", "IdProduto", "dbo.Produto");
            DropForeignKey("dbo.ProdutoFornecedor", "IdFornecedor", "dbo.Fornecedor");
            DropForeignKey("dbo.Fornecedor", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Fornecedor", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.Processo", "IdSite", "dbo.Site");
            DropForeignKey("dbo.NotificacaoMensagem", "IdSite", "dbo.Site");
            DropForeignKey("dbo.NotificacaoMensagem", "IdSmtpNotificacao", "dbo.NotificacaoSmtp");
            DropForeignKey("dbo.DocDocumento", "IdElaborador", "dbo.Usuario");
            DropForeignKey("dbo.DocDocumento", "IdSite", "dbo.Site");
            DropForeignKey("dbo.DocDocumento", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.DocDocumento", "IdRegistro", "dbo.Registros");
            DropForeignKey("dbo.DocUsuarioVerificaAprova", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.DocUsuarioVerificaAprova", "IdDocumento", "dbo.DocDocumento");
            DropForeignKey("dbo.DocTemplate", "IdDocumento", "dbo.DocDocumento");
            DropForeignKey("dbo.DocCargo", "IdDocumento", "dbo.DocDocumento");
            DropForeignKey("dbo.Cadastro", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Site", "IdCliente", "dbo.Cliente");
            DropForeignKey("dbo.UsuarioClienteSite", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioClienteSite", "IdSite", "dbo.Site");
            DropForeignKey("dbo.UsuarioClienteSite", "IdCliente", "dbo.Cliente");
            DropForeignKey("dbo.UsuarioCargo", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioCargo", "IdCargo", "dbo.Cargo");
            DropForeignKey("dbo.Cargo", "IdSite", "dbo.Site");
            DropForeignKey("dbo.CargoProcesso", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.CargoProcesso", "IdFuncao", "dbo.Funcao");
            DropForeignKey("dbo.Funcao", "IdFuncionalidade", "dbo.Funcionalidade");
            DropForeignKey("dbo.SiteModulo", "IdSite", "dbo.Site");
            DropForeignKey("dbo.SiteModulo", "IdModulo", "dbo.Funcionalidade");
            DropForeignKey("dbo.Notificacao", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.Notificacao", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Notificacao", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.Notificacao", "IdFuncionalidade", "dbo.Funcionalidade");
            DropForeignKey("dbo.CargoProcesso", "IdCargo", "dbo.Cargo");
            DropForeignKey("dbo.NormaProcesso", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.NormaProcesso", "IdNorma", "dbo.Norma");
            DropForeignKey("dbo.CriterioAvaliacao", "IdProduto", "dbo.Produto");
            DropForeignKey("dbo.AvaliacaoCriticidade", "IdProduto", "dbo.Produto");
            DropIndex("dbo.PlaiProcessoNorma", new[] { "IdNorma" });
            DropIndex("dbo.PlaiProcessoNorma", new[] { "IdProcesso" });
            DropIndex("dbo.PlaiProcessoNorma", new[] { "IdPlai" });
            DropIndex("dbo.Pai", new[] { "IdSite" });
            DropIndex("dbo.Pai", new[] { "IdGestor" });
            DropIndex("dbo.Plai", new[] { "IdPai" });
            DropIndex("dbo.Log", new[] { "IdUsuario" });
            DropIndex("dbo.Instrumento", new[] { "IdProcesso" });
            DropIndex("dbo.Instrumento", new[] { "IdSite" });
            DropIndex("dbo.CriterioAceitacao", new[] { "IdCalibracao" });
            DropIndex("dbo.Calibracao", new[] { "IdInstrumento" });
            DropIndex("dbo.AnaliseCriticaTema", new[] { "IdAnaliseCritica" });
            DropIndex("dbo.AnaliseCriticaTema", new[] { "IdRegistro" });
            DropIndex("dbo.AnaliseCriticaTema", new[] { "IdControladorCategoria" });
            DropIndex("dbo.AnaliseCriticaTema", new[] { "IdUsuario" });
            DropIndex("dbo.AnaliseCriticaFuncionario", new[] { "IdAnaliseCritica" });
            DropIndex("dbo.AnaliseCriticaFuncionario", new[] { "IdUsuario" });
            DropIndex("dbo.AnaliseCritica", new[] { "IdResponsavel" });
            DropIndex("dbo.AnaliseCritica", new[] { "IdSite" });
            DropIndex("dbo.NotificacaoMensagem", new[] { "IdSmtpNotificacao" });
            DropIndex("dbo.NotificacaoMensagem", new[] { "IdSite" });
            DropIndex("dbo.DocUsuarioVerificaAprova", new[] { "IdUsuario" });
            DropIndex("dbo.DocUsuarioVerificaAprova", new[] { "IdDocumento" });
            DropIndex("dbo.DocTemplate", new[] { "IdDocumento" });
            DropIndex("dbo.DocCargo", new[] { "IdDocumento" });
            DropIndex("dbo.DocDocumento", new[] { "IdRegistro" });
            DropIndex("dbo.DocDocumento", new[] { "IdElaborador" });
            DropIndex("dbo.DocDocumento", new[] { "IdProcesso" });
            DropIndex("dbo.DocDocumento", new[] { "IdSite" });
            DropIndex("dbo.Cadastro", new[] { "IdSite" });
            DropIndex("dbo.UsuarioClienteSite", new[] { "IdSite" });
            DropIndex("dbo.UsuarioClienteSite", new[] { "IdUsuario" });
            DropIndex("dbo.UsuarioClienteSite", new[] { "IdCliente" });
            DropIndex("dbo.UsuarioCargo", new[] { "IdCargo" });
            DropIndex("dbo.UsuarioCargo", new[] { "IdUsuario" });
            DropIndex("dbo.SiteModulo", new[] { "IdModulo" });
            DropIndex("dbo.SiteModulo", new[] { "IdSite" });
            DropIndex("dbo.Notificacao", new[] { "IdFuncionalidade" });
            DropIndex("dbo.Notificacao", new[] { "IdSite" });
            DropIndex("dbo.Notificacao", new[] { "IdProcesso" });
            DropIndex("dbo.Notificacao", new[] { "IdUsuario" });
            DropIndex("dbo.Funcao", new[] { "IdFuncionalidade" });
            DropIndex("dbo.CargoProcesso", new[] { "IdFuncao" });
            DropIndex("dbo.CargoProcesso", new[] { "IdCargo" });
            DropIndex("dbo.CargoProcesso", new[] { "IdProcesso" });
            DropIndex("dbo.Cargo", new[] { "IdSite" });
            DropIndex("dbo.Site", new[] { "IdCliente" });
            DropIndex("dbo.NormaProcesso", new[] { "IdNorma" });
            DropIndex("dbo.NormaProcesso", new[] { "IdProcesso" });
            DropIndex("dbo.Processo", new[] { "IdSite" });
            DropIndex("dbo.Fornecedor", new[] { "IdProcesso" });
            DropIndex("dbo.Fornecedor", new[] { "IdSite" });
            DropIndex("dbo.ProdutoFornecedor", new[] { "IdFornecedor" });
            DropIndex("dbo.ProdutoFornecedor", new[] { "IdProduto" });
            DropIndex("dbo.CriterioAvaliacao", new[] { "IdProduto" });
            DropIndex("dbo.AvaliacaoCriticidade", new[] { "IdProduto" });
            DropIndex("dbo.Produto", new[] { "IdResponsavel" });
            DropIndex("dbo.Produto", new[] { "IdSite" });
            DropIndex("dbo.CriterioQualificacao", new[] { "IdResponsavelPorQualificar" });
            DropIndex("dbo.CriterioQualificacao", new[] { "IdResponsavelPorControlarVencimento" });
            DropIndex("dbo.CriterioQualificacao", new[] { "IdProduto" });
            DropIndex("dbo.Registros", new[] { "IdResponsavelInicarAcaoCorretiva" });
            DropIndex("dbo.Registros", new[] { "IdTipoNaoConformidade" });
            DropIndex("dbo.Registros", new[] { "IdResponsavelImplementar" });
            DropIndex("dbo.Registros", new[] { "IdResponsavelDefinir" });
            DropIndex("dbo.Registros", new[] { "IdResponsavelAnalisar" });
            DropIndex("dbo.Registros", new[] { "IdResponsavelAcaoCorretiva" });
            DropIndex("dbo.Registros", new[] { "IdEmissor" });
            DropIndex("dbo.Registros", new[] { "IdProcesso" });
            DropIndex("dbo.Registros", new[] { "IdSite" });
            DropIndex("dbo.RegistroAcaoImediata", new[] { "IdRegistro" });
            DropIndex("dbo.RegistroAcaoImediata", new[] { "IdReponsavelImplementar" });
            DropTable("dbo.Telefone");
            DropTable("dbo.PlaiProcessoNorma");
            DropTable("dbo.Pai");
            DropTable("dbo.Plai");
            DropTable("dbo.Log");
            DropTable("dbo.ListaValor");
            DropTable("dbo.Endereco");
            DropTable("dbo.Perfil");
            DropTable("dbo.Instrumento");
            DropTable("dbo.CriterioAceitacao");
            DropTable("dbo.Calibracao");
            DropTable("dbo.AnaliseCriticaTema");
            DropTable("dbo.AnaliseCriticaFuncionario");
            DropTable("dbo.AnaliseCritica");
            DropTable("dbo.NotificacaoSmtp");
            DropTable("dbo.NotificacaoMensagem");
            DropTable("dbo.DocUsuarioVerificaAprova");
            DropTable("dbo.DocTemplate");
            DropTable("dbo.DocCargo");
            DropTable("dbo.DocDocumento");
            DropTable("dbo.Cadastro");
            DropTable("dbo.UsuarioClienteSite");
            DropTable("dbo.Cliente");
            DropTable("dbo.UsuarioCargo");
            DropTable("dbo.SiteModulo");
            DropTable("dbo.Notificacao");
            DropTable("dbo.Funcionalidade");
            DropTable("dbo.Funcao");
            DropTable("dbo.CargoProcesso");
            DropTable("dbo.Cargo");
            DropTable("dbo.Site");
            DropTable("dbo.Norma");
            DropTable("dbo.NormaProcesso");
            DropTable("dbo.Processo");
            DropTable("dbo.Fornecedor");
            DropTable("dbo.ProdutoFornecedor");
            DropTable("dbo.CriterioAvaliacao");
            DropTable("dbo.AvaliacaoCriticidade");
            DropTable("dbo.Produto");
            DropTable("dbo.CriterioQualificacao");
            DropTable("dbo.Usuario");
            DropTable("dbo.Registros");
            DropTable("dbo.RegistroAcaoImediata");
        }
    }
}
