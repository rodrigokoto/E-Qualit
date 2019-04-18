namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_colunas_indicadores : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Indicador", name: "Id", newName: "IdIndicador");
            AddColumn("dbo.Indicador", "MetaAnual", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AddColumn("dbo.Indicador", "Ano", c => c.Int(nullable: false));
            AddColumn("dbo.Indicador", "Periodicidade", c => c.Int(nullable: false));
            AlterColumn("dbo.Indicador", "Objetivo", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AlterColumn("dbo.Indicador", "Descricao", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AlterColumn("dbo.Indicador", "Unidade", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            AlterColumn("dbo.Indicador", "Direcao", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            DropColumn("dbo.Indicador", "Peso");
            DropColumn("dbo.Indicador", "Maximo");
            DropColumn("dbo.Indicador", "Minimo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Indicador", "Minimo", c => c.Byte(nullable: false));
            AddColumn("dbo.Indicador", "Maximo", c => c.Byte(nullable: false));
            AddColumn("dbo.Indicador", "Peso", c => c.Int(nullable: false));
            AlterColumn("dbo.Indicador", "Direcao", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Indicador", "Unidade", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Indicador", "Descricao", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Indicador", "Objetivo", c => c.String(maxLength: 8000, unicode: false));
            DropColumn("dbo.Indicador", "Periodicidade");
            DropColumn("dbo.Indicador", "Ano");
            DropColumn("dbo.Indicador", "MetaAnual");
            RenameColumn(table: "dbo.Indicador", name: "IdIndicador", newName: "Id");
        }
    }
}
