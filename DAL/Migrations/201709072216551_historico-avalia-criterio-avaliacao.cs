namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class historicoavaliacriterioavaliacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvaliaCriterioAvaliacao",
                c => new
                    {
                        IdAvaliaCriterioAvaliacao = c.Int(nullable: false, identity: true),
                        IdCriterioAvaliacao = c.Int(nullable: false),
                        NotaAvaliacao = c.Int(nullable: false),
                        DtAvaliacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdAvaliaCriterioAvaliacao)
                .ForeignKey("dbo.CriterioAvaliacao", t => t.IdCriterioAvaliacao)
                .Index(t => t.IdCriterioAvaliacao);
            
            AddColumn("dbo.Produto", "StatusCriterioAvaliacao", c => c.Int(nullable: false));
            AddColumn("dbo.Produto", "StatusCriterioQualificacao", c => c.Int(nullable: false));
            DropColumn("dbo.CriterioAvaliacao", "NotaAvaliacao");
            DropColumn("dbo.CriterioAvaliacao", "DtNotaAvaliacao");
            DropColumn("dbo.CriterioAvaliacao", "DtNotaAvaliacaoAlteracao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CriterioAvaliacao", "DtNotaAvaliacaoAlteracao", c => c.DateTime());
            AddColumn("dbo.CriterioAvaliacao", "DtNotaAvaliacao", c => c.DateTime());
            AddColumn("dbo.CriterioAvaliacao", "NotaAvaliacao", c => c.Int());
            DropForeignKey("dbo.AvaliaCriterioAvaliacao", "IdCriterioAvaliacao", "dbo.CriterioAvaliacao");
            DropIndex("dbo.AvaliaCriterioAvaliacao", new[] { "IdCriterioAvaliacao" });
            DropColumn("dbo.Produto", "StatusCriterioQualificacao");
            DropColumn("dbo.Produto", "StatusCriterioAvaliacao");
            DropTable("dbo.AvaliaCriterioAvaliacao");
        }
    }
}
