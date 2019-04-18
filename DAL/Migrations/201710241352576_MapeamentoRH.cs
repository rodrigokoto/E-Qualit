namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class MapeamentoRH : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DocDocumento", "IdSite", "dbo.Site");
            DropIndex("dbo.DocDocumento", new[] { "IdSite" });
            CreateTable(
                "dbo.Advertencia",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoFuncionario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .Index(t => t.CodigoFuncionario);
            
            CreateTable(
                "dbo.Funcionario",
                c => new
                    {
                        CodigoCargo = c.Int(nullable: false, identity: true),
                        Codigo = c.Int(nullable: false),
                        Nome = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Sexo = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataNascimento = c.DateTime(nullable: false),
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
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoCompetencia = c.Int(nullable: false),
                        CodigoEmprestimo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CodigoCargo)
                .ForeignKey("dbo.CargoRH", t => t.Codigo)
                .ForeignKey("dbo.Competencia", t => t.CodigoCompetencia)
                .ForeignKey("dbo.Emprestimo", t => t.Codigo)
                .Index(t => t.Codigo)
                .Index(t => t.CodigoCompetencia);
            
            CreateTable(
                "dbo.CargoRH",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoCompetencia = c.Int(nullable: false),
                        CodigoSindicato = c.Int(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
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
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoCompetencia = c.Int(nullable: false),
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
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoCompetencia = c.Int(nullable: false),
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
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoCompetencia = c.Int(nullable: false),
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
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoCompetencia = c.Int(nullable: false),
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
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoCompetencia = c.Int(nullable: false),
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
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoCompetencia = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Competencia", t => t.CodigoCompetencia)
                .Index(t => t.CodigoCompetencia);
            
            CreateTable(
                "dbo.Sindicato",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo);
            
            CreateTable(
                "dbo.Convenio",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Desconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        ConvenioRelacionamento_Codigo = c.Int(),
                        Funcionario_CodigoCargo = c.Int(),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.ConvenioRelacionamento", t => t.ConvenioRelacionamento_Codigo)
                .ForeignKey("dbo.Funcionario", t => t.Funcionario_CodigoCargo)
                .Index(t => t.ConvenioRelacionamento_Codigo)
                .Index(t => t.Funcionario_CodigoCargo);
            
            CreateTable(
                "dbo.ConvenioRelacionamento",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        CodigoFuncionario = c.Int(),
                        CodigoDependente = c.Int(),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Dependente", t => t.CodigoDependente)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .Index(t => t.CodigoFuncionario)
                .Index(t => t.CodigoDependente);
            
            CreateTable(
                "dbo.Dependente",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataNascimento = c.DateTime(nullable: false),
                        Sexo = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Documentos = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoFuncionario = c.Int(nullable: false),
                        Convenios_Codigo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Convenio", t => t.Convenios_Codigo)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .Index(t => t.CodigoFuncionario)
                .Index(t => t.Convenios_Codigo);
            
            CreateTable(
                "dbo.Emprestimo",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Parcelas = c.Int(nullable: false),
                        DescontoFerias = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoFuncionario = c.Int(nullable: false),
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
                        Tipo = c.Int(nullable: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoFuncionario = c.Int(nullable: false),
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
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoFuncionario = c.Int(nullable: false),
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
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoFuncionario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .Index(t => t.CodigoFuncionario);
            
            CreateTable(
                "dbo.ValeTransporte",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Valor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                        CodigoFuncionario = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Funcionario", t => t.CodigoFuncionario)
                .Index(t => t.CodigoFuncionario);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Advertencia", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.ValeTransporte", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.FolhaDePagamento", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Ferias", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Exame", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.EPI", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Funcionario", "Codigo", "dbo.Emprestimo");
            DropForeignKey("dbo.Convenio", "Funcionario_CodigoCargo", "dbo.Funcionario");
            DropForeignKey("dbo.Convenio", "ConvenioRelacionamento_Codigo", "dbo.ConvenioRelacionamento");
            DropForeignKey("dbo.ConvenioRelacionamento", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.ConvenioRelacionamento", "CodigoDependente", "dbo.Dependente");
            DropForeignKey("dbo.Dependente", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Dependente", "Convenios_Codigo", "dbo.Convenio");
            DropForeignKey("dbo.Funcionario", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.Funcionario", "Codigo", "dbo.CargoRH");
            DropForeignKey("dbo.CargoRH", "CodigoSindicato", "dbo.Sindicato");
            DropForeignKey("dbo.CargoRH", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.Treinamento", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.Habilidade", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.FormacaoAcademica", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.Curso", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.Conhecimento", "CodigoCompetencia", "dbo.Competencia");
            DropForeignKey("dbo.Atribuicao", "CodigoCompetencia", "dbo.Competencia");
            DropIndex("dbo.ValeTransporte", new[] { "CodigoFuncionario" });
            DropIndex("dbo.FolhaDePagamento", new[] { "CodigoFuncionario" });
            DropIndex("dbo.Ferias", new[] { "CodigoFuncionario" });
            DropIndex("dbo.Exame", new[] { "CodigoFuncionario" });
            DropIndex("dbo.EPI", new[] { "CodigoFuncionario" });
            DropIndex("dbo.Dependente", new[] { "Convenios_Codigo" });
            DropIndex("dbo.Dependente", new[] { "CodigoFuncionario" });
            DropIndex("dbo.ConvenioRelacionamento", new[] { "CodigoDependente" });
            DropIndex("dbo.ConvenioRelacionamento", new[] { "CodigoFuncionario" });
            DropIndex("dbo.Convenio", new[] { "Funcionario_CodigoCargo" });
            DropIndex("dbo.Convenio", new[] { "ConvenioRelacionamento_Codigo" });
            DropIndex("dbo.Treinamento", new[] { "CodigoCompetencia" });
            DropIndex("dbo.Habilidade", new[] { "CodigoCompetencia" });
            DropIndex("dbo.FormacaoAcademica", new[] { "CodigoCompetencia" });
            DropIndex("dbo.Curso", new[] { "CodigoCompetencia" });
            DropIndex("dbo.Conhecimento", new[] { "CodigoCompetencia" });
            DropIndex("dbo.Atribuicao", new[] { "CodigoCompetencia" });
            DropIndex("dbo.CargoRH", new[] { "CodigoSindicato" });
            DropIndex("dbo.CargoRH", new[] { "CodigoCompetencia" });
            DropIndex("dbo.Funcionario", new[] { "CodigoCompetencia" });
            DropIndex("dbo.Funcionario", new[] { "Codigo" });
            DropIndex("dbo.Advertencia", new[] { "CodigoFuncionario" });
            DropTable("dbo.ValeTransporte");
            DropTable("dbo.FolhaDePagamento");
            DropTable("dbo.Ferias");
            DropTable("dbo.Exame");
            DropTable("dbo.EPI");
            DropTable("dbo.Emprestimo");
            DropTable("dbo.Dependente");
            DropTable("dbo.ConvenioRelacionamento");
            DropTable("dbo.Convenio");
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
            CreateIndex("dbo.DocDocumento", "IdSite");
            AddForeignKey("dbo.DocDocumento", "IdSite", "dbo.Site", "IdSite");
        }
    }
}
