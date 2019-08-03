namespace Dominio.Entidade
{
    public class ArquivoAcaoCorretivaAnexo
    {
        public int IdArquivoAcaoCorretivaAnexo { get; set; }
        public int IdAnexo { get; set; }
        public int IdRegistroConformidade { get; set; }

        //informação somente da tela, solicitando para apagar
        public int ApagarAnexo { get; set; } = 0;

        #region Relacionamento
        public virtual RegistroConformidade AcaoCorretiva { get; set; }
        public virtual Anexo Anexo { get; set; }

        #endregion
    }
}
