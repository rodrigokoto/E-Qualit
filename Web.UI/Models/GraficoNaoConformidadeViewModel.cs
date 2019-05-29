using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Models
{
    public class GraficoNaoConformidadeViewModel
    {
        public int IdTipoGrafico { get; set; }
        public int IdTipoNaoConformidade { get; set; }
        public DateTime DtDe { get; set; }
        public DateTime DtAte { get; set; }
        public int EstiloGrafico { get; set; }
        public Dictionary<string,string> Parametros { get; set; }
    }
}