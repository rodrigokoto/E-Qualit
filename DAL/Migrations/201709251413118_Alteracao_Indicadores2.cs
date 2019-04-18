namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracao_Indicadores2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Endereco", "teste_Aciole");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Endereco", "teste_Aciole", c => c.Int(nullable: false));
        }
    }
}
