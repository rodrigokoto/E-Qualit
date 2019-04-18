namespace Dominio.Entidade.RH
{
    public class Advertencia : Base
    {
        public string Descricao { get; set; }
        public int CodigoTipoAdvertencia { get; set; }
        public TipoAdvertencia TipoAdvertencia { get; set; }
        public int CodigoFuncionario { get; set; }
        public virtual Funcionario Funcionario { get; set; }

    }
}
