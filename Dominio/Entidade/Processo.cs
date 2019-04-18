using DomainValidation.Interfaces.Validation;
using DomainValidation.Validation;
using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class Processo : ISelfValidator
    {
        public Processo()
        {
            CargoProcessoes = new List<CargoProcesso>();
        }

        public int IdProcesso { get; set; }
        public int IdSite { get; set; }
        public int? IdUsuarioIncluiu { get; set; }
        public string Atividade { get; set; }
        public string DocumentosAplicaveis { get; set; }
        public virtual List<NormaProcesso> Normas { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string Nome { get; set; }
        public bool FlAtivo { get; set; }
        public bool FlQualidade { get; set; }

        public virtual ICollection<CargoProcesso> CargoProcessoes { get; set; }
        public virtual Site Site { get; set; }
        public virtual ICollection<Notificacao> Notificacoes { get; set; }
        public virtual ICollection<Fornecedor> Fornecedores { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
