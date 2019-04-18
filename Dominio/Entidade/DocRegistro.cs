using System;

namespace Dominio.Entidade
{
    public class DocRegistro
    {
        public int IdDocRegistro { get; set; }

        public int IdDocumento { get; set; }

        public String Identificar { get; set; }
        public String Armazenar { get; set; }
        public String Proteger { get; set; }
        public String Retencao { get; set; }
        public String Recuperar { get; set; }
        public String Disposicao { get; set; }

        #region Relacionamento
        public virtual DocDocumento DocDocumento { get; set; }
        #endregion
    }
}