namespace Dominio.Entidade
{
    public class ArquivoDocDocumentoAnexo
    {
        public int IdArquivooControleDeDocumentoAnexo { get; set; }
        public int IdAnexo { get; set; }
        public int IdoControleDeDocumento { get; set; }

        //informação somente da tela, solicitando para apagar
        public int ApagarAnexo { get; set; } = 0;

        #region Relacionamento
        public virtual DocDocumento DocDocumento { get; set; }
        public virtual Anexo Anexo { get; set; }

        #endregion
    }
}
