namespace Dominio.Entidade.RH
{
    public class Emprestimo : Base
    {
        public decimal Valor { get; set; }
        public int Parcelas { get; set; }
        public decimal DescontoFerias { get; set; }
        public int CodigoTipoEmprestimo { get; set; }
        public virtual TipoEmprestimo TipoEmprestimo { get; set; }
        public int CodigoFuncionario { get; set; }
        public virtual Funcionario Funcionario { get; set; }
    }
}
