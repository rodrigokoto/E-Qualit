namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracao_Indicadores1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Endereco", "teste_Aciole", c => c.Int(nullable: false));
            AddColumn("dbo.Indicador", "Maximo", c => c.Byte(nullable: false));
            AddColumn("dbo.Indicador", "Minimo", c => c.Byte(nullable: false));
            AddColumn("dbo.PeriodicidaDeAnalise", "RealAcumulado", c => c.Double(nullable: false));
            AddColumn("dbo.PeriodicidaDeAnalise", "MetaEstimulada", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PeriodicidaDeAnalise", "MetaEstimulada");
            DropColumn("dbo.PeriodicidaDeAnalise", "RealAcumulado");
            DropColumn("dbo.Indicador", "Minimo");
            DropColumn("dbo.Indicador", "Maximo");
            DropColumn("dbo.Endereco", "teste_Aciole");
        }
    }
}
