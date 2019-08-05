namespace Dominio.Entidade
{
    public class ArquivoNaoConformidadeAnexo
    {
        public int IdArquivoNaoConformidadeAnexo { get; set; }
        public int IdAnexo { get; set; }
        public int IdRegistroConformidade { get; set; }

        //informação somente da tela, solicitando para apagar
        public int ApagarAnexo { get; set; } = 0;

        #region Relacionamento
        public virtual RegistroAcaoImediata NaoConformidade { get; set; }
        public virtual Anexo Anexo { get; set; }

        #endregion
    }
}
