using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidade
{
    public class UsuarioSenha
    {
        public int IdUsuarioSenha { get; set; }
        public int IdUsuario { get; set; }
        public string CdSenha { get; set; }
        public DateTime DtInclusaoSenha { get; set; }

        public virtual Usuario Usuario { get; set; }
        
    }
}
