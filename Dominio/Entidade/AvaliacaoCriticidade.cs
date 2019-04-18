using DomainValidation.Interfaces.Validation;
using System;
using DomainValidation.Validation;

namespace Dominio.Entidade
{
    public class AvaliacaoCriticidade : ISelfValidator
    {
        public AvaliacaoCriticidade()
        {
            DtCriacao = DateTime.Now;
            DtAlteracao = DateTime.Now;
        }

        public int IdAvaliacaoCriticidade { get; set; }
        public int IdProduto { get; set; }
        public string Titulo { get; set; }
        public bool Ativo { get; set; }
        public DateTime DtCriacao { get; set; }
        public DateTime DtAlteracao { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }

        #region Relacionamentos

        public virtual Produto Produto { get; set; }

        #endregion
    }
}
