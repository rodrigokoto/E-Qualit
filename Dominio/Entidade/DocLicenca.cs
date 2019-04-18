namespace Dominio.Entidade
{
    public class DocLicenca
    {
        public int IdDocLicenca { get; set; }
        public int IdDocumento { get; set; }
        public int IdAnexo { get; set; }

        #region Relacionamento
        public virtual DocDocumento DocDocumento { get; set; }
        public virtual Anexo Licenca { get; set; }

        #endregion
    }
}
