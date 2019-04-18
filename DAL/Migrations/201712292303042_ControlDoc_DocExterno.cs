namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlDoc_DocExterno : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocExterno",
                c => new
                    {
                        IdDocExterno = c.Int(nullable: false, identity: true),
                        IdAnexo = c.Int(nullable: false),
                        LinkDocumentoExterno = c.String(nullable: false, maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.IdDocExterno)
                .ForeignKey("dbo.Anexo", t => t.IdAnexo, cascadeDelete: true)
                .Index(t => t.IdAnexo);
            
            AddColumn("dbo.DocDocumento", "IdDocExterno", c => c.Int());
            CreateIndex("dbo.DocDocumento", "IdDocExterno");
            AddForeignKey("dbo.DocDocumento", "IdDocExterno", "dbo.DocExterno", "IdDocExterno");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocDocumento", "IdDocExterno", "dbo.DocExterno");
            DropForeignKey("dbo.DocExterno", "IdAnexo", "dbo.Anexo");
            DropIndex("dbo.DocDocumento", new[] { "IdDocExterno" });
            DropIndex("dbo.DocExterno", new[] { "IdAnexo" });
            DropColumn("dbo.DocDocumento", "IdDocExterno");
            DropTable("dbo.DocExterno");
        }
    }
}
