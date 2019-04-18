namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ClienteAlteracaoCampoContrato : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cliente", "NmAquivoContrato", c => c.String(maxLength: 1000, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cliente", "NmAquivoContrato", c => c.String(maxLength: 80, unicode: false));
        }
    }
}
