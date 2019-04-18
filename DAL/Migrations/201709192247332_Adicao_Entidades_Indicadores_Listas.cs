namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adicao_Entidades_Indicadores_Listas : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PeriodicidaDeAnalise", "IdMeta", "dbo.Meta");
            DropForeignKey("dbo.PeriodicidaDeAnalise", "IdPlanoDeVoo", "dbo.PlanoVoo");
            DropIndex("dbo.PeriodicidaDeAnalise", new[] { "IdMeta" });
            DropIndex("dbo.PeriodicidaDeAnalise", new[] { "IdPlanoDeVoo" });
            AddColumn("dbo.Meta", "IdPeriodicidadeAnalise", c => c.Int(nullable: false));
            AddColumn("dbo.PlanoVoo", "IdPeriodicidadeAnalise", c => c.Int(nullable: false));
            CreateIndex("dbo.Meta", "IdPeriodicidadeAnalise");
            CreateIndex("dbo.PlanoVoo", "IdPeriodicidadeAnalise");
            AddForeignKey("dbo.Meta", "IdPeriodicidadeAnalise", "dbo.PeriodicidaDeAnalise", "Id");
            AddForeignKey("dbo.PlanoVoo", "IdPeriodicidadeAnalise", "dbo.PeriodicidaDeAnalise", "Id");
            DropColumn("dbo.PeriodicidaDeAnalise", "IdMeta");
            DropColumn("dbo.PeriodicidaDeAnalise", "IdPlanoDeVoo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PeriodicidaDeAnalise", "IdPlanoDeVoo", c => c.Int(nullable: false));
            AddColumn("dbo.PeriodicidaDeAnalise", "IdMeta", c => c.Int(nullable: false));
            DropForeignKey("dbo.PlanoVoo", "IdPeriodicidadeAnalise", "dbo.PeriodicidaDeAnalise");
            DropForeignKey("dbo.Meta", "IdPeriodicidadeAnalise", "dbo.PeriodicidaDeAnalise");
            DropIndex("dbo.PlanoVoo", new[] { "IdPeriodicidadeAnalise" });
            DropIndex("dbo.Meta", new[] { "IdPeriodicidadeAnalise" });
            DropColumn("dbo.PlanoVoo", "IdPeriodicidadeAnalise");
            DropColumn("dbo.Meta", "IdPeriodicidadeAnalise");
            CreateIndex("dbo.PeriodicidaDeAnalise", "IdPlanoDeVoo");
            CreateIndex("dbo.PeriodicidaDeAnalise", "IdMeta");
            AddForeignKey("dbo.PeriodicidaDeAnalise", "IdPlanoDeVoo", "dbo.PlanoVoo", "Id");
            AddForeignKey("dbo.PeriodicidaDeAnalise", "IdMeta", "dbo.Meta", "Id");
        }
    }
}
