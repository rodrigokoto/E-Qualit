namespace Dominio.Entidade
{
    public class ArquivosDeEvidencia
    {
        public int IdArquivosDeEvidencia { get; set; }

        public int IdRegistroConformidade { get; set; }
        public int IdAnexo { get; set; }
        public string TipoRegistro { get; set; }

        #region Relacionamento

        public virtual RegistroConformidade RegistroConformidade { get; set; }

        public virtual Anexo Anexo { get; set; }

        #endregion
    }
}
