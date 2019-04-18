namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adicao_Entidades_Indicadores : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Indicador",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Objetivo = c.String(maxLength: 8000, unicode: false),
                        Descricao = c.String(maxLength: 8000, unicode: false),
                        Unidade = c.String(maxLength: 8000, unicode: false),
                        Direcao = c.String(maxLength: 8000, unicode: false),
                        IdSite = c.Int(nullable: false),
                        IdUsuarioIncluiu = c.Int(nullable: false),
                        DataAlteracao = c.DateTime(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                        IdProcesso = c.Int(nullable: false),
                        IdResponsavel = c.Int(nullable: false),
                        ValidationResult_Message = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Processo", t => t.IdProcesso)
                .ForeignKey("dbo.Usuario", t => t.IdResponsavel)
                .Index(t => t.IdProcesso)
                .Index(t => t.IdResponsavel);
            
            CreateTable(
                "dbo.PeriodicidaDeAnalise",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PeriodoAnalise = c.Byte(nullable: false),
                        Justificativa = c.String(maxLength: 8000, unicode: false),
                        CorRisco = c.String(maxLength: 8000, unicode: false),
                        Inicio = c.DateTime(nullable: false),
                        Fim = c.DateTime(nullable: false),
                        IdMeta = c.Int(nullable: false),
                        IdIndicador = c.Int(nullable: false),
                        IdPlanoDeVoo = c.Int(nullable: false),
                        IdPlanDeAcao = c.Int(),
                        Indicador_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Indicador", t => t.IdIndicador)
                .ForeignKey("dbo.Meta", t => t.IdMeta)
                .ForeignKey("dbo.Registros", t => t.IdPlanDeAcao)
                .ForeignKey("dbo.PlanoVoo", t => t.IdPlanoDeVoo)
                .ForeignKey("dbo.Indicador", t => t.Indicador_Id)
                .Index(t => t.IdMeta)
                .Index(t => t.IdIndicador)
                .Index(t => t.IdPlanoDeVoo)
                .Index(t => t.IdPlanDeAcao)
                .Index(t => t.Indicador_Id);
            
            CreateTable(
                "dbo.Meta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Valor = c.Double(nullable: false),
                        UnidadeMedida = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlanoVoo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Realizado = c.Double(nullable: false),
                        DataAlteracao = c.DateTime(nullable: false),
                        DataInclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Indicador", "IdResponsavel", "dbo.Usuario");
            DropForeignKey("dbo.Indicador", "IdProcesso", "dbo.Processo");
            DropForeignKey("dbo.PeriodicidaDeAnalise", "Indicador_Id", "dbo.Indicador");
            DropForeignKey("dbo.PeriodicidaDeAnalise", "IdPlanoDeVoo", "dbo.PlanoVoo");
            DropForeignKey("dbo.PeriodicidaDeAnalise", "IdPlanDeAcao", "dbo.Registros");
            DropForeignKey("dbo.PeriodicidaDeAnalise", "IdMeta", "dbo.Meta");
            DropForeignKey("dbo.PeriodicidaDeAnalise", "IdIndicador", "dbo.Indicador");
            DropIndex("dbo.PeriodicidaDeAnalise", new[] { "Indicador_Id" });
            DropIndex("dbo.PeriodicidaDeAnalise", new[] { "IdPlanDeAcao" });
            DropIndex("dbo.PeriodicidaDeAnalise", new[] { "IdPlanoDeVoo" });
            DropIndex("dbo.PeriodicidaDeAnalise", new[] { "IdIndicador" });
            DropIndex("dbo.PeriodicidaDeAnalise", new[] { "IdMeta" });
            DropIndex("dbo.Indicador", new[] { "IdResponsavel" });
            DropIndex("dbo.Indicador", new[] { "IdProcesso" });
            DropTable("dbo.PlanoVoo");
            DropTable("dbo.Meta");
            DropTable("dbo.PeriodicidaDeAnalise");
            DropTable("dbo.Indicador");
        }
    }
}
