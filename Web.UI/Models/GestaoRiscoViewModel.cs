using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Models
{
    public class GestaoRiscoViewModel
    {
        public int IdRegistro { get; set; }
        public int NuRegistro { get; set; }
        public string NomeEmissor { get; set; }
        public DateTime DtEmissao { get; set; }
        public DateTime? DtEncerramento { get; set; }
        public byte? StatusEtapa { get; set; }
        public string Responsavel { get; set; }
        public string Tags { get; set; }
    }
}