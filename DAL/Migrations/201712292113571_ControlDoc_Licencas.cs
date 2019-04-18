namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlDoc_Licencas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Licenca",
                c => new
                    {
                        IdLicenca = c.Int(nullable: false, identity: true),
                        IdAnexo = c.Int(nullable: false),
                        DataEmissao = c.DateTime(nullable: false),
                        DataVencimento = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdLicenca)
                .ForeignKey("dbo.Anexo", t => t.IdAnexo, cascadeDelete: true)
                .Index(t => t.IdAnexo);
            
            AddColumn("dbo.DocDocumento", "IdLicenca", c => c.Int(nullable: false));
            CreateIndex("dbo.DocDocumento", "IdLicenca");
            AddForeignKey("dbo.DocDocumento", "IdLicenca", "dbo.Licenca", "IdLicenca");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocDocumento", "IdLicenca", "dbo.Licenca");
            DropForeignKey("dbo.Licenca", "IdAnexo", "dbo.Anexo");
            DropIndex("dbo.DocDocumento", new[] { "IdLicenca" });
            DropIndex("dbo.Licenca", new[] { "IdAnexo" });
            DropColumn("dbo.DocDocumento", "IdLicenca");
            DropTable("dbo.Licenca");
        }
    }
}
