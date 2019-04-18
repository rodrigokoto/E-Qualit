namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AdicaoHistorioAvaliacaoCriterioAvaliacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HistoricoCriterioAvaliacao",
                c => new
                    {
                        IdHistoricoCriterioAvaliacao = c.Int(nullable: false, identity: true),
                        IdCriterioAvaliacao = c.Int(nullable: false),
                        Nota = c.Int(nullable: false),
                        DtCriacao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IdHistoricoCriterioAvaliacao)
                .ForeignKey("dbo.CriterioAvaliacao", t => t.IdCriterioAvaliacao)
                .Index(t => t.IdCriterioAvaliacao);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HistoricoCriterioAvaliacao", "IdCriterioAvaliacao", "dbo.CriterioAvaliacao");
            DropIndex("dbo.HistoricoCriterioAvaliacao", new[] { "IdCriterioAvaliacao" });
            DropTable("dbo.HistoricoCriterioAvaliacao");
        }
    }
}
