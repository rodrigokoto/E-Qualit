namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class alteraçãomapeamentoRegistroConformidade : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Registros", new[] { "IdSite" });
            DropIndex("dbo.Registros", new[] { "IdProcesso" });
            DropIndex("dbo.Calibracao", new[] { "IdInstrumento" });
            RenameColumn(table: "dbo.Registros", name: "DsTags", newName: "Tags");
            RenameColumn(table: "dbo.Registros", name: "IdResponsavelInicarAcaoCorretiva", newName: "IdResponsavelInicarAcaoImediata");
            RenameIndex(table: "dbo.Registros", name: "IX_IdResponsavelInicarAcaoCorretiva", newName: "IX_IdResponsavelInicarAcaoImediata");
            AlterColumn("dbo.RegistroAcaoImediata", "DtPrazoImplementacao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RegistroAcaoImediata", "DtInclusao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Registros", "IdSite", c => c.Int(nullable: false));
            AlterColumn("dbo.Registros", "IdProcesso", c => c.Int(nullable: false));
            AlterColumn("dbo.Calibracao", "IdInstrumento", c => c.Int(nullable: false));
            CreateIndex("dbo.Registros", "IdSite");
            CreateIndex("dbo.Registros", "IdProcesso");
            CreateIndex("dbo.Calibracao", "IdInstrumento");
            DropColumn("dbo.Registros", "IdResponsavelIniciar");
            DropColumn("dbo.Registros", "DsEvidenciaEficaciaAcao");
            DropColumn("dbo.Registros", "XmlMetadata");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Registros", "XmlMetadata", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Registros", "DsEvidenciaEficaciaAcao", c => c.String(maxLength: 2000, unicode: false));
            AddColumn("dbo.Registros", "IdResponsavelIniciar", c => c.Int());
            DropIndex("dbo.Calibracao", new[] { "IdInstrumento" });
            DropIndex("dbo.Registros", new[] { "IdProcesso" });
            DropIndex("dbo.Registros", new[] { "IdSite" });
            AlterColumn("dbo.Calibracao", "IdInstrumento", c => c.Int());
            AlterColumn("dbo.Registros", "IdProcesso", c => c.Int());
            AlterColumn("dbo.Registros", "IdSite", c => c.Int());
            AlterColumn("dbo.RegistroAcaoImediata", "DtInclusao", c => c.DateTime());
            AlterColumn("dbo.RegistroAcaoImediata", "DtPrazoImplementacao", c => c.DateTime());
            RenameIndex(table: "dbo.Registros", name: "IX_IdResponsavelInicarAcaoImediata", newName: "IX_IdResponsavelInicarAcaoCorretiva");
            RenameColumn(table: "dbo.Registros", name: "IdResponsavelInicarAcaoImediata", newName: "IdResponsavelInicarAcaoCorretiva");
            RenameColumn(table: "dbo.Registros", name: "Tags", newName: "DsTags");
            CreateIndex("dbo.Calibracao", "IdInstrumento");
            CreateIndex("dbo.Registros", "IdProcesso");
            CreateIndex("dbo.Registros", "IdSite");
        }
    }
}
