using System;

namespace Dominio.Entidade
{
    public class HistoricoCriterioAvaliacao
    {

        public HistoricoCriterioAvaliacao()
        {
            DtCriacao = DateTime.Now;
        }

        public int IdHistoricoCriterioAvaliacao { get; set; }
        public int IdCriterioAvaliacao { get; set; }

        public int Nota { get; set; }
        public DateTime DtCriacao { get; set; }

        #region Relacionamentos

        public virtual CriterioAvaliacao CriterioAvaliacao { get; set; }

        #endregion

    }
}

