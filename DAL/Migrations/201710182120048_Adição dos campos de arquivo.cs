namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adiçãodoscamposdearquivo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuario", "NomeArquivo", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AddColumn("dbo.Usuario", "TipoConteudo", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AddColumn("dbo.Usuario", "Arquivo", c => c.Binary(nullable: false));
            AddColumn("dbo.Site", "NomeArquivo", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AddColumn("dbo.Site", "TipoConteudo", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AddColumn("dbo.Site", "Arquivo", c => c.Binary(nullable: false));
            AddColumn("dbo.Cliente", "NomeArquivo", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AddColumn("dbo.Cliente", "TipoConteudo", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AddColumn("dbo.Cliente", "Arquivo", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cliente", "Arquivo");
            DropColumn("dbo.Cliente", "TipoConteudo");
            DropColumn("dbo.Cliente", "NomeArquivo");
            DropColumn("dbo.Site", "Arquivo");
            DropColumn("dbo.Site", "TipoConteudo");
            DropColumn("dbo.Site", "NomeArquivo");
            DropColumn("dbo.Usuario", "Arquivo");
            DropColumn("dbo.Usuario", "TipoConteudo");
            DropColumn("dbo.Usuario", "NomeArquivo");
        }
    }
}
