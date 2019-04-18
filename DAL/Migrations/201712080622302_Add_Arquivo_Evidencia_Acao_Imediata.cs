namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Arquivo_Evidencia_Acao_Imediata : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArquivoDeEvidenciaAcaoImediata",
                c => new
                    {
                        IdArquivoDeEvidenciaAcaoImediata = c.Int(nullable: false, identity: true),
                        IdAcaoImediata = c.Int(nullable: false),
                        IdAnexo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdArquivoDeEvidenciaAcaoImediata)
                .ForeignKey("dbo.RegistroAcaoImediata", t => t.IdAcaoImediata, cascadeDelete: true)
                .ForeignKey("dbo.Anexo", t => t.IdAnexo, cascadeDelete: true)
                .Index(t => t.IdAcaoImediata)
                .Index(t => t.IdAnexo);
            
            DropColumn("dbo.RegistroAcaoImediata", "NmArquivoEvidencia");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RegistroAcaoImediata", "NmArquivoEvidencia", c => c.String(maxLength: 1000, unicode: false));
            DropForeignKey("dbo.ArquivoDeEvidenciaAcaoImediata", "IdAnexo", "dbo.Anexo");
            DropForeignKey("dbo.ArquivoDeEvidenciaAcaoImediata", "IdAcaoImediata", "dbo.RegistroAcaoImediata");
            DropIndex("dbo.ArquivoDeEvidenciaAcaoImediata", new[] { "IdAnexo" });
            DropIndex("dbo.ArquivoDeEvidenciaAcaoImediata", new[] { "IdAcaoImediata" });
            DropTable("dbo.ArquivoDeEvidenciaAcaoImediata");
        }
    }
}
