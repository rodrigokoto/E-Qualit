namespace Dominio.Entidade.RH
{
    public class CargoRH : Base
    {
        public string Descricao { get; set; }
        public int CodigoCompetencia { get; set; }
        public virtual Competencia Competencia { get; set; }
        public int CodigoSindicato { get; set; }
        public virtual Sindicato Sindicato { get; set; }
    }
}
