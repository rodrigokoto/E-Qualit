namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracaocalibracaoidusuario : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Calibracao", "Aprovador");
            AddForeignKey("dbo.Calibracao", "Aprovador", "dbo.Usuario", "IdUsuario");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Calibracao", "Aprovador", "dbo.Usuario");
            DropIndex("dbo.Calibracao", new[] { "Aprovador" });
        }
    }
}
