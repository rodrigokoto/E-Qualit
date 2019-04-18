namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracao_Campo_Cliente : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cliente", "FlExigeSenhaForte", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cliente", "FlExigeSenhaForte", c => c.Boolean());
        }
    }
}
