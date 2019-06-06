using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidade
{
    public class ComentarioAcaoImediata
    {
        //[Key]
        public int IdComentarioAcaoImediata { get; set; }
        
        public int IdAcaoImediata { get; set; }
        public string Motivo { get; set; }
        public string Orientacao { get; set; }
        public string UsuarioComentario { get; set; }
        public string DataComentario { get; set; }
        //public virtual DocDocumento Documento { get; set; }
        public virtual RegistroAcaoImediata RegistroAcaoImediata { get; set; }

    }
}
