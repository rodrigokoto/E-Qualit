using System;

namespace Dominio.Entidade
{
    public class Licenca
    {
        public int IdLicenca { get; set; }

        public int IdAnexo { get; set; }

        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }

        #region Relacionamento
        public virtual Anexo Anexo { get; set; }

        #endregion
    }
}