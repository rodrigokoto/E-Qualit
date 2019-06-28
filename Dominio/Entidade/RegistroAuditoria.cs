using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidade
{
    public class RegistroAuditoria
    {
        public long IdRegAuditoria { get; set; }
        public int IdPlai { get; set; }
        public DateTime? DtInclusao { get; set; }
        public int IdGestor { get; set; }
        public long? IdFilaEnvio { get; set; }
        public int IdUsuarioInclusao { get; set; }
    }
}
