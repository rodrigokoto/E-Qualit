namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adicao_Entidades_Indicadores_Alteracao1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meta", "DataReferencia", c => c.DateTime(nullable: false));
            AddColumn("dbo.PlanoVoo", "DataReferencia", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PlanoVoo", "DataReferencia");
            DropColumn("dbo.Meta", "DataReferencia");
        }
    }
}
