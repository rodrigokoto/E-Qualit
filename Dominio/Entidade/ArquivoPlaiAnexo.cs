namespace Dominio.Entidade
{
    public class ArquivoPlaiAnexo
    {
        public int IdArquivoPlaiAnexo { get; set; }
        public int IdAnexo { get; set; }
        public int IdPlai { get; set; }

        //informação somente da tela, solicitando para apagar
        public int ApagarAnexo { get; set; } = 0;

        #region Relacionamento
        public virtual Plai Plai { get; set; }
        public virtual Anexo Anexo { get; set; }

        #endregion
    }
}
