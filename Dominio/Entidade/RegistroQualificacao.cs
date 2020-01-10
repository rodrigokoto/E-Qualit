using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidade
{
    public class RegistroQualificacao
    {
        [Key]
        public long IdRegQualificacao { get; set; }
        public int IdUsuarioAvaliacao { get; set; }

        public int IdAvaliaCriterio { get; set; }
        public DateTime? DtInclusao { get; set; }
        public int IdFornecedor { get; set; }
        public long? IdFilaEnvio { get; set; }
        public int IdUsuarioInclusao { get; set; }
    }
}
