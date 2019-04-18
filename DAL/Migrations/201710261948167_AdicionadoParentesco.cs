namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdicionadoParentesco : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.Dependente", "CodigoParentesco", c => c.Int(nullable: false));
            AddColumn("dbo.ValeTransporte", "Possui", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Dependente", "CodigoParentesco");
            AddForeignKey("dbo.Dependente", "CodigoParentesco", "dbo.Parentesco", "Codigo");
            DropColumn("dbo.Dependente", "Parentesco");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dependente", "Parentesco", c => c.Int(nullable: false));
            DropForeignKey("dbo.Dependente", "CodigoParentesco", "dbo.Parentesco");
            DropIndex("dbo.Dependente", new[] { "CodigoParentesco" });
            DropColumn("dbo.ValeTransporte", "Possui");
            DropColumn("dbo.Dependente", "CodigoParentesco");
            DropTable("dbo.Parentesco");
        }
    }
}
