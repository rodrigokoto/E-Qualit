using System;
using System.Collections.Generic;

namespace Dominio.Entidade
{
    public class CriterioAvaliacao
    {
        public CriterioAvaliacao()
        {
            DtCriacao = DateTime.Now;
            DtAlteracao = DateTime.Now;
        }

        public int IdCriterioAvaliacao { get; set; }
        public int IdProduto { get; set; }

        public string Titulo { get; set; }

        public DateTime DtCriacao { get; set; }
        public DateTime DtAlteracao { get; set; }

        public bool Ativo { get; set; }

        #region Relacionamentos

        public virtual Produto Produto { get; set; }
        public virtual ICollection<AvaliaCriterioAvaliacao> Avaliacoes { get; set; }

        #endregion


    }
}
        