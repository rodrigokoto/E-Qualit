using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidade
{
    public class RegistroLicenca
    {
        [Key]
        public int IdRegLicenca { get; set; }
        
        public string GuidLicenca { get; set; }
        public DateTime? DtInclusao { get; set; }
        public int IdResponsavel { get; set; }
        public long? IdFilaEnvio { get; set; }
        public int IdUsuarioInclusao { get; set; }
    }
}
