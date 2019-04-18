using DomainValidation.Validation;
using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class Cargo
    {
        public Cargo()
        {
            CargoProcessos = new List<CargoProcesso>();
            UsuarioCargos = new List<UsuarioCargo>();
        }

        public int IdCargo { get; set; }
        public int IdSite { get; set; }
        public string NmNome { get; set; }
        public bool Ativo { get; set; }
        public int? IdUsuarioIncluiu { get; set; }
        public DateTime? DtInclusao { get; set; }
        public virtual Site Site { get; set; }
        public virtual ICollection<CargoProcesso> CargoProcessos { get; set; }
        public virtual ICollection<UsuarioCargo> UsuarioCargos { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
