using Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Models
{
    public class LayoutImpressaoViewModel
    {
        public bool IsImpressaoControlada { get; set; }
        public string LogoCliente { get; set; }
        public DocDocumento Documento { get; set; }
        public List<DocDocumento> docDocumentos { get; set; }
    }
}