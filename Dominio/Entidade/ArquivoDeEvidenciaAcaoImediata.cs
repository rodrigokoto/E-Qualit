namespace Dominio.Entidade
{
    public class ArquivoDeEvidenciaAcaoImediata
    {
        public int IdArquivoDeEvidenciaAcaoImediata { get; set; }
        public int IdAcaoImediata { get; set; }
        public int IdAnexo { get; set; }

        //informação somente da tela, solicitando para apagar
        public int ApagarAnexo { get; set; } = 0;

        public virtual RegistroAcaoImediata AcaoImediata { get; set; }
        public virtual Anexo Anexo { get; set; }
    }
}
