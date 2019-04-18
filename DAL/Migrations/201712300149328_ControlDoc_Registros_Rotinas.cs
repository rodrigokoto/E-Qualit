namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlDoc_Registros_Rotinas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegistroDoc",
                c => new
                    {
                        IdRegistroDoc = c.Int(nullable: false, identity: true),
                        IdDocumento = c.Int(nullable: false),
                        Identificar = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Armazenar = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Proteger = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Retencao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Recuperar = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Disposicao = c.String(nullable: false, maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.IdRegistroDoc)
                .ForeignKey("dbo.DocDocumento", t => t.IdDocumento, cascadeDelete: true)
                .Index(t => t.IdDocumento);
            
            CreateTable(
                "dbo.RotinaDoc",
                c => new
                    {
                        IdRotina = c.Int(nullable: false, identity: true),
                        IdDocumento = c.Int(nullable: false),
                        OQue = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Quem = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Registro = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Como = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Item = c.String(nullable: false, maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.IdRotina)
                .ForeignKey("dbo.DocDocumento", t => t.IdDocumento, cascadeDelete: true)
                .Index(t => t.IdDocumento);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RotinaDoc", "IdDocumento", "dbo.DocDocumento");
            DropForeignKey("dbo.RegistroDoc", "IdDocumento", "dbo.DocDocumento");
            DropIndex("dbo.RotinaDoc", new[] { "IdDocumento" });
            DropIndex("dbo.RegistroDoc", new[] { "IdDocumento" });
            DropTable("dbo.RotinaDoc");
            DropTable("dbo.RegistroDoc");
        }
    }
}
