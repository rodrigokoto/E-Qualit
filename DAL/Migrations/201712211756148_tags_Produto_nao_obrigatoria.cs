namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tags_Produto_nao_obrigatoria : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Produto", "Tags", c => c.String(maxLength: 8000, unicode: false));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.Produto", "Tags", c => c.String(nullable: false, maxLength: 8000, unicode: false));
        }
    }
}
