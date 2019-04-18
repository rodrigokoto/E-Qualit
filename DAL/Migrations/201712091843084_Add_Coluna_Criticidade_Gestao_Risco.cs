namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Coluna_Criticidade_Gestao_Risco : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registros", "CriticidadeGestaoRisco", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Registros", "CriticidadeGestaoRisco");
        }
    }
}
