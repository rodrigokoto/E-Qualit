using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dominio.Validacao
{
    public abstract class BaseValidation
    {
        public bool IsDelete { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsInsert { get; set; }
        public List<ValidationResult> ModelErros = new List<ValidationResult>();

        public string MensagemErro
        {
            get
            {
                string _erros = string.Empty;
                if (ModelErros == null) return _erros;

                foreach (ValidationResult item in ModelErros)
                {
                    _erros += item.ErrorMessage;
                }
                return _erros;
            }
        }

        public virtual bool IsValid()
        {
            var context = new ValidationContext(this, null, null);

            bool bValida = Validator.TryValidateObject(this, context, ModelErros, true);

            return bValida;
        }
    }
}
