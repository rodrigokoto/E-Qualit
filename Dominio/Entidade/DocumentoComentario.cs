using System;

namespace Dominio.Entidade
{
    public class DocumentoComentario
    {
        public int Id { get; set; }
        public int IdDocumento { get; set; }
        
        public string Descricao { get; set; }
        public DateTime DataComentario { get; set; }

        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        public virtual DocDocumento Documento { get; set; }
    }
}
