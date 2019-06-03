using Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Models
{
    public class PdfAcaoCorreticaViewModel
    {
       public RegistroConformidade AcaoCorretiva { get; set; }
       public string LogoCliente { get; set; }
    }
}