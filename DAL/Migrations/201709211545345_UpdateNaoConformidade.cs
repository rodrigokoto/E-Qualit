namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateNaoConformidade : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RegistroAcaoImediata", "IdRegistro", "dbo.Registros");
            AddForeignKey("dbo.RegistroAcaoImediata", "IdRegistro", "dbo.Registros", "IdRegistro", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegistroAcaoImediata", "IdRegistro", "dbo.Registros");
            AddForeignKey("dbo.RegistroAcaoImediata", "IdRegistro", "dbo.Registros", "IdRegistro");
        }
    }
}
