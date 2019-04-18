namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdicionadoSubModulo : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.SubModulo", "CodigoSite", c => c.Int(nullable: false));
            //AlterColumn("dbo.Usuario", "NmCompleto", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            //AlterColumn("dbo.Usuario", "CdIdentificacao", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            //AlterColumn("dbo.Usuario", "NuCPF", c => c.String(maxLength: 8000, unicode: false));
            //AlterColumn("dbo.Usuario", "CdSenha", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            //AlterColumn("dbo.Usuario", "NmFuncao", c => c.String(maxLength: 8000, unicode: false));
            //AlterColumn("dbo.Usuario", "FlSexo", c => c.String(nullable: false, maxLength: 128, unicode: false));
            //AlterColumn("dbo.Site", "NmLogo", c => c.String(maxLength: 8000, unicode: false));
            //AlterColumn("dbo.Site", "NmFantasia", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            //AlterColumn("dbo.Site", "NmRazaoSocial", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            //AlterColumn("dbo.Site", "NuCNPJ", c => c.String(maxLength: 8000, unicode: false));
            //AlterColumn("dbo.Site", "DsObservacoes", c => c.String(maxLength: 8000, unicode: false));
            //AlterColumn("dbo.Site", "DsFrase", c => c.String(maxLength: 8000, unicode: false));
            //CreateIndex("dbo.SubModulo", "CodigoSite");
            //AddForeignKey("dbo.SubModulo", "CodigoSite", "dbo.Site", "IdSite");
            //DropColumn("dbo.Usuario", "NmArquivoFoto");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Usuario", "NmArquivoFoto", c => c.String(nullable: false, maxLength: 80, unicode: false));
            //DropForeignKey("dbo.SubModulo", "CodigoSite", "dbo.Site");
            //DropIndex("dbo.SubModulo", new[] { "CodigoSite" });
            //AlterColumn("dbo.Site", "DsFrase", c => c.String(maxLength: 150, unicode: false));
            //AlterColumn("dbo.Site", "DsObservacoes", c => c.String(maxLength: 2000, unicode: false));
            //AlterColumn("dbo.Site", "NuCNPJ", c => c.String(maxLength: 14, unicode: false));
            //AlterColumn("dbo.Site", "NmRazaoSocial", c => c.String(nullable: false, maxLength: 60, unicode: false));
            //AlterColumn("dbo.Site", "NmFantasia", c => c.String(nullable: false, maxLength: 40, unicode: false));
            //AlterColumn("dbo.Site", "NmLogo", c => c.String(maxLength: 40, unicode: false));
            //AlterColumn("dbo.Usuario", "FlSexo", c => c.String(nullable: false, maxLength: 1, unicode: false));
            //AlterColumn("dbo.Usuario", "NmFuncao", c => c.String(maxLength: 100, unicode: false));
            //AlterColumn("dbo.Usuario", "CdSenha", c => c.String(nullable: false, maxLength: 120, unicode: false));
            //AlterColumn("dbo.Usuario", "NuCPF", c => c.String(maxLength: 30, unicode: false));
            //AlterColumn("dbo.Usuario", "CdIdentificacao", c => c.String(nullable: false, maxLength: 120, unicode: false));
            //AlterColumn("dbo.Usuario", "NmCompleto", c => c.String(nullable: false, maxLength: 30, unicode: false));
            //DropColumn("dbo.SubModulo", "CodigoSite");
        }
    }
}
