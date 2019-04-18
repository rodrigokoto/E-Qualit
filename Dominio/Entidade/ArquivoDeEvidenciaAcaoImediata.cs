namespace Dominio.Entidade
{
    public class ArquivoDeEvidenciaAcaoImediata
    {
        public int IdArquivoDeEvidenciaAcaoImediata { get; set; }
        public int IdAcaoImediata { get; set; }
        public int IdAnexo { get; set; }

        public virtual RegistroAcaoImediata AcaoImediata { get; set; }
        public virtual Anexo Anexo { get; set; }
    }
}
