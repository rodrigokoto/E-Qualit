namespace Dominio.Entidade
{
    public class ArquivosEvidenciaCriterioQualificacao
    {
        public int IdArquivosEvidenciaCriterioQualificacao { get; set; }

        public int IdAvaliaCriterioQualificacao { get; set; }
        public int IdAnexo { get; set; }
        

        #region Relacionamento

        public virtual AvaliaCriterioQualificacao AvaliaCriterioQualificacao { get; set; }

        public virtual Anexo Anexo { get; set; }

        #endregion
    }
}
