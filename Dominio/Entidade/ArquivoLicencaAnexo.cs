namespace Dominio.Entidade
{
    public class ArquivoLicencaAnexo
    {
        public int IdArquivoLicencaAnexo { get; set; }
        public int IdAnexo { get; set; }
        public int IdLicenca { get; set; }

        //informação somente da tela, solicitando para apagar
        public int ApagarAnexo { get; set; } = 0;

        #region Relacionamento
        public virtual Licenca Licenca { get; set; }
        public virtual Anexo Anexo { get; set; }

        #endregion
    }
}
