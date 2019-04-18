namespace Dominio.Entidade
{
    public partial class DocUsuarioVerificaAprova
    {
        public int IdDocUsuarioVerificaAprova { get; set; }
        public int IdDocumento { get; set; }
        public int IdUsuario { get; set; }
        public string TpEtapa { get; set; }
        public bool? FlAprovou { get; set; }
        public bool? FlVerificou { get; set; }
        public bool? Ativo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual DocDocumento DocDocumento { get; set; }
        public bool? deletar { get; set; }
    }
}
