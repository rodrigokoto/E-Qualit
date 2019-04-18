namespace Dominio.Entidade
{
    public class NormaProcesso
    {
        public int IdNormaProcesso { get; set; }
        public int IdProcesso { get; set; }
        public string Requisitos { get; set; }
        public virtual Processo Processo { get; set; }
        public int IdNorma { get; set; }
        public virtual Norma Norma { get; set; }
    }
}
