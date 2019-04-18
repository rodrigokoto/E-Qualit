using System;

namespace Dominio.Entidade.RH
{
    public class ValeTransporte : Base
    {
        public decimal ValorCredito { get; set; }
        public decimal ValorDescontoMensal { get; set; }
        public DateTime DataVigencia { get; set; }
        public bool Possui { get; set; }
        public int CodigoFuncionario { get; set; }
        public virtual Funcionario Funcionario { get; set; }
    }
}
