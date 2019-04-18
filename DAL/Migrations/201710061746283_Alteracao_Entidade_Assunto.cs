namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracao_Entidade_Assunto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssuntoDocumento", "Revisao", c => c.String(nullable: false, maxLength: 8000, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssuntoDocumento", "Revisao");
        }
    }
}
