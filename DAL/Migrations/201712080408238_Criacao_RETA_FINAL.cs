namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Criacao_RETA_FINAL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegistroAcaoImediata",
                c => new
                    {
                        IdRegistroAcaoImediata = c.Int(nullable: false, identity: true),
                        DtPrazoImplementacao = c.DateTime(nullable: false),
                        DtEfetivaImplementacao = c.DateTime(),
                        DsAcao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        NmArquivoEvidencia = c.String(maxLength: 1000, unicode: false),
                        IdReponsavelImplementar = c.Int(nullable: false),
                        IdUsuarioIncluiu = c.Int(),
                        DtInclusao = c.DateTime(nullable: false),
                        IdRegistro = c.Int(nullable: false),
                        Aprovado = c.Boolean(),
                    })
                .PrimaryKey(t => t.IdRegistroAcaoImediata)
                .ForeignKey("dbo.Registros", t => t.IdRegistro, cascadeDelete: true)
                .ForeignKey("dbo.Usuario", t => t.IdReponsavelImplementar)
                .Index(t => t.IdReponsavelImplementar)
                .Index(t => t.IdRegistro);
            
            CreateTable(
                "dbo.Registros",
                c => new
                    {
                        IdRegistro = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        IdProcesso = c.Int(nullable: false),
                        IdEmissor = c.Int(nullable: false),
                        IdNaoConformidade = c.Int(),
                        IdResponsavelInicarAcaoImediata = c.Int(),
                        IdResponsavelAcaoCorretiva = c.Int(),
                        IdResponsavelDefinir = c.Int(),
                        IdResponsavelAnalisar = c.Int(),
                        IdResponsavelImplementar = c.Int(),
                        IdTipoNaoConformidade = c.Int(),
                        IdRegistroFilho = c.Int(),
                        IdResponsavelEtapa = c.Int(nullable: false),
                        IdUsuarioIncluiu = c.Int(nullable: false),
                        IdUsuarioAlterou = c.Int(nullable: false),
                        DsOqueTexto = c.String(nullable: false, maxLength: 8000, unicode: false),
                        TpRegistro = c.String(nullable: false, maxLength: 2, unicode: false),
                        NuRegistro = c.Int(nullable: false),
                        DsAcao = c.String(maxLength: 8000, unicode: false),
                        NmEvidenciaImagem = c.String(maxLength: 1000, unicode: false),
                        DsJustificativa = c.String(maxLength: 8000, unicode: false),
                        DtAnalise = c.DateTime(),
                        DtDescricaoAcao = c.DateTime(),
                        DtEfetivaImplementacao = c.DateTime(),
                        DtEmissao = c.DateTime(nullable: false),
                        DtEnceramento = c.DateTime(),
                        DtPrazoImplementacao = c.DateTime(),
                        DtRegistroEvidencia = c.DateTime(),
                        FlDesbloqueado = c.Byte(nullable: false),
                        FlEficaz = c.Boolean(),
                        FlStatusAntesAnulacao = c.Byte(),
                        FlStatus = c.Byte(),
                        DtInclusao = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(),
                        FlProcedente = c.Boolean(),
                        FlCorrecao = c.Boolean(),
                        DsJustificativaAnulacao = c.String(maxLength: 8000, unicode: false),
                        DsDescricaoCausa = c.String(maxLength: 8000, unicode: false),
                        FlNecessitaAcaoCorretiva = c.Boolean(),
                        FlNaoConfirmidadeAuditoria = c.Boolean(),
                        Tags = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.IdRegistro)
                .ForeignKey("dbo.Usuario", t => t.IdEmissor)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelAcaoCorretiva)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelAnalisar)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelEtapa)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelImplementar)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelInicarAcaoImediata)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelDefinir)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .ForeignKey("dbo.Cadastro", t => t.IdTipoNaoConformidade)
                .Index(t => t.IdSite)
                .Index(t => t.IdProcesso)
                .Index(t => t.IdEmissor)
                .Index(t => t.IdResponsavelInicarAcaoImediata)
                .Index(t => t.IdResponsavelAcaoCorretiva)
                .Index(t => t.IdResponsavelDefinir)
                .Index(t => t.IdResponsavelAnalisar)
                .Index(t => t.IdResponsavelImplementar)
                .Index(t => t.IdTipoNaoConformidade)
                .Index(t => t.IdResponsavelEtapa);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        IdUsuario = c.Int(nullable: false, identity: true),
                        Token = c.Guid(),
                        IdPerfil = c.Int(nullable: false),
                        NmCompleto = c.String(nullable: false, maxLength: 60, unicode: false),
                        CdIdentificacao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        NuCPF = c.String(maxLength: 11, unicode: false),
                        CdSenha = c.String(maxLength: 8000, unicode: false),
                        DtExpiracao = c.DateTime(),
                        NmFuncao = c.String(maxLength: 8000, unicode: false),
                        FlCompartilhado = c.Boolean(nullable: false),
                        FlRecebeEmail = c.Boolean(nullable: false),
                        FlBloqueado = c.Boolean(nullable: false),
                        FlAtivo = c.Boolean(nullable: false),
                        FlSexo = c.String(maxLength: 8000, unicode: false),
                        DtUltimoAcesso = c.DateTime(),
                        NuFalhaLNoLogin = c.Int(nullable: false),
                        DtAlteracaoSenha = c.DateTime(),
                        IdUsuarioIncluiu = c.Int(),
                        DtInclusao = c.DateTime(),
                        DtAlteracao = c.DateTime(),
                        Anexo_IdAnexo = c.Int(),
                    })
                .PrimaryKey(t => t.IdUsuario)
                .ForeignKey("dbo.Anexo", t => t.Anexo_IdAnexo)
                .Index(t => t.Anexo_IdAnexo);
            
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
                        Funcao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdAnaliseCriticaFuncionario)
                .ForeignKey("dbo.AnaliseCritica", t => t.IdAnaliseCritica)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario)
                .Index(t => t.IdUsuario)
                .Index(t => t.IdAnaliseCritica);
            
            CreateTable(
                "dbo.Site",
                c => new
                    {
                        IdSite = c.Int(nullable: false, identity: true),
                        IdCliente = c.Int(nullable: false),
                        NmFantasia = c.String(nullable: false, maxLength: 8000, unicode: false),
                        NmRazaoSocial = c.String(nullable: false, maxLength: 8000, unicode: false),
                        NuCNPJ = c.String(maxLength: 8000, unicode: false),
                        DsObservacoes = c.String(maxLength: 8000, unicode: false),
                        DsFrase = c.String(maxLength: 8000, unicode: false),
                        FlAtivo = c.Boolean(nullable: false),
                        IdUsuarioIncluiu = c.Int(),
                        DtInclusao = c.DateTime(),
                        SiteLogoAux_IdAnexo = c.Int(),
                    })
                .PrimaryKey(t => t.IdSite)
                .ForeignKey("dbo.Cliente", t => t.IdCliente)
                .ForeignKey("dbo.Anexo", t => t.SiteLogoAux_IdAnexo)
                .Index(t => t.IdCliente)
                .Index(t => t.SiteLogoAux_IdAnexo);
            
            CreateTable(
                "dbo.Cargo",
                c => new
                    {
                        IdCargo = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        NmNome = c.String(nullable: false, maxLength: 40, unicode: false),
                        FlAtivo = c.Boolean(nullable: false),
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
                        NuOrdem = c.Int(nullable: false),
                        CdFormulario = c.String(maxLength: 20, unicode: false),
                        Url = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.IdFuncionalidade);
            
            CreateTable(
                "dbo.Notificacao",
                c => new
                    {
                        IdNotificacao = c.Int(nullable: false, identity: true),
                        IdUsuario = c.Int(nullable: false),
                        IdProcesso = c.Int(),
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
                "dbo.Processo",
                c => new
                    {
                        IdProcesso = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        IdUsuarioIncluiu = c.Int(),
                        Atividade = c.String(maxLength: 8000, unicode: false),
                        DocumentosAplicaveis = c.String(maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(),
                        DataAlteracao = c.DateTime(),
                        NmProcesso = c.String(nullable: false, maxLength: 8000, unicode: false),
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
                        Requisitos = c.String(maxLength: 8000, unicode: false),
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
                        Codigo = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Titulo = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Ativo = c.Boolean(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdNorma);
            
            CreateTable(
                "dbo.SiteModulo",
                c => new
                    {
                        IdSiteModulo = c.Int(nullable: false, identity: true),
                        IdModulo = c.Int(nullable: false),
                        IdSite = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSiteModulo)
                .ForeignKey("dbo.Funcionalidade", t => t.IdModulo)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .Index(t => t.IdModulo)
                .Index(t => t.IdSite);
            
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
                        NmFantasia = c.String(nullable: false, maxLength: 60, unicode: false),
                        NmUrlAcesso = c.String(nullable: false, maxLength: 60, unicode: false),
                        DtValidadeContrato = c.DateTime(nullable: false),
                        NuDiasTrocaSenha = c.Int(nullable: false),
                        NuTentativaBloqueioLogin = c.Int(),
                        NuArmazenaSenha = c.Int(),
                        FlExigeSenhaForte = c.Boolean(nullable: false),
                        FlTemCaptcha = c.Boolean(nullable: false),
                        FlEstruturaAprovador = c.Boolean(),
                        FlAtivo = c.Boolean(nullable: false),
                        IdUsuarioIncluiu = c.Int(),
                        DtInclusao = c.DateTime(),
                        ClienteLogoAux_IdAnexo = c.Int(),
                    })
                .PrimaryKey(t => t.IdCliente)
                .ForeignKey("dbo.Anexo", t => t.ClienteLogoAux_IdAnexo)
                .Index(t => t.ClienteLogoAux_IdAnexo);
            
            CreateTable(
                "dbo.ClienteLogo",
                c => new
                    {
                        IdClienteLogo = c.Int(nullable: false, identity: true),
                        IdCliente = c.Int(nullable: false),
                        IdAnexo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdClienteLogo)
                .ForeignKey("dbo.Anexo", t => t.IdAnexo, cascadeDelete: true)
                .ForeignKey("dbo.Cliente", t => t.IdCliente, cascadeDelete: true)
                .Index(t => t.IdCliente)
                .Index(t => t.IdAnexo);
            
            CreateTable(
                "dbo.Anexo",
                c => new
                    {
                        IdAnexo = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Extensao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Arquivo = c.Binary(nullable: false),
                        DtCriacao = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdAnexo);
            
            CreateTable(
                "dbo.ClienteContrato",
                c => new
                    {
                        IdClienteContrato = c.Int(nullable: false, identity: true),
                        IdCliente = c.Int(nullable: false),
                        IdAnexo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdClienteContrato)
                .ForeignKey("dbo.Cliente", t => t.IdCliente, cascadeDelete: true)
                .ForeignKey("dbo.Anexo", t => t.IdAnexo, cascadeDelete: true)
                .Index(t => t.IdCliente)
                .Index(t => t.IdAnexo);
            
            CreateTable(
                "dbo.SiteAnexo",
                c => new
                    {
                        IdSiteAnexo = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        IdAnexo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSiteAnexo)
                .ForeignKey("dbo.Anexo", t => t.IdAnexo, cascadeDelete: true)
                .ForeignKey("dbo.Site", t => t.IdSite, cascadeDelete: true)
                .Index(t => t.IdSite)
                .Index(t => t.IdAnexo);
            
            CreateTable(
                "dbo.UsuarioAnexo",
                c => new
                    {
                        IdUsuarioAnexo = c.Int(nullable: false, identity: true),
                        IdUsuario = c.Int(nullable: false),
                        IdAnexo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdUsuarioAnexo)
                .ForeignKey("dbo.Anexo", t => t.IdAnexo, cascadeDelete: true)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdUsuario)
                .Index(t => t.IdAnexo);
            
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
                "dbo.Fornecedor",
                c => new
                    {
                        IdFornecedor = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Telefone = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Email = c.String(nullable: false, maxLength: 8000, unicode: false),
                        IdSite = c.Int(nullable: false),
                        IdProcesso = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdFornecedor)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .ForeignKey("dbo.Site", t => t.IdSite)
                .Index(t => t.IdSite)
                .Index(t => t.IdProcesso);
            
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
                "dbo.Produto",
                c => new
                    {
                        IdProduto = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        IdResponsavel = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Especificacao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Tags = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DtCriacao = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        StatusCriterioAvaliacao = c.Int(nullable: false),
                        StatusCriterioQualificacao = c.Int(nullable: false),
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
                        Titulo = c.String(nullable: false, maxLength: 8000, unicode: false),
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
                        IdProduto = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DtCriacao = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdCriterioAvaliacao)
                .ForeignKey("dbo.Produto", t => t.IdProduto)
                .Index(t => t.IdProduto);
            
            CreateTable(
                "dbo.AvaliaCriterioAvaliacao",
                c => new
                    {
                        IdAvaliaCriterioAvaliacao = c.Int(nullable: false, identity: true),
                        IdCriterioAvaliacao = c.Int(nullable: false),
                        NotaAvaliacao = c.Int(nullable: false),
                        DtAvaliacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdAvaliaCriterioAvaliacao)
                .ForeignKey("dbo.CriterioAvaliacao", t => t.IdCriterioAvaliacao)
                .Index(t => t.IdCriterioAvaliacao);
            
            CreateTable(
                "dbo.CriterioQualificacao",
                c => new
                    {
                        IdCriterioQualificacao = c.Int(nullable: false, identity: true),
                        IdProduto = c.Int(nullable: false),
                        Titulo = c.String(nullable: false, maxLength: 8000, unicode: false),
                        TemControleVencimento = c.Boolean(nullable: false),
                        DtVencimento = c.DateTime(),
                        DtCriacao = c.DateTime(nullable: false),
                        DtAlteracao = c.DateTime(nullable: false),
                        Aprovado = c.Boolean(),
                        ArquivoEvidencia = c.String(maxLength: 8000, unicode: false),
                        DtEmissao = c.DateTime(),
                        DtQualificacaoVencimento = c.DateTime(),
                        IdResponsavelPorControlarVencimento = c.Int(),
                        IdResponsavelPorQualificar = c.Int(),
                        NumeroDocumento = c.String(maxLength: 8000, unicode: false),
                        Observacoes = c.String(maxLength: 8000, unicode: false),
                        ObservacoesDocumento = c.String(maxLength: 8000, unicode: false),
                        OrgaoExpedidor = c.String(maxLength: 8000, unicode: false),
                        DtAlteracaoEmissao = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdCriterioQualificacao)
                .ForeignKey("dbo.Produto", t => t.IdProduto)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelPorControlarVencimento)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavelPorQualificar)
                .Index(t => t.IdProduto)
                .Index(t => t.IdResponsavelPorControlarVencimento)
                .Index(t => t.IdResponsavelPorQualificar);
            
            CreateTable(
                "dbo.NotificacaoMensagem",
                c => new
                    {
                        IdNotificacaoMenssagem = c.Int(nullable: false, identity: true),
                        IdSite = c.Int(nullable: false),
                        NmEmailPara = c.String(nullable: false, maxLength: 150, unicode: false),
                        NmEmailNome = c.String(nullable: false, maxLength: 60, unicode: false),
                        DsMensagem = c.String(maxLength: 8000, unicode: false),
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
                        CdUsuario = c.String(maxLength: 8000, unicode: false),
                        CdSenha = c.String(maxLength: 8000, unicode: false),
                        NmNome = c.String(maxLength: 8000, unicode: false),
                        FlAtivo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdSmptNotificacao);
            
            CreateTable(
                "dbo.AnaliseCriticaTema",
                c => new
                    {
                        IdTema = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Risco = c.String(nullable: false, maxLength: 8000, unicode: false),
                        PossuiGestaoRisco = c.Boolean(nullable: false),
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
                .Index(t => t.IdControladorCategoria)
                .Index(t => t.IdRegistro)
                .Index(t => t.IdAnaliseCritica);
            
            CreateTable(
                "dbo.Advertencia",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CodigoTipoAdvertencia = c.Int(nullable: false),
                        CodigoFuncionario = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .ForeignKey("dbo.TipoAdvertencia", t => t.CodigoTipoAdvertencia)
                .Index(t => t.CodigoTipoAdvertencia)
                .Index(t => t.CodigoFuncionario);
            
            CreateTable(
                "dbo.Funcionario",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Sexo = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataNascimento = c.DateTime(nullable: false),
                        DataVencimentoFerias = c.DateTime(nullable: false),
                        DataUltimoPrazo = c.DateTime(nullable: false),
                        NumeroRegistro = c.Int(nullable: false),
                        EstadoCivil = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Endereco = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Bairro = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Cep = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Cidade = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Uf = c.String(nullable: false, maxLength: 8000, unicode: false),
                        TelefoneResidencial = c.String(maxLength: 8000, unicode: false),
                        TelefoneCelular = c.String(maxLength: 8000, unicode: false),
                        TelefoneRecado = c.String(maxLength: 8000, unicode: false),
                        CNH = c.String(maxLength: 8000, unicode: false),
                        VencimentoCNH = c.DateTime(nullable: false),
                        TituloEleitoral = c.String(maxLength: 8000, unicode: false),
                        Outro = c.String(maxLength: 8000, unicode: false),
                        Observacao = c.String(maxLength: 8000, unicode: false),
                        CodigoCargo = c.Int(),
                        CodigoCompetencia = c.Int(),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.CargoRH", t => t.CodigoCargo)
                .ForeignKey("dbo.Competencia", t => t.CodigoCompetencia)
                .Index(t => t.CodigoCargo)
                .Index(t => t.CodigoCompetencia);
            
            CreateTable(
                "dbo.CargoRH",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CodigoCompetencia = c.Int(nullable: false),
                        CodigoSindicato = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Competencia", t => t.CodigoCompetencia)
                .ForeignKey("dbo.Sindicato", t => t.CodigoSindicato)
                .Index(t => t.CodigoCompetencia)
                .Index(t => t.CodigoSindicato);
            
            CreateTable(
                "dbo.Competencia",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        NivelEscolaridade = c.Int(nullable: false),
                        NivelFormacaoAcademica = c.Int(nullable: false),
                        Tipo = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo);
            
            CreateTable(
                "dbo.Atribuicao",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CodigoCompetencia = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Competencia", t => t.CodigoCompetencia)
                .Index(t => t.CodigoCompetencia);
            
            CreateTable(
                "dbo.Conhecimento",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CodigoCompetencia = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Competencia", t => t.CodigoCompetencia)
                .Index(t => t.CodigoCompetencia);
            
            CreateTable(
                "dbo.Curso",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Entidade = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataValidade = c.DateTime(nullable: false),
                        CodigoCompetencia = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Competencia", t => t.CodigoCompetencia)
                .Index(t => t.CodigoCompetencia);
            
            CreateTable(
                "dbo.FormacaoAcademica",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CodigoCompetencia = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Competencia", t => t.CodigoCompetencia)
                .Index(t => t.CodigoCompetencia);
            
            CreateTable(
                "dbo.Habilidade",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CodigoCompetencia = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Competencia", t => t.CodigoCompetencia)
                .Index(t => t.CodigoCompetencia);
            
            CreateTable(
                "dbo.Treinamento",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CodigoCompetencia = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Competencia", t => t.CodigoCompetencia)
                .Index(t => t.CodigoCompetencia);
            
            CreateTable(
                "dbo.Sindicato",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        ValorAnual = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PercentualAnual = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo);
            
            CreateTable(
                "dbo.Dependente",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataNascimento = c.DateTime(nullable: false),
                        Sexo = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Documentos = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CodigoParentesco = c.Int(nullable: false),
                        CodigoFuncionario = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                        Planos_Codigo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .ForeignKey("dbo.Parentesco", t => t.CodigoParentesco)
                .ForeignKey("dbo.Plano", t => t.Planos_Codigo)
                .Index(t => t.CodigoParentesco)
                .Index(t => t.CodigoFuncionario)
                .Index(t => t.Planos_Codigo);
            
            CreateTable(
                "dbo.Parentesco",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo);
            
            CreateTable(
                "dbo.Plano",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CodigoTipoPlano = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.TipoPlano", t => t.CodigoTipoPlano)
                .Index(t => t.CodigoTipoPlano);
            
            CreateTable(
                "dbo.TipoPlano",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo);
            
            CreateTable(
                "dbo.Emprestimo",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Parcelas = c.Int(nullable: false),
                        DescontoFerias = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CodigoTipoEmprestimo = c.Int(nullable: false),
                        CodigoFuncionario = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .ForeignKey("dbo.TipoEmprestimo", t => t.CodigoTipoEmprestimo)
                .Index(t => t.CodigoTipoEmprestimo)
                .Index(t => t.CodigoFuncionario);
            
            CreateTable(
                "dbo.TipoEmprestimo",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo);
            
            CreateTable(
                "dbo.EPI",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CodigoFuncionario = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .Index(t => t.CodigoFuncionario);
            
            CreateTable(
                "dbo.Exame",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CodigoFuncionario = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .Index(t => t.CodigoFuncionario);
            
            CreateTable(
                "dbo.Ferias",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        DataInicial = c.DateTime(nullable: false),
                        DataFinal = c.DateTime(nullable: false),
                        Remuneracao = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TeveAdiantamentoDecimoTerceiro = c.Boolean(nullable: false),
                        Obervacao = c.String(maxLength: 8000, unicode: false),
                        CodigoFuncionario = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .Index(t => t.CodigoFuncionario);
            
            CreateTable(
                "dbo.FolhaDePagamento",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Desconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CodigoFuncionario = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .Index(t => t.CodigoFuncionario);
            
            CreateTable(
                "dbo.ValeTransporte",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        ValorCredito = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValorDescontoMensal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataVigencia = c.DateTime(nullable: false),
                        Possui = c.Boolean(nullable: false),
                        CodigoFuncionario = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .Index(t => t.CodigoFuncionario);
            
            CreateTable(
                "dbo.TipoAdvertencia",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo);
            
            CreateTable(
                "dbo.BreadCrumb",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Lingua = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Ativo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo);
            
            CreateTable(
                "dbo.Calibracao",
                c => new
                    {
                        IdCalibracao = c.Int(nullable: false, identity: true),
                        IdInstrumento = c.Int(nullable: false),
                        Certificado = c.String(maxLength: 8000, unicode: false),
                        OrgaoCalibrador = c.String(maxLength: 8000, unicode: false),
                        Aprovado = c.Byte(nullable: false),
                        Aprovador = c.Int(nullable: false),
                        Observacoes = c.String(maxLength: 8000, unicode: false),
                        ArquivoCertificado = c.String(maxLength: 8000, unicode: false),
                        DataCalibracao = c.DateTime(nullable: false),
                        DataProximaCalibracao = c.DateTime(nullable: false),
                        DataCriacao = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(nullable: false),
                        IdUsuarioIncluiu = c.Int(nullable: false),
                        DataRegistro = c.DateTime(nullable: false),
                        DataNotificacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdCalibracao)
                .ForeignKey("dbo.Instrumento", t => t.IdInstrumento)
                .ForeignKey("dbo.Usuario", t => t.Aprovador)
                .Index(t => t.IdInstrumento)
                .Index(t => t.Aprovador);
            
            CreateTable(
                "dbo.CriterioAceitacao",
                c => new
                    {
                        IdCriterioAceitacao = c.Int(nullable: false, identity: true),
                        Periodicidade = c.String(maxLength: 8000, unicode: false),
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
                        Equipamento = c.String(maxLength: 8000, unicode: false),
                        Numero = c.String(maxLength: 8000, unicode: false),
                        LocalDeUso = c.String(maxLength: 8000, unicode: false),
                        Marca = c.String(maxLength: 8000, unicode: false),
                        Modelo = c.String(maxLength: 8000, unicode: false),
                        Periodicidade = c.Byte(nullable: false),
                        Escala = c.String(maxLength: 8000, unicode: false),
                        CriterioAceitacao = c.String(maxLength: 8000, unicode: false),
                        MenorDivisao = c.String(maxLength: 8000, unicode: false),
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
                "dbo.ControleImpressao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdFuncionalidade = c.Int(nullable: false),
                        CodigoReferencia = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CopiaControlada = c.Boolean(nullable: false),
                        IdUsuarioDestino = c.Int(),
                        DataImpressao = c.DateTime(nullable: false),
                        IdUsuarioIncluiu = c.Int(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Funcionalidade", t => t.IdFuncionalidade)
                .ForeignKey("dbo.Usuario", t => t.IdUsuarioIncluiu)
                .ForeignKey("dbo.Usuario", t => t.IdUsuarioDestino)
                .Index(t => t.IdFuncionalidade)
                .Index(t => t.IdUsuarioDestino)
                .Index(t => t.IdUsuarioIncluiu);
            
            CreateTable(
                "dbo.Perfil",
                c => new
                    {
                        IdPerfil = c.Int(nullable: false, identity: true),
                        NmNome = c.String(nullable: false, maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.IdPerfil);
            
            CreateTable(
                "dbo.DocDocumento",
                c => new
                    {
                        IdDocumento = c.Int(nullable: false, identity: true),
                        IdDocumentoPai = c.Int(),
                        IdSite = c.Int(nullable: false),
                        IdDocIdentificador = c.Int(nullable: false),
                        NumeroDocumento = c.String(maxLength: 15, unicode: false),
                        NuRevisao = c.Byte(),
                        Titulo = c.String(maxLength: 1000, unicode: false),
                        DtPedidoVerificacao = c.DateTime(),
                        DtVerificacao = c.DateTime(),
                        DtVencimento = c.DateTime(),
                        DtEmissao = c.DateTime(),
                        DtPedidoAprovacao = c.DateTime(),
                        DtAprovacao = c.DateTime(),
                        DtNotificacao = c.DateTime(),
                        XmlMetadata = c.String(maxLength: 8000, unicode: false),
                        FlWorkFlow = c.Boolean(nullable: false),
                        FlRevisaoPeriodica = c.Boolean(nullable: false),
                        FlStatus = c.Byte(nullable: false),
                        IdUsuarioIncluiu = c.Int(),
                        DtInclusao = c.DateTime(),
                        DtAlteracao = c.DateTime(nullable: false),
                        Ativo = c.Boolean(),
                        Risco = c.String(maxLength: 8000, unicode: false),
                        PossuiGestaoRisco = c.Boolean(),
                        IdProcesso = c.Int(),
                        IdCategoria = c.Int(nullable: false),
                        IdSigla = c.Int(nullable: false),
                        IdElaborador = c.Int(nullable: false),
                        IdRegistro = c.Int(),
                    })
                .PrimaryKey(t => t.IdDocumento)
                .ForeignKey("dbo.Cadastro", t => t.IdCategoria)
                .ForeignKey("dbo.Usuario", t => t.IdElaborador)
                .ForeignKey("dbo.Registros", t => t.IdRegistro)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .ForeignKey("dbo.Cadastro", t => t.IdSigla)
                .Index(t => t.IdProcesso)
                .Index(t => t.IdCategoria)
                .Index(t => t.IdSigla)
                .Index(t => t.IdElaborador)
                .Index(t => t.IdRegistro);
            
            CreateTable(
                "dbo.DocumentoAssunto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdDocumento = c.Int(nullable: false),
                        Revisao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataAssunto = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocDocumento", t => t.IdDocumento)
                .Index(t => t.IdDocumento);
            
            CreateTable(
                "dbo.DocumentoComentario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdDocumento = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataComentario = c.DateTime(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocDocumento", t => t.IdDocumento)
                .ForeignKey("dbo.Usuario", t => t.IdUsuario)
                .Index(t => t.IdDocumento)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.DocumentoCargo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdCargo = c.Int(nullable: false),
                        IdDocumento = c.Int(nullable: false),
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
                "dbo.Estoque",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Quantidade = c.Int(nullable: false),
                        CodigoEPI = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.EPI", t => t.CodigoEPI)
                .Index(t => t.CodigoEPI);
            
            CreateTable(
                "dbo.HistoricoCriterioAvaliacao",
                c => new
                    {
                        IdHistoricoCriterioAvaliacao = c.Int(nullable: false, identity: true),
                        IdCriterioAvaliacao = c.Int(nullable: false),
                        Nota = c.Int(nullable: false),
                        DtCriacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdHistoricoCriterioAvaliacao)
                .ForeignKey("dbo.CriterioAvaliacao", t => t.IdCriterioAvaliacao)
                .Index(t => t.IdCriterioAvaliacao);
            
            CreateTable(
                "dbo.Indicador",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Objetivo = c.String(maxLength: 8000, unicode: false),
                        Descricao = c.String(maxLength: 8000, unicode: false),
                        Unidade = c.String(maxLength: 8000, unicode: false),
                        Direcao = c.String(maxLength: 8000, unicode: false),
                        Peso = c.Int(nullable: false),
                        Maximo = c.Byte(nullable: false),
                        Minimo = c.Byte(nullable: false),
                        IdSite = c.Int(nullable: false),
                        IdUsuarioIncluiu = c.Int(nullable: false),
                        DataAlteracao = c.DateTime(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        IdProcesso = c.Int(nullable: false),
                        IdResponsavel = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavel)
                .Index(t => t.IdProcesso)
                .Index(t => t.IdResponsavel);
            
            CreateTable(
                "dbo.PeriodicidaDeAnalise",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PeriodoAnalise = c.Byte(nullable: false),
                        Justificativa = c.String(maxLength: 8000, unicode: false),
                        CorRisco = c.String(maxLength: 8000, unicode: false),
                        Inicio = c.DateTime(nullable: false),
                        Fim = c.DateTime(nullable: false),
                        RealAcumulado = c.Double(nullable: false),
                        MetaEstimulada = c.Double(nullable: false),
                        IdIndicador = c.Int(nullable: false),
                        IdPlanDeAcao = c.Int(),
                        Indicador_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Indicador", t => t.IdIndicador)
                .ForeignKey("dbo.Registros", t => t.IdPlanDeAcao)
                .ForeignKey("dbo.Indicador", t => t.Indicador_Id)
                .Index(t => t.IdIndicador)
                .Index(t => t.IdPlanDeAcao)
                .Index(t => t.Indicador_Id);
            
            CreateTable(
                "dbo.Meta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Valor = c.Double(nullable: false),
                        UnidadeMedida = c.Byte(nullable: false),
                        DataReferencia = c.DateTime(nullable: false),
                        IdPeriodicidadeAnalise = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PeriodicidaDeAnalise", t => t.IdPeriodicidadeAnalise)
                .Index(t => t.IdPeriodicidadeAnalise);
            
            CreateTable(
                "dbo.PlanoVoo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Realizado = c.Double(nullable: false),
                        DataReferencia = c.DateTime(nullable: false),
                        DataAlteracao = c.DateTime(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        IdPeriodicidadeAnalise = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PeriodicidaDeAnalise", t => t.IdPeriodicidadeAnalise)
                .Index(t => t.IdPeriodicidadeAnalise);
            
            CreateTable(
                "dbo.ListaValor",
                c => new
                    {
                        IdListaValor = c.Int(nullable: false, identity: true),
                        CdTabela = c.String(nullable: false, maxLength: 10, unicode: false),
                        CdCodigo = c.String(nullable: false, maxLength: 40, unicode: false),
                        DsDescricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CdCulture = c.String(maxLength: 5, unicode: false),
                    })
                .PrimaryKey(t => t.IdListaValor);
            
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        IdLog = c.Int(nullable: false, identity: true),
                        IdAcao = c.Int(nullable: false),
                        IP = c.String(maxLength: 8000, unicode: false),
                        Browser = c.String(maxLength: 8000, unicode: false),
                        Mensagem = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        IdUsuario = c.Int(),
                    })
                .PrimaryKey(t => t.IdLog);
            
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
                        Endereco = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Escopo = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Gestores = c.String(nullable: false, maxLength: 8000, unicode: false),
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
                "dbo.SubModulo",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoSite = c.Int(nullable: false),
                        CodigoFuncionalidade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Funcionalidade", t => t.CodigoFuncionalidade)
                .ForeignKey("dbo.Site", t => t.CodigoSite)
                .Index(t => t.CodigoSite)
                .Index(t => t.CodigoFuncionalidade);
            
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
            DropForeignKey("dbo.SubModulo", "CodigoSite", "dbo.Site");
            DropForeignKey("dbo.SubModulo", "CodigoFuncionalidade", "dbo.Funcionalidade");
            DropForeignKey("dbo.PlaiProcessoNorma", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.PlaiProcessoNorma", "IdPlai", "dbo.Plai");
            DropForeignKey("dbo.PlaiProcessoNorma", "IdNorma", "dbo.Norma");
            DropForeignKey("dbo.Plai", "IdPai", "dbo.Pai");
            DropForeignKey("dbo.Pai", "IdGestor", "dbo.Usuario");
            DropForeignKey("dbo.Pai", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Indicador", "IdResponsavel", "dbo.Usuario");
            DropForeignKey("dbo.Indicador", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.PeriodicidaDeAnalise", "Indicador_Id", "dbo.Indicador");
            DropForeignKey("dbo.PlanoVoo", "IdPeriodicidadeAnalise", "dbo.PeriodicidaDeAnalise");
            DropForeignKey("dbo.PeriodicidaDeAnalise", "IdPlanDeAcao", "dbo.Registros");
            DropForeignKey("dbo.Meta", "IdPeriodicidadeAnalise", "dbo.PeriodicidaDeAnalise");
            DropForeignKey("dbo.PeriodicidaDeAnalise", "IdIndicador", "dbo.Indicador");
            DropForeignKey("dbo.HistoricoCriterioAvaliacao", "IdCriterioAvaliacao", "dbo.CriterioAvaliacao");
            DropForeignKey("dbo.Estoque", "CodigoEPI", "dbo.EPI");
            DropForeignKey("dbo.DocDocumento", "IdSigla", "dbo.Cadastro");
            DropForeignKey("dbo.DocDocumento", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.DocDocumento", "IdRegistro", "dbo.Registros");
            DropForeignKey("dbo.DocDocumento", "IdElaborador", "dbo.Usuario");
            DropForeignKey("dbo.DocUsuarioVerificaAprova", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.DocUsuarioVerificaAprova", "IdDocumento", "dbo.DocDocumento");
            DropForeignKey("dbo.DocTemplate", "IdDocumento", "dbo.DocDocumento");
            DropForeignKey("dbo.DocumentoCargo", "IdDocumento", "dbo.DocDocumento");
            DropForeignKey("dbo.DocumentoComentario", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.DocumentoComentario", "IdDocumento", "dbo.DocDocumento");
            DropForeignKey("dbo.DocDocumento", "IdCategoria", "dbo.Cadastro");
            DropForeignKey("dbo.DocumentoAssunto", "IdDocumento", "dbo.DocDocumento");
            DropForeignKey("dbo.ControleImpressao", "IdUsuarioDestino", "dbo.Usuario");
            DropForeignKey("dbo.ControleImpressao", "IdUsuarioIncluiu", "dbo.Usuario");
            DropForeignKey("dbo.ControleImpressao", "IdFuncionalidade", "dbo.Funcionalidade");
            DropForeignKey("dbo.Calibracao", "Aprovador", "dbo.Usuario");
            DropForeignKey("dbo.Calibracao", "IdInstrumento", "dbo.Instrumento");
            DropForeignKey("dbo.Instrumento", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Instrumento", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.CriterioAceitacao", "IdCalibracao", "dbo.Calibracao");
            DropForeignKey("dbo.Advertencia", "CodigoTipoAdvertencia", "dbo.TipoAdvertencia");
            DropForeignKey("dbo.Advertencia", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.ValeTransporte", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.FolhaDePagamento", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Ferias", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Exame", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.EPI", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Emprestimo", "CodigoTipoEmprestimo", "dbo.TipoEmprestimo");
            DropForeignKey("dbo.Emprestimo", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Dependente", "Planos_Codigo", "dbo.Plano");
            DropForeignKey("dbo.Plano", "CodigoTipoPlano", "dbo.TipoPlano");
            DropForeignKey("dbo.Dependente", "CodigoParentesco", "dbo.Parentesco");
            DropForeignKey("dbo.Dependente", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Funcionario", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.Funcionario", "CodigoCargo", "dbo.CargoRH");
            DropForeignKey("dbo.CargoRH", "CodigoSindicato", "dbo.Sindicato");
            DropForeignKey("dbo.CargoRH", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.Treinamento", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.Habilidade", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.FormacaoAcademica", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.Curso", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.Conhecimento", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.Atribuicao", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.RegistroAcaoImediata", "IdReponsavelImplementar", "dbo.Usuario");
            DropForeignKey("dbo.RegistroAcaoImediata", "IdRegistro", "dbo.Registros");
            DropForeignKey("dbo.Registros", "IdTipoNaoConformidade", "dbo.Cadastro");
            DropForeignKey("dbo.Registros", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Registros", "IdResponsavelDefinir", "dbo.Usuario");
            DropForeignKey("dbo.Registros", "IdResponsavelInicarAcaoImediata", "dbo.Usuario");
            DropForeignKey("dbo.Registros", "IdResponsavelImplementar", "dbo.Usuario");
            DropForeignKey("dbo.Registros", "IdResponsavelEtapa", "dbo.Usuario");
            DropForeignKey("dbo.Registros", "IdResponsavelAnalisar", "dbo.Usuario");
            DropForeignKey("dbo.Registros", "IdResponsavelAcaoCorretiva", "dbo.Usuario");
            DropForeignKey("dbo.Registros", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.Registros", "IdEmissor", "dbo.Usuario");
            DropForeignKey("dbo.AnaliseCriticaTema", "IdRegistro", "dbo.Registros");
            DropForeignKey("dbo.AnaliseCriticaTema", "IdControladorCategoria", "dbo.Cadastro");
            DropForeignKey("dbo.AnaliseCriticaTema", "IdAnaliseCritica", "dbo.AnaliseCritica");
            DropForeignKey("dbo.AnaliseCritica", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Site", "SiteLogoAux_IdAnexo", "dbo.Anexo");
            DropForeignKey("dbo.NotificacaoMensagem", "IdSite", "dbo.Site");
            DropForeignKey("dbo.NotificacaoMensagem", "IdSmtpNotificacao", "dbo.NotificacaoSmtp");
            DropForeignKey("dbo.Fornecedor", "IdSite", "dbo.Site");
            DropForeignKey("dbo.ProdutoFornecedor", "IdProduto", "dbo.Produto");
            DropForeignKey("dbo.Produto", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Produto", "IdResponsavel", "dbo.Usuario");
            DropForeignKey("dbo.CriterioQualificacao", "IdResponsavelPorQualificar", "dbo.Usuario");
            DropForeignKey("dbo.CriterioQualificacao", "IdResponsavelPorControlarVencimento", "dbo.Usuario");
            DropForeignKey("dbo.CriterioQualificacao", "IdProduto", "dbo.Produto");
            DropForeignKey("dbo.CriterioAvaliacao", "IdProduto", "dbo.Produto");
            DropForeignKey("dbo.AvaliaCriterioAvaliacao", "IdCriterioAvaliacao", "dbo.CriterioAvaliacao");
            DropForeignKey("dbo.AvaliacaoCriticidade", "IdProduto", "dbo.Produto");
            DropForeignKey("dbo.ProdutoFornecedor", "IdFornecedor", "dbo.Fornecedor");
            DropForeignKey("dbo.Fornecedor", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.Cadastro", "IdSite", "dbo.Site");
            DropForeignKey("dbo.Site", "IdCliente", "dbo.Cliente");
            DropForeignKey("dbo.UsuarioClienteSite", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioClienteSite", "IdSite", "dbo.Site");
            DropForeignKey("dbo.UsuarioClienteSite", "IdCliente", "dbo.Cliente");
            DropForeignKey("dbo.Cliente", "ClienteLogoAux_IdAnexo", "dbo.Anexo");
            DropForeignKey("dbo.ClienteLogo", "IdCliente", "dbo.Cliente");
            DropForeignKey("dbo.ClienteLogo", "IdAnexo", "dbo.Anexo");
            DropForeignKey("dbo.Usuario", "Anexo_IdAnexo", "dbo.Anexo");
            DropForeignKey("dbo.UsuarioAnexo", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioAnexo", "IdAnexo", "dbo.Anexo");
            DropForeignKey("dbo.SiteAnexo", "IdSite", "dbo.Site");
            DropForeignKey("dbo.SiteAnexo", "IdAnexo", "dbo.Anexo");
            DropForeignKey("dbo.ClienteContrato", "IdAnexo", "dbo.Anexo");
            DropForeignKey("dbo.ClienteContrato", "IdCliente", "dbo.Cliente");
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
            DropForeignKey("dbo.Processo", "IdSite", "dbo.Site");
            DropForeignKey("dbo.NormaProcesso", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.NormaProcesso", "IdNorma", "dbo.Norma");
            DropForeignKey("dbo.Notificacao", "IdFuncionalidade", "dbo.Funcionalidade");
            DropForeignKey("dbo.CargoProcesso", "IdCargo", "dbo.Cargo");
            DropForeignKey("dbo.AnaliseCritica", "IdResponsavel", "dbo.Usuario");
            DropForeignKey("dbo.AnaliseCriticaFuncionario", "IdUsuario", "dbo.Usuario");
            DropForeignKey("dbo.AnaliseCriticaFuncionario", "IdAnaliseCritica", "dbo.AnaliseCritica");
            DropIndex("dbo.SubModulo", new[] { "CodigoFuncionalidade" });
            DropIndex("dbo.SubModulo", new[] { "CodigoSite" });
            DropIndex("dbo.PlaiProcessoNorma", new[] { "IdNorma" });
            DropIndex("dbo.PlaiProcessoNorma", new[] { "IdProcesso" });
            DropIndex("dbo.PlaiProcessoNorma", new[] { "IdPlai" });
            DropIndex("dbo.Pai", new[] { "IdSite" });
            DropIndex("dbo.Pai", new[] { "IdGestor" });
            DropIndex("dbo.Plai", new[] { "IdPai" });
            DropIndex("dbo.PlanoVoo", new[] { "IdPeriodicidadeAnalise" });
            DropIndex("dbo.Meta", new[] { "IdPeriodicidadeAnalise" });
            DropIndex("dbo.PeriodicidaDeAnalise", new[] { "Indicador_Id" });
            DropIndex("dbo.PeriodicidaDeAnalise", new[] { "IdPlanDeAcao" });
            DropIndex("dbo.PeriodicidaDeAnalise", new[] { "IdIndicador" });
            DropIndex("dbo.Indicador", new[] { "IdResponsavel" });
            DropIndex("dbo.Indicador", new[] { "IdProcesso" });
            DropIndex("dbo.HistoricoCriterioAvaliacao", new[] { "IdCriterioAvaliacao" });
            DropIndex("dbo.Estoque", new[] { "CodigoEPI" });
            DropIndex("dbo.DocUsuarioVerificaAprova", new[] { "IdUsuario" });
            DropIndex("dbo.DocUsuarioVerificaAprova", new[] { "IdDocumento" });
            DropIndex("dbo.DocTemplate", new[] { "IdDocumento" });
            DropIndex("dbo.DocumentoCargo", new[] { "IdDocumento" });
            DropIndex("dbo.DocumentoComentario", new[] { "IdUsuario" });
            DropIndex("dbo.DocumentoComentario", new[] { "IdDocumento" });
            DropIndex("dbo.DocumentoAssunto", new[] { "IdDocumento" });
            DropIndex("dbo.DocDocumento", new[] { "IdRegistro" });
            DropIndex("dbo.DocDocumento", new[] { "IdElaborador" });
            DropIndex("dbo.DocDocumento", new[] { "IdSigla" });
            DropIndex("dbo.DocDocumento", new[] { "IdCategoria" });
            DropIndex("dbo.DocDocumento", new[] { "IdProcesso" });
            DropIndex("dbo.ControleImpressao", new[] { "IdUsuarioIncluiu" });
            DropIndex("dbo.ControleImpressao", new[] { "IdUsuarioDestino" });
            DropIndex("dbo.ControleImpressao", new[] { "IdFuncionalidade" });
            DropIndex("dbo.Instrumento", new[] { "IdProcesso" });
            DropIndex("dbo.Instrumento", new[] { "IdSite" });
            DropIndex("dbo.CriterioAceitacao", new[] { "IdCalibracao" });
            DropIndex("dbo.Calibracao", new[] { "Aprovador" });
            DropIndex("dbo.Calibracao", new[] { "IdInstrumento" });
            DropIndex("dbo.ValeTransporte", new[] { "CodigoFuncionario" });
            DropIndex("dbo.FolhaDePagamento", new[] { "CodigoFuncionario" });
            DropIndex("dbo.Ferias", new[] { "CodigoFuncionario" });
            DropIndex("dbo.Exame", new[] { "CodigoFuncionario" });
            DropIndex("dbo.EPI", new[] { "CodigoFuncionario" });
            DropIndex("dbo.Emprestimo", new[] { "CodigoFuncionario" });
            DropIndex("dbo.Emprestimo", new[] { "CodigoTipoEmprestimo" });
            DropIndex("dbo.Plano", new[] { "CodigoTipoPlano" });
            DropIndex("dbo.Dependente", new[] { "Planos_Codigo" });
            DropIndex("dbo.Dependente", new[] { "CodigoFuncionario" });
            DropIndex("dbo.Dependente", new[] { "CodigoParentesco" });
            DropIndex("dbo.Treinamento", new[] { "CodigoCompetencia" });
            DropIndex("dbo.Habilidade", new[] { "CodigoCompetencia" });
            DropIndex("dbo.FormacaoAcademica", new[] { "CodigoCompetencia" });
            DropIndex("dbo.Curso", new[] { "CodigoCompetencia" });
            DropIndex("dbo.Conhecimento", new[] { "CodigoCompetencia" });
            DropIndex("dbo.Atribuicao", new[] { "CodigoCompetencia" });
            DropIndex("dbo.CargoRH", new[] { "CodigoSindicato" });
            DropIndex("dbo.CargoRH", new[] { "CodigoCompetencia" });
            DropIndex("dbo.Funcionario", new[] { "CodigoCompetencia" });
            DropIndex("dbo.Funcionario", new[] { "CodigoCargo" });
            DropIndex("dbo.Advertencia", new[] { "CodigoFuncionario" });
            DropIndex("dbo.Advertencia", new[] { "CodigoTipoAdvertencia" });
            DropIndex("dbo.AnaliseCriticaTema", new[] { "IdAnaliseCritica" });
            DropIndex("dbo.AnaliseCriticaTema", new[] { "IdRegistro" });
            DropIndex("dbo.AnaliseCriticaTema", new[] { "IdControladorCategoria" });
            DropIndex("dbo.NotificacaoMensagem", new[] { "IdSmtpNotificacao" });
            DropIndex("dbo.NotificacaoMensagem", new[] { "IdSite" });
            DropIndex("dbo.CriterioQualificacao", new[] { "IdResponsavelPorQualificar" });
            DropIndex("dbo.CriterioQualificacao", new[] { "IdResponsavelPorControlarVencimento" });
            DropIndex("dbo.CriterioQualificacao", new[] { "IdProduto" });
            DropIndex("dbo.AvaliaCriterioAvaliacao", new[] { "IdCriterioAvaliacao" });
            DropIndex("dbo.CriterioAvaliacao", new[] { "IdProduto" });
            DropIndex("dbo.AvaliacaoCriticidade", new[] { "IdProduto" });
            DropIndex("dbo.Produto", new[] { "IdResponsavel" });
            DropIndex("dbo.Produto", new[] { "IdSite" });
            DropIndex("dbo.ProdutoFornecedor", new[] { "IdFornecedor" });
            DropIndex("dbo.ProdutoFornecedor", new[] { "IdProduto" });
            DropIndex("dbo.Fornecedor", new[] { "IdProcesso" });
            DropIndex("dbo.Fornecedor", new[] { "IdSite" });
            DropIndex("dbo.Cadastro", new[] { "IdSite" });
            DropIndex("dbo.UsuarioClienteSite", new[] { "IdSite" });
            DropIndex("dbo.UsuarioClienteSite", new[] { "IdUsuario" });
            DropIndex("dbo.UsuarioClienteSite", new[] { "IdCliente" });
            DropIndex("dbo.UsuarioAnexo", new[] { "IdAnexo" });
            DropIndex("dbo.UsuarioAnexo", new[] { "IdUsuario" });
            DropIndex("dbo.SiteAnexo", new[] { "IdAnexo" });
            DropIndex("dbo.SiteAnexo", new[] { "IdSite" });
            DropIndex("dbo.ClienteContrato", new[] { "IdAnexo" });
            DropIndex("dbo.ClienteContrato", new[] { "IdCliente" });
            DropIndex("dbo.ClienteLogo", new[] { "IdAnexo" });
            DropIndex("dbo.ClienteLogo", new[] { "IdCliente" });
            DropIndex("dbo.Cliente", new[] { "ClienteLogoAux_IdAnexo" });
            DropIndex("dbo.UsuarioCargo", new[] { "IdCargo" });
            DropIndex("dbo.UsuarioCargo", new[] { "IdUsuario" });
            DropIndex("dbo.SiteModulo", new[] { "IdSite" });
            DropIndex("dbo.SiteModulo", new[] { "IdModulo" });
            DropIndex("dbo.NormaProcesso", new[] { "IdNorma" });
            DropIndex("dbo.NormaProcesso", new[] { "IdProcesso" });
            DropIndex("dbo.Processo", new[] { "IdSite" });
            DropIndex("dbo.Notificacao", new[] { "IdFuncionalidade" });
            DropIndex("dbo.Notificacao", new[] { "IdSite" });
            DropIndex("dbo.Notificacao", new[] { "IdProcesso" });
            DropIndex("dbo.Notificacao", new[] { "IdUsuario" });
            DropIndex("dbo.Funcao", new[] { "IdFuncionalidade" });
            DropIndex("dbo.CargoProcesso", new[] { "IdFuncao" });
            DropIndex("dbo.CargoProcesso", new[] { "IdCargo" });
            DropIndex("dbo.CargoProcesso", new[] { "IdProcesso" });
            DropIndex("dbo.Cargo", new[] { "IdSite" });
            DropIndex("dbo.Site", new[] { "SiteLogoAux_IdAnexo" });
            DropIndex("dbo.Site", new[] { "IdCliente" });
            DropIndex("dbo.AnaliseCriticaFuncionario", new[] { "IdAnaliseCritica" });
            DropIndex("dbo.AnaliseCriticaFuncionario", new[] { "IdUsuario" });
            DropIndex("dbo.AnaliseCritica", new[] { "IdResponsavel" });
            DropIndex("dbo.AnaliseCritica", new[] { "IdSite" });
            DropIndex("dbo.Usuario", new[] { "Anexo_IdAnexo" });
            DropIndex("dbo.Registros", new[] { "IdResponsavelEtapa" });
            DropIndex("dbo.Registros", new[] { "IdTipoNaoConformidade" });
            DropIndex("dbo.Registros", new[] { "IdResponsavelImplementar" });
            DropIndex("dbo.Registros", new[] { "IdResponsavelAnalisar" });
            DropIndex("dbo.Registros", new[] { "IdResponsavelDefinir" });
            DropIndex("dbo.Registros", new[] { "IdResponsavelAcaoCorretiva" });
            DropIndex("dbo.Registros", new[] { "IdResponsavelInicarAcaoImediata" });
            DropIndex("dbo.Registros", new[] { "IdEmissor" });
            DropIndex("dbo.Registros", new[] { "IdProcesso" });
            DropIndex("dbo.Registros", new[] { "IdSite" });
            DropIndex("dbo.RegistroAcaoImediata", new[] { "IdRegistro" });
            DropIndex("dbo.RegistroAcaoImediata", new[] { "IdReponsavelImplementar" });
            DropTable("dbo.Telefone");
            DropTable("dbo.SubModulo");
            DropTable("dbo.PlaiProcessoNorma");
            DropTable("dbo.Pai");
            DropTable("dbo.Plai");
            DropTable("dbo.Log");
            DropTable("dbo.ListaValor");
            DropTable("dbo.PlanoVoo");
            DropTable("dbo.Meta");
            DropTable("dbo.PeriodicidaDeAnalise");
            DropTable("dbo.Indicador");
            DropTable("dbo.HistoricoCriterioAvaliacao");
            DropTable("dbo.Estoque");
            DropTable("dbo.Endereco");
            DropTable("dbo.DocUsuarioVerificaAprova");
            DropTable("dbo.DocTemplate");
            DropTable("dbo.DocumentoCargo");
            DropTable("dbo.DocumentoComentario");
            DropTable("dbo.DocumentoAssunto");
            DropTable("dbo.DocDocumento");
            DropTable("dbo.Perfil");
            DropTable("dbo.ControleImpressao");
            DropTable("dbo.Instrumento");
            DropTable("dbo.CriterioAceitacao");
            DropTable("dbo.Calibracao");
            DropTable("dbo.BreadCrumb");
            DropTable("dbo.TipoAdvertencia");
            DropTable("dbo.ValeTransporte");
            DropTable("dbo.FolhaDePagamento");
            DropTable("dbo.Ferias");
            DropTable("dbo.Exame");
            DropTable("dbo.EPI");
            DropTable("dbo.TipoEmprestimo");
            DropTable("dbo.Emprestimo");
            DropTable("dbo.TipoPlano");
            DropTable("dbo.Plano");
            DropTable("dbo.Parentesco");
            DropTable("dbo.Dependente");
            DropTable("dbo.Sindicato");
            DropTable("dbo.Treinamento");
            DropTable("dbo.Habilidade");
            DropTable("dbo.FormacaoAcademica");
            DropTable("dbo.Curso");
            DropTable("dbo.Conhecimento");
            DropTable("dbo.Atribuicao");
            DropTable("dbo.Competencia");
            DropTable("dbo.CargoRH");
            DropTable("dbo.Funcionario");
            DropTable("dbo.Advertencia");
            DropTable("dbo.AnaliseCriticaTema");
            DropTable("dbo.NotificacaoSmtp");
            DropTable("dbo.NotificacaoMensagem");
            DropTable("dbo.CriterioQualificacao");
            DropTable("dbo.AvaliaCriterioAvaliacao");
            DropTable("dbo.CriterioAvaliacao");
            DropTable("dbo.AvaliacaoCriticidade");
            DropTable("dbo.Produto");
            DropTable("dbo.ProdutoFornecedor");
            DropTable("dbo.Fornecedor");
            DropTable("dbo.Cadastro");
            DropTable("dbo.UsuarioClienteSite");
            DropTable("dbo.UsuarioAnexo");
            DropTable("dbo.SiteAnexo");
            DropTable("dbo.ClienteContrato");
            DropTable("dbo.Anexo");
            DropTable("dbo.ClienteLogo");
            DropTable("dbo.Cliente");
            DropTable("dbo.UsuarioCargo");
            DropTable("dbo.SiteModulo");
            DropTable("dbo.Norma");
            DropTable("dbo.NormaProcesso");
            DropTable("dbo.Processo");
            DropTable("dbo.Notificacao");
            DropTable("dbo.Funcionalidade");
            DropTable("dbo.Funcao");
            DropTable("dbo.CargoProcesso");
            DropTable("dbo.Cargo");
            DropTable("dbo.Site");
            DropTable("dbo.AnaliseCriticaFuncionario");
            DropTable("dbo.AnaliseCritica");
            DropTable("dbo.Usuario");
            DropTable("dbo.Registros");
            DropTable("dbo.RegistroAcaoImediata");
        }
    }
}
