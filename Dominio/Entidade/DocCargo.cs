namespace Dominio.Entidade
{
    public class DocumentoCargo
    {
        public int Id { get; set; }
        public int IdCargo { get; set; }
        public int IdDocumento { get; set; }
        public virtual DocDocumento DocDocumento { get; set; }
    }
}
