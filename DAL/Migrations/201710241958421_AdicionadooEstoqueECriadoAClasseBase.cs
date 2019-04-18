namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdicionadooEstoqueECriadoAClasseBase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Funcionario", "Codigo", "dbo.CargoRH");
            DropForeignKey("dbo.Dependente", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Emprestimo", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.EPI", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Exame", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Ferias", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.FolhaDePagamento", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.ValeTransporte", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Advertencia", "CodigoFuncionario", "dbo.Funcionario");
            DropIndex("dbo.Funcionario", new[] { "Codigo" });
            DropPrimaryKey("dbo.Funcionario");
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
            
            AddColumn("dbo.Advertencia", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Funcionario", "Cargo_Codigo", c => c.Int());
            AddColumn("dbo.Atribuicao", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Conhecimento", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.FormacaoAcademica", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Habilidade", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Treinamento", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Sindicato", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Dependente", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Emprestimo", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.EPI", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Exame", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Ferias", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.FolhaDePagamento", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.ValeTransporte", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Funcionario", "CodigoCargo", c => c.Int(nullable: false));
            AlterColumn("dbo.Funcionario", "Codigo", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Funcionario", "Codigo");
            CreateIndex("dbo.Funcionario", "Cargo_Codigo");
            AddForeignKey("dbo.Funcionario", "Cargo_Codigo", "dbo.CargoRH", "Codigo");
            AddForeignKey("dbo.Dependente", "CodigoFuncionario", "dbo.Funcionario", "Codigo");
            AddForeignKey("dbo.Emprestimo", "CodigoFuncionario", "dbo.Funcionario", "Codigo");
            AddForeignKey("dbo.EPI", "CodigoFuncionario", "dbo.Funcionario", "Codigo");
            AddForeignKey("dbo.Exame", "CodigoFuncionario", "dbo.Funcionario", "Codigo");
            AddForeignKey("dbo.Ferias", "CodigoFuncionario", "dbo.Funcionario", "Codigo");
            AddForeignKey("dbo.FolhaDePagamento", "CodigoFuncionario", "dbo.Funcionario", "Codigo");
            AddForeignKey("dbo.ValeTransporte", "CodigoFuncionario", "dbo.Funcionario", "Codigo");
            AddForeignKey("dbo.Advertencia", "CodigoFuncionario", "dbo.Funcionario", "Codigo");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Advertencia", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.ValeTransporte", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.FolhaDePagamento", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Ferias", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Exame", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.EPI", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Emprestimo", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Dependente", "CodigoFuncionario", "dbo.Funcionario");
            DropForeignKey("dbo.Funcionario", "Cargo_Codigo", "dbo.CargoRH");
            DropForeignKey("dbo.Estoque", "CodigoEPI", "dbo.EPI");
            DropIndex("dbo.Estoque", new[] { "CodigoEPI" });
            DropIndex("dbo.Funcionario", new[] { "Cargo_Codigo" });
            DropPrimaryKey("dbo.Funcionario");
            AlterColumn("dbo.Funcionario", "Codigo", c => c.Int(nullable: false));
            AlterColumn("dbo.Funcionario", "CodigoCargo", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.ValeTransporte", "ValidationResult_Message");
            DropColumn("dbo.FolhaDePagamento", "ValidationResult_Message");
            DropColumn("dbo.Ferias", "ValidationResult_Message");
            DropColumn("dbo.Exame", "ValidationResult_Message");
            DropColumn("dbo.EPI", "ValidationResult_Message");
            DropColumn("dbo.Emprestimo", "ValidationResult_Message");
            DropColumn("dbo.Dependente", "ValidationResult_Message");
            DropColumn("dbo.Sindicato", "ValidationResult_Message");
            DropColumn("dbo.Treinamento", "ValidationResult_Message");
            DropColumn("dbo.Habilidade", "ValidationResult_Message");
            DropColumn("dbo.FormacaoAcademica", "ValidationResult_Message");
            DropColumn("dbo.Conhecimento", "ValidationResult_Message");
            DropColumn("dbo.Atribuicao", "ValidationResult_Message");
            DropColumn("dbo.Funcionario", "Cargo_Codigo");
            DropColumn("dbo.Advertencia", "ValidationResult_Message");
            DropTable("dbo.Estoque");
            AddPrimaryKey("dbo.Funcionario", "CodigoCargo");
            CreateIndex("dbo.Funcionario", "Codigo");
            AddForeignKey("dbo.Advertencia", "CodigoFuncionario", "dbo.Funcionario", "CodigoCargo");
            AddForeignKey("dbo.ValeTransporte", "CodigoFuncionario", "dbo.Funcionario", "CodigoCargo");
            AddForeignKey("dbo.FolhaDePagamento", "CodigoFuncionario", "dbo.Funcionario", "CodigoCargo");
            AddForeignKey("dbo.Ferias", "CodigoFuncionario", "dbo.Funcionario", "CodigoCargo");
            AddForeignKey("dbo.Exame", "CodigoFuncionario", "dbo.Funcionario", "CodigoCargo");
            AddForeignKey("dbo.EPI", "CodigoFuncionario", "dbo.Funcionario", "CodigoCargo");
            AddForeignKey("dbo.Emprestimo", "CodigoFuncionario", "dbo.Funcionario", "CodigoCargo");
            AddForeignKey("dbo.Dependente", "CodigoFuncionario", "dbo.Funcionario", "CodigoCargo");
            AddForeignKey("dbo.Funcionario", "Codigo", "dbo.CargoRH", "Codigo");
        }
    }
}
