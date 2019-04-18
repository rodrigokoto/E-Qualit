using DomainValidation.Validation;
using System;

namespace Dominio.Entidade
{
    public class CriterioAceitacao
    {
        public int IdCriterioAceitacao { get; set; }
        public string Periodicidade { get; set; }
        public string Erro { get; set; }
        public string Incerteza { get; set; }
        public string Resultado { get; set; }
        public bool? Aceito { get; set; }
        public int? IdUsuarioIncluiu { get; set; }
        public DateTime? DtInclusao { get; set; }
        public DateTime DtAlteracao { get; set; }
        public int? IdCalibracao { get; set; }

        public virtual Calibracao Calibracao { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
