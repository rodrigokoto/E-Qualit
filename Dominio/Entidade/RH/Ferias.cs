using System;

namespace Dominio.Entidade.RH
{
    public class Ferias : Base
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public int Dias
        {
            get
            {
                var data = DataFinal - DataInicial;

                return data.Days;
            }

            
        }
        public decimal Remuneracao { get; set; }
        public bool TeveAdiantamentoDecimoTerceiro { get; set; }
        public string Obervacao { get; set; }
        public int CodigoFuncionario { get; set; }
        public virtual Funcionario Funcionario { get; set; }
    }
}
