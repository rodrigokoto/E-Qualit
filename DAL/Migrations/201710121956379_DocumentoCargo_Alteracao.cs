namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class DocumentoCargo_Alteracao : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DocumentoCargo", "IdUsuarioIncluiu");
            DropColumn("dbo.DocumentoCargo", "DtInclusao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocumentoCargo", "DtInclusao", c => c.DateTime(nullable: false));
            AddColumn("dbo.DocumentoCargo", "IdUsuarioIncluiu", c => c.Int(nullable: false));
        }
    }
}
