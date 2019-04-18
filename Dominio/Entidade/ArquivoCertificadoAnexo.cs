namespace Dominio.Entidade
{
    public class ArquivoCertificadoAnexo
    {
        public int IdArquivoCertificadoAnexo { get; set; }
        public int IdAnexo { get; set; }
        public int IdCalibracao { get; set; }

        #region Relacionamento
        public virtual Calibracao Calibracao { get; set; }
        public virtual Anexo Anexo { get; set; }

        #endregion
    }
}
