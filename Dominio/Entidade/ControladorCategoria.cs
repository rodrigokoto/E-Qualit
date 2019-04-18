using DomainValidation.Interfaces.Validation;
using DomainValidation.Validation;

namespace Dominio.Entidade
{
    public class ControladorCategoria : ISelfValidator
    {
        public int IdControladorCategorias { get; set; }

        public int IdSite { get; set; }

        public string Descricao { get; set; }

        public string TipoTabela { get; set; }

        public bool Ativo { get; set; }

        public virtual Site Site { get; set; }

        public ValidationResult ValidationResult { get; set; } 

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }
    }
}
