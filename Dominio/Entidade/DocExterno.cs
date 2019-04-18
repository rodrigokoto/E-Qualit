namespace Dominio.Entidade
{
    public class DocExterno
    {
        public int IdDocExterno { get; set; }

        public int IdAnexo { get; set; }

        public string LinkDocumentoExterno { get; set; }

        #region Relacionamento
        public virtual Anexo Anexo { get; set; }

        #endregion
    }
}
