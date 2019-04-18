namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdicaoDaEntidadeControleImpressao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ControleImpressao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdFuncionalidade = c.Int(nullable: false),
                        CodigoReferencia = c.String(nullable: false, maxLength: 8000, unicode: false),
                        CopiaControlada = c.Boolean(nullable: false),
                        IdUsuarioDestino = c.Int(nullable: false),
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ControleImpressao", "IdUsuarioDestino", "dbo.Usuario");
            DropForeignKey("dbo.ControleImpressao", "IdUsuarioIncluiu", "dbo.Usuario");
            DropForeignKey("dbo.ControleImpressao", "IdFuncionalidade", "dbo.Funcionalidade");
            DropIndex("dbo.ControleImpressao", new[] { "IdUsuarioIncluiu" });
            DropIndex("dbo.ControleImpressao", new[] { "IdUsuarioDestino" });
            DropIndex("dbo.ControleImpressao", new[] { "IdFuncionalidade" });
            DropTable("dbo.ControleImpressao");
        }
    }
}
