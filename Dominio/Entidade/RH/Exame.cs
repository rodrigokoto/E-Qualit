namespace Dominio.Entidade.RH
{
    public class Exame : Base
    {
        public string Descricao { get; set; }
        public int CodigoFuncionario { get; set; }
        public virtual Funcionario Funcionario { get; set; }
    }
}
