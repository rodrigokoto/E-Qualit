namespace Dominio.Entidade
{
    public class ClienteContrato
    {
        public int IdClienteContrato { get; set; }
        public int IdCliente { get; set; }
        public int IdAnexo { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Anexo Contrato { get; set; }
    }
}
