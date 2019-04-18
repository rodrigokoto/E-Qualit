using System;

namespace Dominio.Entidade
{
    public class PlaiProcessoNorma
    {
        public int IdPlaiProcessoNorma { get; set; }
        public int IdPlai { get; set; }
        public virtual Plai Plai { get; set; }
        public int IdProcesso { get; set; }
        public string NomeProcesso { get; set; }
        public virtual Processo Processo { get; set; }
        public int? IdNorma { get; set; }
        public virtual Norma Norma { get; set; }
        public DateTime Data { get; set; }
        public bool Ativo { get; set; }
    }
}
