namespace Dominio.Entidade.RH
{
    public class Plano : Base
    {
        public string Descricao { get; set; }
        public int CodigoTipoPlano { get; set; }
        public virtual TipoPlano TipoPlano { get; set; }
    }
}
