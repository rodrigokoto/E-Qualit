namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_ControlDoc : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RegistroDoc", newName: "DocRegistro");
            RenameTable(name: "dbo.RotinaDoc", newName: "DocRotina");
            DropPrimaryKey("dbo.DocRegistro");
            DropPrimaryKey("dbo.DocRotina");
            DropColumn("dbo.DocRegistro", "IdRegistroDoc");
            DropColumn("dbo.DocRotina", "IdRotina");
            AddColumn("dbo.DocRegistro", "IdDocRegistro", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.DocRotina", "IdDocRotina", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.DocRegistro", "IdDocRegistro");
            AddPrimaryKey("dbo.DocRotina", "IdDocRotina");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.DocRotina");
            DropPrimaryKey("dbo.DocRegistro");
            DropColumn("dbo.DocRotina", "IdDocRotina");
            DropColumn("dbo.DocRegistro", "IdDocRegistro");
            AddColumn("dbo.DocRotina", "IdRotina", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.DocRegistro", "IdRegistroDoc", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.DocRotina", "IdRotina");
            AddPrimaryKey("dbo.DocRegistro", "IdRegistroDoc");
            RenameTable(name: "dbo.DocRotina", newName: "RotinaDoc");
            RenameTable(name: "dbo.DocRegistro", newName: "RegistroDoc");
        }
    }
}
