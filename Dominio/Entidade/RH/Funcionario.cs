using System;
using System.Collections.Generic;

namespace Dominio.Entidade.RH
{
    public class Funcionario : Base
    {
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataVencimentoFerias { get; set; }
        public DateTime DataUltimoPrazo { get; set; }
        public int NumeroRegistro { get; set; }
        public string EstadoCivil { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string TelefoneResidencial { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneRecado { get; set; }

        public string CNH { get; set; }
        public DateTime VencimentoCNH { get; set; }
        public string TituloEleitoral { get; set; }
        public string Outro { get; set; }
        public string Observacao { get; set; }

        public virtual List<Dependente> Dependentes { get; set; }
        public virtual List<Ferias> Ferias { get; set; }
        public virtual List<Advertencia> Advertencias { get; set; }
        public virtual List<EPI> Epis { get; set; }
        public virtual List<FolhaDePagamento> FolhaDePagamentos { get; set; }
        public virtual List<Exame> Exames { get; set; }
        public virtual List<ValeTransporte> ValeTransportes { get; set; }
        public virtual List<Emprestimo> Emprestimos { get; set; }

        public int? CodigoCargo { get; set; }
        public virtual CargoRH Cargo { get; set; }

        public int? CodigoCompetencia { get; set; }
        public virtual Competencia Competencia { get; set; }
    }
}
