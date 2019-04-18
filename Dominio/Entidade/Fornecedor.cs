using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class Fornecedor
    {
        
        public Fornecedor()
        {
            AvaliaCriteriosQualificacao = new List<AvaliaCriterioQualificacao>();
            AvaliaCriteriosAvaliacao = new List<AvaliaCriterioAvaliacao>();
        }
        public int IdFornecedor { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }

        public int IdSite { get; set; }
        public int ? IdProcesso { get; set; } //Departamento 

        public int ? IdUsuarioAvaliacao { get; set; }

        #region Relacionamentos

        public virtual Site Site { get; set; }

        public virtual Processo Processo { get; set; }

        public virtual Usuario UsuarioAvaliacao { get; set; }

        public virtual ICollection<ProdutoFornecedor> Produtos { get; set; }

        public virtual ICollection<AvaliaCriterioQualificacao> AvaliaCriteriosQualificacao { get; set; }

        public virtual ICollection<AvaliaCriterioAvaliacao> AvaliaCriteriosAvaliacao { get; set; }

        #endregion

    }
}
