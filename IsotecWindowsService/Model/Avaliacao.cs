using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsotecWindowsService
{
    public class Avaliacao
    {
        public int IdFornecedor { get; set; }
        public DateTime DtProximaAvaliacao { get; set; }    
        public int IdUsuarioAvaliacao { get; set; }

        public string GuidAvaliacao { get; set; }
    }
}
