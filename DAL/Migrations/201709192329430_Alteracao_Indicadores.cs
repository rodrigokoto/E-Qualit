namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracao_Indicadores : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Indicador", "Peso", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Indicador", "Peso");
        }
    }
}
