namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddColunaArquivoContratoemCliente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cliente", "ArquivoContrato", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cliente", "ArquivoContrato");
        }
    }
}
