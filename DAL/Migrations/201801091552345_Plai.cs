namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Plai : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PlaiProcessoNorma", "IdPlai", "dbo.Plai");
            DropForeignKey("dbo.PlaiProcessoNorma", "IdProcesso", "dbo.Processo");
            AddColumn("dbo.Plai", "IdElaborador", c => c.Int(nullable: false));
            AddColumn("dbo.Plai", "IdArquivo", c => c.Int());
            CreateIndex("dbo.Plai", "IdArquivo");
            AddForeignKey("dbo.Plai", "IdArquivo", "dbo.Anexo", "IdAnexo");
            AddForeignKey("dbo.Plai", "IdPai", "dbo.Usuario", "IdUsuario");
            AddForeignKey("dbo.PlaiProcessoNorma", "IdPlai", "dbo.Plai", "IdPlai", cascadeDelete: true);
            AddForeignKey("dbo.PlaiProcessoNorma", "IdProcesso", "dbo.Processo", "IdProcesso", cascadeDelete: true);
            DropColumn("dbo.Plai", "IdSite");
            DropColumn("dbo.Plai", "Arquivo");
            DropColumn("dbo.Plai", "Endereco");
            DropColumn("dbo.Plai", "Escopo");
            DropColumn("dbo.Plai", "Gestores");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Plai", "Gestores", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AddColumn("dbo.Plai", "Escopo", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AddColumn("dbo.Plai", "Endereco", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AddColumn("dbo.Plai", "Arquivo", c => c.Binary());
            AddColumn("dbo.Plai", "IdSite", c => c.Int(nullable: false));
            DropForeignKey("dbo.PlaiProcessoNorma", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.PlaiProcessoNorma", "IdPlai", "dbo.Plai");
            DropForeignKey("dbo.Plai", "IdPai", "dbo.Usuario");
            DropForeignKey("dbo.Plai", "IdArquivo", "dbo.Anexo");
            DropIndex("dbo.Plai", new[] { "IdArquivo" });
            DropColumn("dbo.Plai", "IdArquivo");
            DropColumn("dbo.Plai", "IdElaborador");
            AddForeignKey("dbo.PlaiProcessoNorma", "IdProcesso", "dbo.Processo", "IdProcesso");
            AddForeignKey("dbo.PlaiProcessoNorma", "IdPlai", "dbo.Plai", "IdPlai");
        }
    }
}
