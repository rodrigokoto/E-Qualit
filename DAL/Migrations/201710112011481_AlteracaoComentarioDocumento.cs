namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoComentarioDocumento : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ComentarioDocumento", newName: "DocumentoComentario");
            CreateIndex("dbo.DocumentoComentario", "IdUsuario");
            AddForeignKey("dbo.DocumentoComentario", "IdUsuario", "dbo.Usuario", "IdUsuario");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentoComentario", "IdUsuario", "dbo.Usuario");
            DropIndex("dbo.DocumentoComentario", new[] { "IdUsuario" });
            RenameTable(name: "dbo.DocumentoComentario", newName: "ComentarioDocumento");
        }
    }
}
