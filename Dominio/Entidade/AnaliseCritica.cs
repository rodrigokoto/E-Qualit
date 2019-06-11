using DomainValidation.Interfaces.Validation;
using System;
using System.Collections.Generic;
using DomainValidation.Validation;

namespace Dominio.Entidade
{
    public class AnaliseCritica : ISelfValidator
    {
        public AnaliseCritica()
        {
            Funcionarios = new List<AnaliseCriticaFuncionario>();
            Temas = new List<AnaliseCriticaTema>();
        }

        public int IdAnaliseCritica { get; set; }
        public string Ata { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataProximaAnalise { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public int IdSite { get; set; }
        public virtual Site Site { get; set; }

        public int IdResponsavel { get; set; }

        public long? IdFilaEnvio { get; set; }
        public virtual Usuario Responsavel { get; set; }

        public virtual List<AnaliseCriticaFuncionario> Funcionarios { get; set; }

        public virtual List<AnaliseCriticaTema> Temas { get; set; }

        public DateTime? DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
