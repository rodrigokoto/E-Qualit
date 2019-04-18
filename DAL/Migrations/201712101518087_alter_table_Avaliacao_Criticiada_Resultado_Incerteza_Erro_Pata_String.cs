namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alter_table_Avaliacao_Criticiada_Resultado_Incerteza_Erro_Pata_String : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CriterioAceitacao", "Erro", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.CriterioAceitacao", "Incerteza", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.CriterioAceitacao", "Resultado", c => c.String(maxLength: 8000, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CriterioAceitacao", "Resultado", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CriterioAceitacao", "Incerteza", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CriterioAceitacao", "Erro", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
