using DomainValidation.Interfaces.Validation;
using DomainValidation.Validation;
using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class CriterioQualificacao : ISelfValidator
    {
        public CriterioQualificacao()
        {
            DtCriacao = DateTime.Now;
            DtAlteracao = DateTime.Now;
        }

        public int IdCriterioQualificacao { get; set; }
        public int IdProduto { get; set; }
        public string Titulo { get; set; }

        public bool TemControleVencimento { get; set; }
        
        public DateTime DtCriacao { get; set; }
        public DateTime DtAlteracao { get; set; }

        public bool Ativo { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }

        #region Relacionamento

        public virtual Produto Produto { get; set; }
        public virtual ICollection<AvaliaCriterioQualificacao> AvaliaCriteriosQualificacao { get; set; }
        
        #endregion
    }
}
