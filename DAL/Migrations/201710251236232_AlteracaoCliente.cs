namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoCliente : DbMigration
    {
        public override void Up()

        {

            AlterColumn("dbo.Cliente", "NmFantasia", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AlterColumn("dbo.Cliente", "NmLogo", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Cliente", "NmAquivoContrato", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Cliente", "NmUrlAcesso", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            DropColumn("dbo.Cliente", "NomeArquivo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cliente", "NomeArquivo", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Cliente", "NmUrlAcesso", c => c.String(nullable: false, maxLength: 60, unicode: false));
            AlterColumn("dbo.Cliente", "NmAquivoContrato", c => c.String(maxLength: 1000, unicode: false));
            AlterColumn("dbo.Cliente", "NmLogo", c => c.String(maxLength: 80, unicode: false));
            AlterColumn("dbo.Cliente", "NmFantasia", c => c.String(nullable: false, maxLength: 40, unicode: false));
        }
    }
}
