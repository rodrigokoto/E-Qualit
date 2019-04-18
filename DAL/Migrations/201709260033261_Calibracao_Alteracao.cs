namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Calibracao_Alteracao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Calibracao", "DataRegistro", c => c.DateTime(nullable: false));
            AddColumn("dbo.Calibracao", "DataNotificacao", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Calibracao", "DataNotificacao");
            DropColumn("dbo.Calibracao", "DataRegistro");
        }
    }
}
