using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Models
{
    public class PendenciaViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int IdResponsavel { get; set; }
        public string Modulo { get; set; }

        public string Url { get; set; }
    }
}