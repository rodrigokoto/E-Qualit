namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class adicaocampoAprovacaoRegistroAcaoImediata : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RegistroAcaoImediata", "Aprovado", c => c.Boolean());
            DropColumn("dbo.Registros", "Aprovado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Registros", "Aprovado", c => c.Boolean());
            DropColumn("dbo.RegistroAcaoImediata", "Aprovado");
        }
    }
}
