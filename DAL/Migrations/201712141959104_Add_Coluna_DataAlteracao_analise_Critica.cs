namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Coluna_DataAlteracao_analise_Critica : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AnaliseCritica", "DataAlteracao", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AnaliseCritica", "DataAlteracao");
        }
    }
}
