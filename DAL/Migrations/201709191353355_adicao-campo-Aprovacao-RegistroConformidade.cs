namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class adicaocampoAprovacaoRegistroConformidade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registros", "Aprovado", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registros", "Aprovado");
        }
    }
}
