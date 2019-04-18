namespace Dominio.Entidade
{
    public class UsuarioAnexo
    {
        public int IdUsuarioAnexo { get; set; }
        public int IdUsuario { get; set; }
        public int IdAnexo { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Anexo Anexo { get; set; }
    }
}
