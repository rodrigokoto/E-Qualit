namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoDocumentoCargo : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DocumentoCargo", "Ativo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocumentoCargo", "Ativo", c => c.Boolean(nullable: false));
        }
    }
}
