using System;

namespace Dominio.Entidade
{
    public class AvaliaCriterioAvaliacao
    {
        public AvaliaCriterioAvaliacao()
        {
            DtAvaliacao = DateTime.Now;            
        }

        public int IdAvaliaCriterioAvaliacao { get; set; }

        public int IdCriterioAvaliacao { get; set; }

        public int IdFornecedor { get; set; }

        public int ? NotaAvaliacao { get; set; }

        public DateTime DtAvaliacao { get; set; }
        public DateTime DtProximaAvaliacao { get; set; }
        
        public int ? IdUsuarioAvaliacao { get; set; }

        #region Relacionamentos

        public virtual CriterioAvaliacao CriterioAvaliacao { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual Usuario UsuarioAvaliacao { get; set; }

        #endregion
    }
}
