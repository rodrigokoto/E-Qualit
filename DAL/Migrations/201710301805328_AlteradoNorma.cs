namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteradoNorma : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Norma", "Item");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Norma", "Item", c => c.String(nullable: false, maxLength: 8000, unicode: false));
        }
    }
}
