using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Models
{
    public class MenuProcessoViewModel
    {
       
        public int IdProcesso { get; set; }

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public int IdCategoria { get; set; }
    }
}