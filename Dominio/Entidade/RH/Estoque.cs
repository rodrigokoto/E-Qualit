namespace Dominio.Entidade.RH
{
    public class Estoque : Base
    {
        public int Quantidade { get; set; }
        public int CodigoEPI { get; set; }
        public virtual EPI EPI { get; set; }
    }
}
