using DomainValidation.Interfaces.Validation;
using DomainValidation.Validation;
using System;

namespace Dominio.Entidade.RH
{
    public class Base : ISelfValidator
    {
        public int Codigo { get; set; }

        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public ValidationResult ValidationResult { get; set; }
        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
