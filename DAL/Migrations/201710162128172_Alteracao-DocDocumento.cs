namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoDocDocumento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocDocumento", "Titulo", c => c.String(maxLength: 1000, unicode: false));
            AddColumn("dbo.DocDocumento", "IdSigla", c => c.Int(nullable: false));
            AddColumn("dbo.DocDocumento", "NumeroDocumento", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.DocDocumento", "IdCategoria");
            CreateIndex("dbo.DocDocumento", "IdSigla");
            AddForeignKey("dbo.DocDocumento", "IdCategoria", "dbo.Cadastro", "IdCadastro");
            AddForeignKey("dbo.DocDocumento", "IdSigla", "dbo.Cadastro", "IdCadastro");
            DropColumn("dbo.DocDocumento", "NmTitulo");
            DropColumn("dbo.DocDocumento", "CdLetra");
            DropColumn("dbo.DocDocumento", "CdNumero");
            DropColumn("dbo.DocDocumento", "DsComentario");
            DropColumn("dbo.DocDocumento", "DsAssunto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DocDocumento", "DsAssunto", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.DocDocumento", "DsComentario", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.DocDocumento", "CdNumero", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.DocDocumento", "CdLetra", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.DocDocumento", "NmTitulo", c => c.String(maxLength: 1000, unicode: false));
            DropForeignKey("dbo.DocDocumento", "IdSigla", "dbo.Cadastro");
            DropForeignKey("dbo.DocDocumento", "IdCategoria", "dbo.Cadastro");
            DropIndex("dbo.DocDocumento", new[] { "IdSigla" });
            DropIndex("dbo.DocDocumento", new[] { "IdCategoria" });
            DropColumn("dbo.DocDocumento", "NumeroDocumento");
            DropColumn("dbo.DocDocumento", "IdSigla");
            DropColumn("dbo.DocDocumento", "Titulo");
        }
    }
}
