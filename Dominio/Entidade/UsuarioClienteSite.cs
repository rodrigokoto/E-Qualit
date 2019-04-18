namespace Dominio.Entidade
{
    public class UsuarioClienteSite
    {
        public int IdUsuarioClienteSite { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public int? IdSite { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Site Site { get; set; }
    }
}
