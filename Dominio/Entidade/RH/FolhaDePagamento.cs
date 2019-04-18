namespace Dominio.Entidade.RH
{
    public class FolhaDePagamento : Base
    {
        public decimal Desconto { get; set; }
        public decimal Valor { get; set; }
        public int CodigoFuncionario { get; set; }
        public virtual Funcionario Funcionario { get; set; }
    }
}
