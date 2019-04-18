namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correcao_de_tipo_double_para_decimal_criterio : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CriterioAceitacao", "Erro", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CriterioAceitacao", "Incerteza", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CriterioAceitacao", "Resultado", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.AnaliseCriticaTema", "Risco");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AnaliseCriticaTema", "Risco", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AlterColumn("dbo.CriterioAceitacao", "Resultado", c => c.Double());
            AlterColumn("dbo.CriterioAceitacao", "Incerteza", c => c.Double());
            AlterColumn("dbo.CriterioAceitacao", "Erro", c => c.Double());
        }
    }
}
