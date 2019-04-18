namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Entidades_Comentario_Assunto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssuntoDocumento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdDocumento = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataAssunto = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocDocumento", t => t.IdDocumento)
                .Index(t => t.IdDocumento);
            
            CreateTable(
                "dbo.ComentarioDocumento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdDocumento = c.Int(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        Descricao = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DataComentario = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocDocumento", t => t.IdDocumento)
                .Index(t => t.IdDocumento);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComentarioDocumento", "IdDocumento", "dbo.DocDocumento");
            DropForeignKey("dbo.AssuntoDocumento", "IdDocumento", "dbo.DocDocumento");
            DropIndex("dbo.ComentarioDocumento", new[] { "IdDocumento" });
            DropIndex("dbo.AssuntoDocumento", new[] { "IdDocumento" });
            DropTable("dbo.ComentarioDocumento");
            DropTable("dbo.AssuntoDocumento");
        }
    }
}
