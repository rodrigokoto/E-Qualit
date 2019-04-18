namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdicionadoOsDevidosTipos : DbMigration
    {
        public override void Up()
        {
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
                "dbo.TipoAdvertencia",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataCadastro = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo);
            
            AddColumn("dbo.Advertencia", "CodigoTipoAdvertencia", c => c.Int(nullable: false));
            AddColumn("dbo.Plano", "CodigoTipoPlano", c => c.Int(nullable: false));
            AddColumn("dbo.Emprestimo", "CodigoTipoEmprestimo", c => c.Int(nullable: false));
            CreateIndex("dbo.Advertencia", "CodigoTipoAdvertencia");
            CreateIndex("dbo.Plano", "CodigoTipoPlano");
            CreateIndex("dbo.Emprestimo", "CodigoTipoEmprestimo");
            AddForeignKey("dbo.Plano", "CodigoTipoPlano", "dbo.TipoPlano", "Codigo");
            AddForeignKey("dbo.Emprestimo", "CodigoTipoEmprestimo", "dbo.TipoEmprestimo", "Codigo");
            AddForeignKey("dbo.Advertencia", "CodigoTipoAdvertencia", "dbo.TipoAdvertencia", "Codigo");
            DropColumn("dbo.Advertencia", "Tipo");
            DropColumn("dbo.Advertencia", "ValidationResult_Message");
            DropColumn("dbo.Atribuicao", "ValidationResult_Message");
            DropColumn("dbo.Plano", "Tipo");
            DropColumn("dbo.Emprestimo", "Tipo");
            DropColumn("dbo.Emprestimo", "ValidationResult_Message");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Emprestimo", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Emprestimo", "Tipo", c => c.Int(nullable: false));
            AddColumn("dbo.Plano", "Tipo", c => c.Int(nullable: false));
            AddColumn("dbo.Atribuicao", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Advertencia", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Advertencia", "Tipo", c => c.Int(nullable: false));
            DropForeignKey("dbo.Advertencia", "CodigoTipoAdvertencia", "dbo.TipoAdvertencia");
            DropForeignKey("dbo.Emprestimo", "CodigoTipoEmprestimo", "dbo.TipoEmprestimo");
            DropForeignKey("dbo.Plano", "CodigoTipoPlano", "dbo.TipoPlano");
            DropIndex("dbo.Emprestimo", new[] { "CodigoTipoEmprestimo" });
            DropIndex("dbo.Plano", new[] { "CodigoTipoPlano" });
            DropIndex("dbo.Advertencia", new[] { "CodigoTipoAdvertencia" });
            DropColumn("dbo.Emprestimo", "CodigoTipoEmprestimo");
            DropColumn("dbo.Plano", "CodigoTipoPlano");
            DropColumn("dbo.Advertencia", "CodigoTipoAdvertencia");
            DropTable("dbo.TipoAdvertencia");
            DropTable("dbo.TipoEmprestimo");
            DropTable("dbo.TipoPlano");
        }
    }
}
