using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidade
{
    public class PlaiGerentes
    {
        public int IdPlaiGerente { get; set; }
        public int IdPlai { get; set; }
        public int IdUsuario { get; set; }

        public virtual Plai Plai { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
