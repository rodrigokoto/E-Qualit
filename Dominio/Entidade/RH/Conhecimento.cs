namespace Dominio.Entidade.RH
{
    public class Conhecimento : Base
    {
        public string Descricao { get; set; }
        public int CodigoCompetencia { get; set; }
        public virtual Competencia Competencia { get; set; }
    }
}
