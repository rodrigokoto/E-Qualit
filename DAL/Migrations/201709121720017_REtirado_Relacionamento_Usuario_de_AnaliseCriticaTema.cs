namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class REtirado_Relacionamento_Usuario_de_AnaliseCriticaTema : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AnaliseCriticaTema", "IdUsuario", "dbo.Usuario");
            DropIndex("dbo.AnaliseCriticaTema", new[] { "IdUsuario" });
            DropColumn("dbo.AnaliseCriticaTema", "IdUsuario");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AnaliseCriticaTema", "IdUsuario", c => c.Int(nullable: false));
            CreateIndex("dbo.AnaliseCriticaTema", "IdUsuario");
            AddForeignKey("dbo.AnaliseCriticaTema", "IdUsuario", "dbo.Usuario", "IdUsuario");
        }
    }
}
