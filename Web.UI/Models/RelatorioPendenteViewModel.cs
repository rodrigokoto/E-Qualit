using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Models
{
    public class RelatorioPendenteViewModel
    {

        public string Nr { get; set; }
        public DateTime dtEmissao { get; set; }
        public string ProcessoNome { get; set; }
        public DateTime dtInclusao { get; set; }
        public string Responsavel { get; set; }
    }
}