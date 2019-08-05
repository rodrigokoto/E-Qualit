namespace Dominio.Entidade
{
    public class ArquivoInstrumentoAnexo
    {
        public int IdArquivoInstrumentoAnexo { get; set; }
        public int IdAnexo { get; set; }
        public int IdInstrumento { get; set; }

        //informação somente da tela, solicitando para apagar
        public int ApagarAnexo { get; set; } = 0;

        #region Relacionamento
        public virtual Instrumento Instrumento { get; set; }
        public virtual Anexo Anexo { get; set; }

        #endregion
    }
}
