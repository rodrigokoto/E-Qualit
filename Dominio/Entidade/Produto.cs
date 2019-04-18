using DomainValidation.Interfaces.Validation;
using System;
using DomainValidation.Validation;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class Produto : ISelfValidator
    {
        public Produto()
        {
            AvaliacoesCriticidade = new List<AvaliacaoCriticidade>();
            Fornecedores = new List<ProdutoFornecedor>();
            CriteriosQualificacao = new List<CriterioQualificacao>();
            CriteriosAvaliacao = new List<CriterioAvaliacao>();


            //Status = 1;

            //MinAprovado = 70;
            //MaxAprovado = 100;

            //MinAprovadoAnalise = 50;
            //MaxAprovadoAnalise = 69;

            //MinReprovado = 1;
            //MaxReprovado = 49;

            DtCriacao = DateTime.Now;
            DtAlteracao = DateTime.Now;


        }
        public int IdProduto { get; set; }
        public int IdSite { get; set; }
        public int IdResponsavel { get; set; }
        public string Nome { get; set; }
        public string Especificacao { get; set; }
        public string Tags { get; set; }

        public DateTime DtCriacao { get; set; }
        public DateTime DtAlteracao { get; set; }

        public int Status { get; set; }

        public int StatusCriterioAvaliacao { get; set; }
        public int StatusCriterioQualificacao { get; set; }

        public int MinAprovado { get; set; }
        public int MaxAprovado { get; set; }

        public int MinAprovadoAnalise { get; set; }
        public int MaxAprovadoAnalise { get; set; }

        public int MinReprovado { get; set; }
        public int MaxReprovado { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool IsValid()
        {
            return ValidationResult.IsValid;
        }

        #region Relacionamentos

        public virtual Usuario Responsavel { get; set; }

        public virtual Site Site { get; set; }

        public virtual ICollection<AvaliacaoCriticidade> AvaliacoesCriticidade { get; set; }

        public virtual ICollection<ProdutoFornecedor> Fornecedores { get; set; }

        public virtual ICollection<CriterioAvaliacao> CriteriosAvaliacao { get; set; }
        public virtual ICollection<CriterioQualificacao> CriteriosQualificacao { get; set; }


        #endregion
    }
}
