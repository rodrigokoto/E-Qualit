namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlDoc_Texto_Fluxo_Recurso : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocDocumento", "TextoDoc", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.DocDocumento", "FluxoDoc", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.DocDocumento", "RecursoDoc", c => c.String(maxLength: 8000, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DocDocumento", "RecursoDoc");
            DropColumn("dbo.DocDocumento", "FluxoDoc");
            DropColumn("dbo.DocDocumento", "TextoDoc");
        }
    }
}
