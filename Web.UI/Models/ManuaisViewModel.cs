using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Models
{
    public class ManuaisViewModel 
    {
        public int IdDocumento { get; set; }
        public string Titulo { get; set; }
        public string Processo { get; set; }
        public string TexToDoc { get; set; }
    }
}