namespace Dominio.Entidade
{
    public class ClienteLogo
    {
        public int IdClienteLogo { get; set; }
        public int IdCliente { get; set; }
        public int IdAnexo { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Anexo Anexo { get; set; }
    }
}
