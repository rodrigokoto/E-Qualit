namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Arquivo_Evidencia_NC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArquivosEvidencia",
                c => new
                    {
                        IdArquivosDeEvidencia = c.Int(nullable: false, identity: true),
                        IdRegistroConformidade = c.Int(nullable: false),
                        IdAnexo = c.Int(nullable: false),
                        TipoRegistro = c.String(nullable: false, maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => t.IdArquivosDeEvidencia)
                .ForeignKey("dbo.Anexo", t => t.IdAnexo, cascadeDelete: true)
                .ForeignKey("dbo.Registros", t => t.IdRegistroConformidade, cascadeDelete: true)
                .Index(t => t.IdRegistroConformidade)
                .Index(t => t.IdAnexo);
            
            DropColumn("dbo.Registros", "NmEvidenciaImagem");
            DropColumn("dbo.Registros", "DtRegistroEvidencia");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Registros", "DtRegistroEvidencia", c => c.DateTime());
            AddColumn("dbo.Registros", "NmEvidenciaImagem", c => c.String(maxLength: 1000, unicode: false));
            DropForeignKey("dbo.ArquivosEvidencia", "IdRegistroConformidade", "dbo.Registros");
            DropForeignKey("dbo.ArquivosEvidencia", "IdAnexo", "dbo.Anexo");
            DropIndex("dbo.ArquivosEvidencia", new[] { "IdAnexo" });
            DropIndex("dbo.ArquivosEvidencia", new[] { "IdRegistroConformidade" });
            DropTable("dbo.ArquivosEvidencia");
        }
    }
}
