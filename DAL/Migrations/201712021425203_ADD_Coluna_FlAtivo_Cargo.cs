namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADD_Coluna_FlAtivo_Cargo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cargo", "FlAtivo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cargo", "FlAtivo");
        }
    }
}
