namespace Dominio.Entidade
{
    public class UsuarioCargo
    {
        public int IdUsuarioProcesso { get; set; }
        public int IdUsuario { get; set; }
        public int IdCargo { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual Usuario Usuario { get; set; }
    } 
}
