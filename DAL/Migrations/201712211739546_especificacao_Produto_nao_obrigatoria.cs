namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class especificacao_Produto_nao_obrigatoria : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Produto", "Especificacao", c => c.String(maxLength: 8000, unicode: false));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.Produto", "Especificacao", c => c.String(nullable: false, maxLength: 8000, unicode: false));
        }
    }
}
