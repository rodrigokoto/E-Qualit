using System;

namespace Dominio.Entidade
{
    public class DocumentoAssunto
    {
        public int Id { get; set; }
        public int IdDocumento { get; set; }
        public string Revisao { get; set; }
        public string Descricao { get; set; }
        public DateTime DataAssunto { get; set; }

        public virtual DocDocumento Documento { get; set; }
    }
}
