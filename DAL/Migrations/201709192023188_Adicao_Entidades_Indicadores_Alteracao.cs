namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adicao_Entidades_Indicadores_Alteracao : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Indicador", "ValidationResult_Message");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Indicador", "ValidationResult_Message", c => c.String(maxLength: 8000, unicode: false));
        }
    }
}
