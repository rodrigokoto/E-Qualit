using System;
using System.Collections.Generic;

namespace Dominio.Entidade.RH
{
    public class Dependente : Base
    {
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public string Documentos { get; set; }
        public int CodigoParentesco { get; set; }
        public virtual Parentesco Parentesco { get; set; }
        public int CodigoFuncionario { get; set; }
        public virtual Funcionario Funcionario { get; set; }
        public virtual List<Plano> Planos { get; set; }
    }
}
