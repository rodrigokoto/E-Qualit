using System;

namespace Dominio.Entidade
{
    public class DocRotina
    {
        public int IdDocRotina { get; set; }

        public int IdDocumento { get; set; }

        public String OQue { get; set; }
        public String Quem { get; set; }
        public String Registro { get; set; }
        public String Como { get; set; }
        public String Item { get; set; }

        #region Relacionamento
        public virtual DocDocumento DocDocumento { get; set; }
        #endregion
    }
}
